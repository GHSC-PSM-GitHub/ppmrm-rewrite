namespace PPMRm.DataSync
{
    using System;
    using System.Threading.Tasks;
    using Paillave.Etl.FileSystem;
    using Paillave.Etl.Zip;
    using Paillave.Etl.TextFile;
    using Paillave.Etl.Core;
    using EtlNet.GZip;
    using PPMRm.ARTMIS.ETL;
    using PPMRm.ARTMIS;
    using System.Collections.Generic;
    using Microsoft.EntityFrameworkCore;
    using Paillave.Etl.EntityFrameworkCore;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Marten;
    using PPMRm.Items;
    using Marten.Events.Projections;
    using System.Threading;
    using Baseline.Dates;

    public class PPMRmDbContext : DbContext
    {
        public PPMRmDbContext()
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(Program.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Item> Items { get; set; }
    }
    class Program
    {
        public const string ConnectionString = "Host=localhost;Port=5432;Database=ppmrm_may;User ID=postgres;Password=mysecretpassword;";


        static List<OrderEto> orderEvents;
        static List<Item> items;
        static List<string> Categories = new List<string>{ "ACTs","mRDTs", "Other Pharma", "Severe Malaria Meds", "SP" };

        async static Task Main(string[] args)
        {
            //var store = new DocumentStore(new PPMRmStoreOptions(ConnectionString, null));
            //using var session = store.OpenSession();

            //var repo = new ARTMIS.PeriodShipments.PeriodShipmentRepository(session);

            //var shipment = await repo.GetAsync("AGO", 202112);

            //if (shipment == null) { throw new Exception(); };

            //var decShipments = shipment.Shipments.Where(s => s.PPMRmProductId != null && (s.ShipmentDate >= new DateTime(2021, 12, 01) || s.ShipmentDateType != ARTMISConsts.OrderDeliveryDateTypes.ActualDeliveryDate)).ToList();
            ////
            //await SeedOrders(args);
            //await SeedItems(new string[] { @"..\..\..\data" });
            // using var session = store.OpenSession();
            //// var order = session.Query<ARTMIS.Orders.Order>().Where(o => o.OrderNumber == "RO10137105").SingleOrDefault();
            // var decemberShipments = session.Query<ARTMIS.Orders.Order>().Where(o => o.DisplayDate > new DateTime(2021, 12, 1) && o.Lines.Any() && o.CountryId == "Angola");

            // Console.WriteLine($"December shipments: {decemberShipments.Count()}");
            // foreach (var order in decemberShipments.ToList())
            // {
            //     Console.WriteLine($"{order.RONumber}-{order.OrderNumber}-{order.CountryId}-{order.DisplayDate.Value.ToShortDateString()}-{order.DeliveryDateType}");
            //     foreach (var line in order.Lines.OrderBy(l => l.LineNumber))
            //     {
            //         Console.WriteLine($"{line.LineNumber} - {line.ProductId} - {line.OrderedQuantity}");
            //     }

            // }
            await SeedOrders(args);

            Console.ReadLine();

        }
        async static Task SeedOrders(string[] args)
        {
            orderEvents = new List<OrderEto>();
            var processRunner = StreamProcessRunner.Create<string>(DefineProcess);
            await processRunner.ExecuteAsync(@"../../../data");
            var store = new DocumentStore(new PPMRmStoreOptions(ConnectionString, null));
            using var session = store.OpenSession();
            var sortedEvents = orderEvents.OrderBy(e => e.FileName).ThenBy(e => e.LineNumber).Select(e => OrderLineEvent.Create(e));
            var changeSets = sortedEvents.GroupBy(e => e.OrderLineId).ToDictionary(g => g.Key, g => g.ToList());

            foreach (var c in changeSets)
            {
                
                try
                {
                    var state = await session.Events.FetchStreamStateAsync(c.Key);
                    //var existing = await session.LoadAsync<ARTMIS.OrderLines.OrderLine>(c.Key);
                    if(state == null)
                    {
                        session.Events.StartStream<ARTMIS.OrderLines.OrderLine>(c.Key, c.Value.OrderBy(e => e.EventTimestamp));
                    }
                    else
                    {
                        session.Events.Append(c.Key, c.Value.OrderBy(e => e.EventTimestamp));
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine($"Changeset: {c.Key} already exists1");
                    return;
                }

            }
            await session.SaveChangesAsync();
            //var firstEvent = changeSets.First().Value.First();
            ////Console.WriteLine($"Total: - {orderEvents.Count}");
            //Console.WriteLine($"First: - {firstEvent.FileName} - {firstEvent.FileTimestamp} - {firstEvent.EventTimestamp} - {firstEvent.PeriodId} - {firstEvent.CountryId} - {firstEvent.OrderLineId}");


            ////var eventsByOrderLine = sortedEvents.GroupBy(e => e.OrderLineId).ToDictionary(e => e.Key, e => e.ToList());
            ////Console.WriteLine($"Unique orders - {eventsByOrderLine.Count()}");
            ////session.Events.StartStream("order", sortedEvents);
            //await session.SaveChangesAsync();
            Console.WriteLine($"Completed importing ARTMIS Changesets!");
            Console.ReadLine();
        }
        async static Task SeedItems(string[] args)
        {
            orderEvents = new List<OrderEto>();
            items = new List<Item>();
            var processRunner = StreamProcessRunner.Create<string>(ProcessItems);
            using (var dbCtx = new PPMRmDbContext())
            {
                using var store = DocumentStore.For(ConnectionString);
                using var session = store.LightweightSession();

                dbCtx.Database.EnsureCreated();
                var executionOptions = new ExecutionOptions<string>
                {
                    Resolver = new SimpleDependencyResolver().Register<DbContext>(dbCtx),
                };
                var res = await processRunner.ExecuteAsync(@"..\..\..\data", executionOptions);
                session.StoreObjects(items.Where(i => i.ProductId != null));
                await session.SaveChangesAsync();
                await dbCtx.SaveChangesAsync();
            }
            
            Console.WriteLine("Hello World!");
        }

        private static void DefineProcess(ISingleStream<string> contextStream)
        {
            var orderStream = contextStream
                .CrossApplyFolderFiles("list all required files", "202310*.tar.gz", true)
                .CrossApplyGZipFiles("extract files from zip", "*order*.txt")
                .CrossApplyTextFile("parse file", 
                    FlatFileDefinition.Create(i => new OrderEto
                    {
                        FileName = i.ToSourceName(),
                        LineNumber = i.ToLineNumber(),
                        EnterpriseCode = i.ToColumn(ARTMISConsts.OrderHeaders.EnterpriseCode),
                        ConsigneeCompanyName = i.ToColumn(ARTMISConsts.OrderHeaders.ConsigneeCompanyName),
                        ActualDeliveryDate = i.ToOptionalDateColumn(ARTMISConsts.OrderHeaders.ActualDeliveryDate, ARTMISConsts.DateFormat),
                        ActualShipDate = i.ToOptionalDateColumn(ARTMISConsts.OrderHeaders.ActualShipDate, ARTMISConsts.DateFormat),
                        ChangeIndicator = (ChangeIndicator) i.ToNumberColumn<int>(ARTMISConsts.OrderHeaders.ChangeIndicator, ""),
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
                //.Do("display zip file name on console", i => Console.WriteLine($"{i.FileDateTimeOffset.Date.ToShortDateString()} - {i.LineNumber} - {i.OrderNumber}"));
                .Do("add to list", i => orderEvents.Add(i));
            // TODO: Define the ETL process here
            
        }

        static void ProcessItems(ISingleStream<string> contextStream)
        {
            var itemStream = contextStream
                .CrossApplyFolderFiles("list all required files", "2021*.tar.gz", true)
                .CrossApplyGZipFiles("extract files from tar", "*item*.txt")
                .CrossApplyTextFile("parse item file",
                    FlatFileDefinition.Create(i => new ItemEto
                    {
                        ItemId = i.ToColumn(ARTMISConsts.ItemHeaders.ItemId),
                        ProductId = i.ToColumn(ARTMISConsts.ItemHeaders.ProductId),
                        BaseUnit = i.ToColumn(ARTMISConsts.ItemHeaders.BaseUnit),
                        BaseUnitMultiplier = i.ToNumberColumn<decimal>(ARTMISConsts.ItemHeaders.BaseUnitMultiplier, "."),
                        CatalogPrice = i.ToNumberColumn<decimal?>(ARTMISConsts.ItemHeaders.CatalogPrice, "."),
                        CountryOfOrigin = i.ToColumn(ARTMISConsts.ItemHeaders.CountryOfOrigin),
                        ManufacturerGTIN = i.ToColumn(ARTMISConsts.ItemHeaders.ManufacturerGTIN),
                        ManufacturerLocation = i.ToColumn(ARTMISConsts.ItemHeaders.ManufacturerLocation),
                        ManufacturerName = i.ToColumn(ARTMISConsts.ItemHeaders.ManufacturerName),
                        ProductName = i.ToColumn(ARTMISConsts.ItemHeaders.ProductName),
                        ProductNumberOfTreatments = i.ToNumberColumn<int?>(ARTMISConsts.ItemHeaders.ProductNumberOfTreatments, ""),
                        SupplierName = i.ToColumn(ARTMISConsts.ItemHeaders.SupplierName),
                        TracerCategory = i.ToColumn(ARTMISConsts.ItemHeaders.TracerCategory),
                        UOM = i.ToColumn(ARTMISConsts.ItemHeaders.UOM)

                    }).IsColumnSeparated('|'))
                .Select("select unique products", i => new Item
                {
                    Id = i.ProductId,
                    Name = i.ProductName,
                    BaseUnit = i.BaseUnit,
                    BaseUnitMultiplier = i.BaseUnitMultiplier,
                    NumberOfTreatments = i.ProductNumberOfTreatments,
                    TracerCategory = i.TracerCategory,
                    UOM = i.UOM,
                    ProductId = ARTMISConsts.PPMRmProductMappings.GetOrDefault(i.ProductId)

                })
                .Distinct("remove duplicates", i => i.Id)
                //.Do("display zip file name on console", i => Console.WriteLine($"{i.Id}-{ i.Name} - {i.BaseUnitMultiplier}"))
                .Do("save to db", i => items.Add(i));
             //var productStream = itemStream
             //       .Distinct("filter duplicates", i => i.ProductId)
             //       .Select("select unique items", i => new Item
             //       {

             //       });
        }
    }

    
}
