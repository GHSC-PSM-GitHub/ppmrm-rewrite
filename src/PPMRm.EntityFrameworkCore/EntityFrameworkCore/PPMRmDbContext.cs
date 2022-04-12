using Microsoft.EntityFrameworkCore;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.IdentityServer.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement;
using Volo.Abp.TenantManagement.EntityFrameworkCore;
using Volo.Abp.BlobStoring.Database.EntityFrameworkCore;
using Volo.CmsKit.EntityFrameworkCore;
using PPMRm.Core;
using Volo.Abp.EntityFrameworkCore.Modeling;
using PPMRm.Products;
using PPMRm.PeriodReports;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace PPMRm.EntityFrameworkCore
{
    [ReplaceDbContext(typeof(IIdentityDbContext))]
    [ReplaceDbContext(typeof(ITenantManagementDbContext))]
    [ConnectionStringName("Default")]
    public class PPMRmDbContext : 
        AbpDbContext<PPMRmDbContext>,
        IIdentityDbContext,
        ITenantManagementDbContext
    {
        /* Add DbSet properties for your Aggregate Roots / Entities here. */
        
        #region Entities from the modules
        
        /* Notice: We only implemented IIdentityDbContext and ITenantManagementDbContext
         * and replaced them for this DbContext. This allows you to perform JOIN
         * queries for the entities of these modules over the repositories easily. You
         * typically don't need that for other modules. But, if you need, you can
         * implement the DbContext interface of the needed module and use ReplaceDbContext
         * attribute just like IIdentityDbContext and ITenantManagementDbContext.
         *
         * More info: Replacing a DbContext of a module ensures that the related module
         * uses this DbContext on runtime. Otherwise, it will use its own DbContext class.
         */
        
        //Identity
        public DbSet<IdentityUser> Users { get; set; }
        public DbSet<IdentityRole> Roles { get; set; }
        public DbSet<IdentityClaimType> ClaimTypes { get; set; }
        public DbSet<OrganizationUnit> OrganizationUnits { get; set; }
        public DbSet<IdentitySecurityLog> SecurityLogs { get; set; }
        public DbSet<IdentityLinkUser> LinkUsers { get; set; }
        
        // Tenant Management
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<TenantConnectionString> TenantConnectionStrings { get; set; }

        // Core Data Management
        public DbSet<Product> Products { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Program> Programs { get; set; }
        public DbSet<Period> Periods { get; set; }
        public DbSet<PeriodReport> PeriodReports { get; set; }

        #endregion

        public PPMRmDbContext(DbContextOptions<PPMRmDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            /* Include modules to your migration db context */

            builder.ConfigurePermissionManagement();
            builder.ConfigureSettingManagement();
            builder.ConfigureBackgroundJobs();
            builder.ConfigureAuditLogging();
            builder.ConfigureIdentity();
            builder.ConfigureIdentityServer();
            builder.ConfigureFeatureManagement();
            builder.ConfigureTenantManagement();

            // Configure CMSKit and Blobstoring
            builder.ConfigureBlobStoring();
            builder.ConfigureCmsKit();



            /* Configure your own tables/entities inside here */
            //builder.Entity<YourEntity>(b =>
            //{
            //    b.ToTable(PPMRmConsts.DbTablePrefix + "YourEntities", PPMRmConsts.DbSchema);
            //    b.ConfigureByConvention(); //auto configure for the base class props
            //    //...
            //});
            
            // Bitwise converter to convert SOHLevelValues
            var sohLevelConverter = new EnumToNumberConverter<SOHLevel, int>();
            builder.Entity<Product>(b =>
            {
                b.ToTable("Products",
                    PPMRmConsts.DbSchema);
                b.ConfigureByConvention(); //auto configure for the base class props
                b.Property(x => x.Name).IsRequired().HasMaxLength(128);
            });

            builder.Entity<Country>(b =>
            {
                b.ToTable(PPMRmConsts.DbTablePrefix + "Countries",
                    PPMRmConsts.DbSchema);
                b.ConfigureByConvention(); //auto configure for the base class props
                b.Property(x => x.Name).IsRequired().HasMaxLength(128);
                b.HasIndex(x => x.Name).IsUnique();
                b.Property(x => x.ARTMISName).IsRequired().HasMaxLength(128);
                b.HasIndex(x => x.ARTMISName).IsUnique();
                b.HasOne<Program>().WithMany().HasForeignKey(x => x.DefaultProgramId).IsRequired();
                b.HasMany(x => x.Products).WithOne().HasForeignKey(x => x.CountryId).IsRequired();
                b.HasMany(x => x.Programs).WithOne().HasForeignKey(x => x.CountryId).IsRequired();

            });

            builder.Entity<CountryProduct>(b =>
            {
                b.ToTable(PPMRmConsts.DbTablePrefix + "CountryProducs", PPMRmConsts.DbSchema);
                b.ConfigureByConvention();
                ;// ADD THE MAPPING FOR THE RELATION
                b.HasOne<Product>().WithMany().HasForeignKey(x => x.ProductId).IsRequired();
                b.HasKey(x => new{ x.CountryId, x.ProductId });
                b.HasIndex(x => new { x.CountryId, x.ProductId});
            });

            builder.Entity<CountryProgram>(b =>
            {
                b.ToTable(PPMRmConsts.DbTablePrefix + "CountryPrograms", PPMRmConsts.DbSchema);
                b.ConfigureByConvention();
                ;// ADD THE MAPPING FOR THE RELATION
                b.HasOne<Program>().WithMany().HasForeignKey(x => x.ProgramId).IsRequired();
                b.HasKey(x => new { x.CountryId, x.ProgramId });
                b.HasIndex(x => new { x.CountryId, x.ProgramId });
            });

            builder.Entity<Program>(b =>
            {
                b.ToTable(PPMRmConsts.DbTablePrefix + "Programs",
                    PPMRmConsts.DbSchema);
                b.ConfigureByConvention(); //auto configure for the base class props
                b.Property(x => x.Name).IsRequired().HasMaxLength(128);
                b.HasIndex(x => x.Name).IsUnique();
            });

            builder.Entity<Period>(b =>
            {
                b.ToTable(PPMRmConsts.DbTablePrefix + "Periods",
                    PPMRmConsts.DbSchema);
                b.ConfigureByConvention(); //auto configure for the base class props
                b.HasIndex(x => new { x.Year, x.Month }).IsUnique();
            });

            builder.Entity<PeriodReport>(b =>
            {
                b.ToTable(PPMRmConsts.DbTablePrefix + "PeriodReports", PPMRmConsts.DbSchema);
                b.ConfigureByConvention();
                b.HasIndex(x => new { x.CountryId, x.PeriodId }).IsUnique();
                // ADD THE MAPPING FOR THE RELATION
                b.HasMany(x => x.ProductShipments).WithOne().HasForeignKey(x => x.PeriodReportId);
                b.HasMany(x => x.ProductStocks).WithOne().HasForeignKey(x => x.PeriodReportId);
                b.OwnsOne(x => x.CommoditySecurityUpdates);
                b.HasOne<Country>().WithMany().HasForeignKey(x => x.CountryId).IsRequired();
                b.HasOne<Period>().WithMany().HasForeignKey(x => x.PeriodId).IsRequired();
            });

            builder.Entity<ProductStock>(b =>
            {
                b.ToTable(PPMRmConsts.DbTablePrefix + "ProductStocks", PPMRmConsts.DbSchema);
                b.ConfigureByConvention();
                b.HasKey(x => new { x.PeriodReportId, x.ProgramId, x.ProductId });
                b.HasOne<Program>().WithMany().HasForeignKey(x => x.ProgramId).IsRequired();
                b.HasOne<Product>().WithMany().HasForeignKey(x => x.ProductId).IsRequired();
                b.Property(e => e.SOHLevels)
                    .HasConversion(sohLevelConverter);
            });

            builder.Entity<ProductShipment>(b =>
            {
                b.ToTable(PPMRmConsts.DbTablePrefix + "ProductShipments", PPMRmConsts.DbSchema);
                b.ConfigureByConvention();
                ;// ADD THE MAPPING FOR THE RELATION
                b.HasOne<Program>().WithMany().HasForeignKey(x => x.ProgramId).IsRequired();
                b.HasOne<Product>().WithMany().HasForeignKey(x => x.ProductId).IsRequired();
            });

            
        }
    }
}
