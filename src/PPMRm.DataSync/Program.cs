﻿namespace PPMRm.DataSync
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

    public class PPMRmDbContext : DbContext
    {
        public PPMRmDbContext()
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(@"Host=localhost;Port=5432;Database=ppmrm_artmis;User ID=postgres;Password=admin;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Item> Items { get; set; }
    }
    class Program
    {

        static List<OrderEto> orderEvents;
        static List<Item> items;
        static List<string> Categories = new List<string>{ "ACTs","mRDTs", "Other Pharma", "Severe Malaria Meds", "SP" };
        async static Task Main(string[] args)
        {
            orderEvents = new List<OrderEto>();
            items = new List<Item>();
            var processRunner = StreamProcessRunner.Create<string>(ProcessItems);
            using (var dbCtx = new PPMRmDbContext())
            {
                dbCtx.Database.EnsureCreated();
                var executionOptions = new ExecutionOptions<string>
                {
                    Resolver = new SimpleDependencyResolver().Register<DbContext>(dbCtx),
                };
                var res = await processRunner.ExecuteAsync(args[0], executionOptions);
                dbCtx.Items.AddRange(items.Where(i => Categories.Contains( i.TracerCategory)));
                await dbCtx.SaveChangesAsync();
            }
            
            Console.WriteLine("Hello World!");
        }

        private static void DefineProcess(ISingleStream<string> contextStream)
        {
            var orderStream = contextStream
                .CrossApplyFolderFiles("list all required files", "202112*.tar.gz", true)
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
                        OrderedQuantity = i.ToNumberColumn<decimal?>(ARTMISConsts.OrderHeaders.OrderedQuantity, "."),
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
                .Do("display zip file name on console", i => Console.WriteLine($"{i.FileDateTimeOffset.Date.ToShortDateString()} - {i.LineNumber} - {i.OrderNumber}"));
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
                    UOM = i.UOM

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

    public class Item
    {
        [Key]
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string BaseUnit { get; set; }
        [Required]
        public decimal BaseUnitMultiplier { get; set; }
        public string TracerCategory { get; set; }
        public string UOM { get; set; }
        public int? NumberOfTreatments { get; set; }
        public string ProductId { get; set; }

    }
}
