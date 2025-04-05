using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShinaParser.Models;

public class ParserContext : DbContext
{
    public virtual DbSet<Brand> Brands { get; set; }
    public virtual DbSet<Country> Countries { get; set; }
    public virtual DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Data Source=SILVERSTONE\\SQLEXPRESS;Initial Catalog=ShinaParserP33;Integrated Security=True;Persist Security Info=False;Pooling=False;Multiple Active Result Sets=False;Connect Timeout=60;Encrypt=True;Trust Server Certificate=True;Command Timeout=0");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Brand

        modelBuilder.Entity<Brand>()
            // Fluent API - set table name
            .ToTable("product_brands")
            // primary key
            .HasKey(b => b.Id);

        // set column name for Id   
        modelBuilder.Entity<Brand>()
            .Property(b => b.Id)
            //.HasColumnType("BIGINT")
            .HasColumnName("brand_id")
            .UseIdentityColumn();

        // set column name for Title
        modelBuilder.Entity<Brand>()
            .Property(b => b.Title)
            .HasColumnName("brand_name")
            .IsRequired()
            .HasMaxLength(100);

        // Country
        modelBuilder.Entity<Country>()
            // Fluent API - set table name
            .ToTable("countries")
            // primary key
            .HasKey(c => c.Id);
        // set column name for Id
        modelBuilder.Entity<Country>()
            .Property(c => c.Id)
            //.HasColumnName("country_id")
            .UseIdentityColumn();

        modelBuilder.Entity<Country>()
            .Property(c => c.Title)
            //.HasColumnName("country_name")
            .IsRequired()
            .HasMaxLength(100);

        modelBuilder.Entity<Country>()
            .HasIndex(c => c.Title)
            .IsUnique()
            .HasDatabaseName("IX_countries_Title");



        modelBuilder.Entity<Product>()
            // - foreign key
            .HasOne<Country>(p => p.Country)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CountryId)
            .IsRequired(false);

        modelBuilder.Entity<Product>()
            // - foreign key
            .Property(p => p.CountryId)
            .HasColumnName("country_model_id");




    }
}
