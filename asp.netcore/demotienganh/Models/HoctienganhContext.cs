using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace demotienganh.Models;

public partial class HoctienganhContext : DbContext
{
    public HoctienganhContext()
    {
    }

    public HoctienganhContext(DbContextOptions<HoctienganhContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Grammar> Grammars { get; set; }

    public virtual DbSet<Testgrammar> Testgrammars { get; set; }

    public virtual DbSet<Vocabulary> Vocabularies { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=QUOCDAT\\SQLEXPRESS;Initial Catalog=hoctienganh;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Name).HasName("PK__account__72E12F1AA1A8FF8C");

            entity.ToTable("account");

            entity.Property(e => e.Name)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Email)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Pass)
                .HasMaxLength(11)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("pass");
            entity.Property(e => e.Role).HasColumnName("role");
        });

        modelBuilder.Entity<Grammar>(entity =>
        {
            entity.HasKey(e => e.Name).HasName("PK__grammar__72E12F1A7EE10918");

            entity.ToTable("grammar");

            entity.Property(e => e.Name)
                .HasMaxLength(40)
                .HasColumnName("name");
            entity.Property(e => e.Confirm)
                .HasMaxLength(50)
                .HasColumnName("confirm");
            entity.Property(e => e.Doubt)
                .HasMaxLength(50)
                .HasColumnName("doubt");
            entity.Property(e => e.Negative)
                .HasMaxLength(50)
                .HasColumnName("negative");
            entity.Property(e => e.Title)
                .HasMaxLength(300)
                .HasColumnName("title");
        });

        modelBuilder.Entity<Testgrammar>(entity =>
        {
            entity.HasKey(e => e.Name).HasName("PK__testgram__72E12F1A94627B75");

            entity.ToTable("testgrammar");

            entity.Property(e => e.Name)
                .HasMaxLength(40)
                .HasColumnName("name");
            entity.Property(e => e.Correct)
                .HasMaxLength(25)
                .HasColumnName("correct");
            entity.Property(e => e.Fail1)
                .HasMaxLength(25)
                .HasColumnName("fail1");
            entity.Property(e => e.Fail2)
                .HasMaxLength(25)
                .HasColumnName("fail2");
            entity.Property(e => e.Fail3)
                .HasMaxLength(25)
                .HasColumnName("fail3");
            entity.Property(e => e.Question)
                .HasMaxLength(100)
                .HasColumnName("question");

            entity.HasOne(d => d.NameNavigation).WithOne(p => p.Testgrammar)
                .HasForeignKey<Testgrammar>(d => d.Name)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__testgramma__name__2EDAF651");
        });

        modelBuilder.Entity<Vocabulary>(entity =>
        {
            entity.HasKey(e => e.NameEn).HasName("PK__vocabula__F480301B1FF78529");

            entity.ToTable("vocabulary");

            entity.Property(e => e.NameEn)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("nameEN");
            entity.Property(e => e.Firstchar)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("firstchar");
            entity.Property(e => e.NameVn)
                .HasMaxLength(200)
                .HasColumnName("nameVN");
            entity.Property(e => e.Title)
                .HasMaxLength(300)
                .HasColumnName("title");
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .HasColumnName("type");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
