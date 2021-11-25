using Microsoft.EntityFrameworkCore;
using RestfulQr.Domain;

namespace RestfulQr.Migrations
{
    public class RestfulQrDbContext : DbContext
    {
        public DbSet<ApiKey> ApiKeys { get; set; }

        public DbSet<RestfulQrCode> RestfulQrCodes { get; set; }

        public RestfulQrDbContext(DbContextOptions<RestfulQrDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ApiKey>(e =>
            {
                e.ToTable("api_keys", schema: "identity");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).HasColumnName("id");
                e.Property(x => x.LocationId).HasColumnName("location_id").ValueGeneratedOnAdd();
                e.Property(x => x.Created).HasColumnName("created").IsRequired();
                e.Property(x => x.LastUsed).HasColumnName("last_used").IsRequired();
                e.HasIndex(x => x.LocationId).IsUnique();
            });

            builder.Entity<RestfulQrCode>(e =>
            {
                e.ToTable("qr_codes", schema: "qr_codes");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).HasColumnName("id");
                e.Property(x => x.Filename).HasColumnName("filename").IsRequired();
                e.Property(x => x.RenderType).HasColumnName("render_type").IsRequired();
                e.Property(x => x.Type).HasColumnName("type").IsRequired();
                e.Property(x => x.Created).HasColumnName("created").IsRequired();
                e.Property(x => x.CreatedBy).HasColumnName("created_by").IsRequired();
                e.Property(x => x.Model).HasColumnName("model").IsRequired();
                e.Property(x => x.PublicUrl).HasColumnName("public_url").IsRequired();

                e.HasOne<ApiKey>().WithMany().HasForeignKey(x => x.CreatedBy);
            });
        }
    }
}