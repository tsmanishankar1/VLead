using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Vis.VleadProcessV3.Models;


namespace Vis.VleadProcessV3.Models
{
    public partial class TallyDbContext:DbContext
    {
        private readonly IConfiguration _configuration;
        public TallyDbContext()
        {

        }
        public TallyDbContext(DbContextOptions<TallyDbContext> options, IConfiguration configuration)
         : base(options)
        {
            _configuration = configuration;
        }
        public virtual DbSet<IntegrationMaster> IntegrationMasters { get; set; }

        public virtual DbSet<IntegrationTran> IntegrationTrans { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
       => optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DBConnection"));
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("Latin1_General_CI_AI");

            modelBuilder.Entity<IntegrationMaster>(entity =>
            {
                entity.ToTable("IntegrationMaster");

                entity.Property(e => e.Amount).HasColumnType("numeric(18, 2)");
                entity.Property(e => e.BankName)
                    .HasMaxLength(100)
                    .IsUnicode(false);
                entity.Property(e => e.ClientName)
                    .HasMaxLength(100)
                    .IsUnicode(false);
                entity.Property(e => e.CreatedUtc)
                    .HasColumnType("datetime")
                    .HasColumnName("CreatedUTC");
                entity.Property(e => e.CurrencyName)
                    .HasMaxLength(15)
                    .IsUnicode(false);
                entity.Property(e => e.CurrencySymbol)
                    .HasMaxLength(5)
                    .IsUnicode(false);
                entity.Property(e => e.DestinationBank)
                    .HasMaxLength(100)
                    .IsUnicode(false);
                entity.Property(e => e.DiscountAmount).HasColumnType("numeric(8, 2)");
                entity.Property(e => e.DocumentDate).HasColumnType("datetime");
                entity.Property(e => e.DocumentNumber)
                    .HasMaxLength(35)
                    .IsUnicode(false);
                entity.Property(e => e.ExchangeRate).HasColumnType("numeric(14, 4)");
                entity.Property(e => e.FailedReason)
                    .HasMaxLength(100)
                    .IsUnicode(false);
                entity.Property(e => e.Mode)
                    .HasMaxLength(100)
                    .IsUnicode(false);
                entity.Property(e => e.ProductValue).HasColumnType("numeric(14, 4)");
                entity.Property(e => e.Roundoff).HasColumnType("numeric(8, 4)");
                entity.Property(e => e.TotalValue).HasColumnType("numeric(18, 2)");
                entity.Property(e => e.TransactionDate).HasColumnType("datetime");
                entity.Property(e => e.TransactionNumber)
                    .HasMaxLength(35)
                    .IsUnicode(false);
                entity.Property(e => e.UpdatedUtc)
                    .HasColumnType("datetime")
                    .HasColumnName("UpdatedUTC");
                entity.Property(e => e.WaiverAmount).HasColumnType("numeric(18, 4)");
            });

            modelBuilder.Entity<IntegrationTran>(entity =>
            {
                entity.ToTable("IntegrationTran");

                entity.Property(e => e.InvoiceDate).HasColumnType("datetime");
                entity.Property(e => e.InvoiceNumber)
                    .HasMaxLength(35)
                    .IsUnicode(false);
                entity.Property(e => e.Qty).HasColumnType("numeric(7, 2)");
                entity.Property(e => e.Rate).HasColumnType("numeric(10, 4)");
                entity.Property(e => e.Scope)
                    .HasMaxLength(100)
                    .IsUnicode(false);
                entity.Property(e => e.Value).HasColumnType("numeric(14, 4)");

                entity.HasOne(d => d.IntegrationMaster).WithMany(p => p.IntegrationTrans)
                    .HasForeignKey(d => d.IntegrationMasterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_IntegrationMasterId");
            });
        }
        }
}
