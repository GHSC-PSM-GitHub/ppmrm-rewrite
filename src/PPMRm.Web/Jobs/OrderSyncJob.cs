using Marten;
using PPMRm.Jobs;
using System.Threading.Tasks;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;
using System;
using Paillave.Etl.FileSystem;
using Paillave.Etl.Zip;
using Paillave.Etl.TextFile;
using Paillave.Etl.Core;
using EtlNet.GZip;
using PPMRm.ARTMIS;
using PPMRm.ARTMIS.ETL;
using System.Linq;
using System.IO;
using System.Collections.Generic;

namespace PPMRm.Web.Jobs
{
    public class ProcessArgs
    {
        public long EventTimestamp { get; set; }
        public long FileTimestamp { get; set; }
    }
    public class OrderSyncJob : AsyncBackgroundJob<OrderSyncJobArgs>, ITransientDependency
    {
        const string OrderFilesPattern = $"*order*{ARTMISConsts.FileExtensions.Txt}";
        const string ItemFilesPattern = $"*item*{ARTMISConsts.FileExtensions.Txt}";

        const string ARTMISConnector = "ARTMIS";

        IDocumentSession Session { get; }
        long LastFileTimestamp { get; set; }
        long LastEventTimestamp { get; set; }
        public OrderSyncJob(IDocumentStore documentStore)
        {
            Session = documentStore.OpenSession();
            var lastEvent = Session.Events.QueryAllRawEvents().OrderByDescending(e => e.Sequence).FirstOrDefault()?.Data as OrderLineEvent;
            LastFileTimestamp = lastEvent?.FileTimestamp ?? default;
            LastEventTimestamp = lastEvent?.EventTimestamp ?? default;
        }
        async public override Task ExecuteAsync(OrderSyncJobArgs args)
        {
            // Get Last Event Timestamp from Stream
            var lastEvent = await Session.Events.QueryAllRawEvents().OrderByDescending(e => e.Sequence).FirstOrDefaultAsync();
            var lastEventFileTimestamp = (lastEvent.Data as OrderLineEvent)?.EventTimestamp;

            // setup fileProvider for ARTMIS Changeset

            
            var changesetFilePattern = $"{args.PeriodId}*{ARTMISConsts.FileExtensions.TarGz}";
            // TODO: Change this to SFTPFileProvider
            var changesetFileProvider = new FileSystemFileValueProvider(ARTMISConnector, ARTMISConnector, Path.Combine(Environment.CurrentDirectory, "data"), changesetFilePattern);

            // setup connectors from config
            var executionOptions = new ExecutionOptions<string>
            {
                Connectors = new FileValueConnectors()
                        .Register(changesetFileProvider),
                Resolver = new SimpleDependencyResolver()
                    .Register(new ProcessArgs
                    {
                        EventTimestamp = (lastEvent.Data as OrderLineEvent)?.EventTimestamp ?? 0,
                        FileTimestamp = (lastEvent.Data as OrderLineEvent)?.FileTimestamp ?? 0
                    })
                    .Register(Session)
            };
            var processRunner = StreamProcessRunner.Create<string>(DefineProcess);
            var res = await processRunner.ExecuteAsync("transmitted parameter", executionOptions);
            if (!res.Failed)
                await Session.SaveChangesAsync();
        }

        private void DefineProcess(ISingleStream<string> contextStream)
        {
            var orderStream = contextStream
                .FromConnector("get ARTMIS period changesets", ARTMISConnector)
                .CrossApplyGZipFiles("extract order files from tar.gz", OrderFilesPattern)
                .CrossApplyTextFile("parse file",
                    FlatFileDefinition.Create(i => new OrderEto
                    {
                        FileName = i.ToSourceName(),
                        LineNumber = i.ToLineNumber(),
                        EnterpriseCode = i.ToColumn(ARTMISConsts.OrderHeaders.EnterpriseCode),
                        ConsigneeCompanyName = i.ToColumn(ARTMISConsts.OrderHeaders.ConsigneeCompanyName),
                        ActualDeliveryDate = i.ToOptionalDateColumn(ARTMISConsts.OrderHeaders.ActualDeliveryDate, ARTMISConsts.DateFormat),
                        ActualShipDate = i.ToOptionalDateColumn(ARTMISConsts.OrderHeaders.ActualShipDate, ARTMISConsts.DateFormat),
                        ChangeIndicator = (ChangeIndicator)i.ToNumberColumn<int>(ARTMISConsts.OrderHeaders.ChangeIndicator, ""),
                        EstimatedDeliveryDate = i.ToOptionalDateColumn(ARTMISConsts.OrderHeaders.EstimatedDeliveryDate, ARTMISConsts.DateFormat),
                        ExternalStatusStage = i.ToColumn(ARTMISConsts.OrderHeaders.ExternalStatusStage),
                        ExternalStatusStageSequence = i.ToColumn(ARTMISConsts.OrderHeaders.ExternalStatusStageSequence),
                        ItemId = i.ToColumn(ARTMISConsts.OrderHeaders.ItemId),
                        LatestEstimatedDeliveryDate = i.ToOptionalDateColumn(ARTMISConsts.OrderHeaders.LatestEstimatedDeliveryDate, ARTMISConsts.DateFormat),
                        LineTotal = i.ToNumberColumn<decimal?>(ARTMISConsts.OrderHeaders.LineTotal, ""),
                        OrderedQuantity = i.ToNumberColumn<decimal>(ARTMISConsts.OrderHeaders.OrderedQuantity, "."),
                        OrderLineNumber = i.ToNumberColumn<int>(ARTMISConsts.OrderHeaders.OrderLineNumber, ""),
                        OrderNumber = i.ToColumn(ARTMISConsts.OrderHeaders.OrderNumber),
                        OrderType = i.ToColumn(ARTMISConsts.OrderHeaders.OrderType),
                        ParentOrderEntryDate = i.ToOptionalDateColumn(ARTMISConsts.OrderHeaders.ParentOrderEntryDate, ARTMISConsts.DateFormat),
                        ParentRONumber = i.ToColumn(ARTMISConsts.OrderHeaders.ParentRONumber),
                        PODOIONumber = i.ToColumn(ARTMISConsts.OrderHeaders.PODOIONumber),
                        POReleasedForFulfillmentDate = i.ToOptionalDateColumn(ARTMISConsts.OrderHeaders.POReleasedForFulfillmentDate, ARTMISConsts.DateFormat),
                        PSMSourceApprovalDate = i.ToOptionalDateColumn(ARTMISConsts.OrderHeaders.PSMSourceApprovalDate, ARTMISConsts.DateFormat),
                        QACompletedDate = i.ToOptionalDateColumn(ARTMISConsts.OrderHeaders.QACompletedDate, ARTMISConsts.DateFormat),
                        QAInitiatedDate = i.ToOptionalDateColumn(ARTMISConsts.OrderHeaders.QAInitiatedDate, ARTMISConsts.DateFormat),
                        RecipientCountry = i.ToColumn(ARTMISConsts.OrderHeaders.RecipientCountry),
                        RequestedDeliveryDate = i.ToOptionalDateColumn(ARTMISConsts.OrderHeaders.RequestedDeliveryDate, ARTMISConsts.DateFormat),
                        RevisedAgreedDeliveryDate = i.ToOptionalDateColumn(ARTMISConsts.OrderHeaders.RevisedAgreedDeliveryDate, ARTMISConsts.DateFormat),
                        RONumber = i.ToColumn(ARTMISConsts.OrderHeaders.RONumber),
                        ROPrimeLineNumber = i.ToNumberColumn<int>(ARTMISConsts.OrderHeaders.ROPrimeLineNumber, ""),
                        StatusSequence = i.ToColumn(ARTMISConsts.OrderHeaders.StatusSequence),
                        UnitPrice = i.ToNumberColumn<decimal?>(ARTMISConsts.OrderHeaders.UnitPrice, "."),
                        UOM = i.ToColumn(ARTMISConsts.OrderHeaders.UOM)

                    }).IsColumnSeparated('|'))
                .Where("filter processed files", i => i.FileTimestamp > LastFileTimestamp);

            var orderLineStream = orderStream
                .Sort("sort events", i => i.EventTimestamp)
                .Select("select OrderLineEvent", i => OrderLineEvent.Create(i))
                .Aggregate("aggregate",
                        i => i.OrderLineId,
                        i => new { Key = i.OrderLineId, Events = new List<OrderLineEvent>() },
                        (orderLine, e) =>
                        {
                            orderLine.Events.Add(e);
                            return orderLine;
                        })
                .Do("add to session", (i) =>
                {
                    Console.WriteLine($"{i.Key} - {i.Aggregation.Events.First().FileName}");
                    //Session.Events.Append(i.Key, i.Aggregation.Events.OrderBy(x => x.EventTimestamp));
                });

                //.Do("display zip file name on console", i => Console.WriteLine($"{i.FileDateTimeOffset.Date.ToShortDateString()} - {i.LineNumber} - {i.OrderNumber}"));
                //.Do("add to list", i => orderEvents.Add(i));
            // TODO: Define the ETL process here

        }
    }
}
