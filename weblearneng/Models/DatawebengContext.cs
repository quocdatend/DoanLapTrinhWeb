using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace weblearneng.Models;

public partial class DatawebengContext : DbContext
{
    public DatawebengContext()
    {
    }

    public DatawebengContext(DbContextOptions<DatawebengContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Exam> Exams { get; set; }

    public virtual DbSet<Grammar> Grammars { get; set; }

    public virtual DbSet<Question> Questions { get; set; }

    public virtual DbSet<QuestionsContent> QuestionsContents { get; set; }

    public virtual DbSet<Score> Scores { get; set; }

    public virtual DbSet<Testgrammar> Testgrammars { get; set; }

    public virtual DbSet<Testvocabulary> Testvocabularies { get; set; }

    public virtual DbSet<UserResponse> UserResponses { get; set; }

    public virtual DbSet<Vocabulary> Vocabularies { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=QUOCDAT\\SQLEXPRESS;Initial Catalog=DATAWEBENG;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ACCOUNT__3214EC274F919F6B");

            entity.ToTable("ACCOUNT");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Email)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            entity.Property(e => e.Name)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("NAME");
            entity.Property(e => e.Pass)
                .HasMaxLength(12)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("PASS");
            entity.Property(e => e.Role).HasColumnName("ROLE");
        });

        modelBuilder.Entity<Exam>(entity =>
        {
            entity.HasKey(e => e.Examid).HasName("PK__EXAMS__8D5AA6D0BFB42206");

            entity.ToTable("EXAMS");

            entity.Property(e => e.Examid)
                .ValueGeneratedNever()
                .HasColumnName("EXAMID");
            entity.Property(e => e.Examname)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("EXAMNAME");
        });

        modelBuilder.Entity<Grammar>(entity =>
        {
            entity.HasKey(e => e.Name).HasName("PK__GRAMMAR__D9C1FA01EAA705D7");

            entity.ToTable("GRAMMAR");

            entity.Property(e => e.Name)
                .HasMaxLength(40)
                .HasColumnName("NAME");
            entity.Property(e => e.Confirm)
                .HasMaxLength(50)
                .HasColumnName("CONFIRM");
            entity.Property(e => e.Doubt)
                .HasMaxLength(50)
                .HasColumnName("DOUBT");
            entity.Property(e => e.Negative)
                .HasMaxLength(50)
                .HasColumnName("NEGATIVE");
            entity.Property(e => e.Title)
                .HasMaxLength(300)
                .HasColumnName("TITLE");
        });

        modelBuilder.Entity<Question>(entity =>
        {
            entity.HasKey(e => e.QuestionId).HasName("PK__QUESTION__0DC06F8CC26352A0");

            entity.ToTable("QUESTIONS");

            entity.Property(e => e.QuestionId)
                .ValueGeneratedNever()
                .HasColumnName("QuestionID");
            entity.Property(e => e.AnswerA).HasColumnType("text");
            entity.Property(e => e.AnswerB).HasColumnType("text");
            entity.Property(e => e.AnswerC).HasColumnType("text");
            entity.Property(e => e.AnswerD).HasColumnType("text");
            entity.Property(e => e.CorrectAnswer)
                .HasMaxLength(1)
                .IsFixedLength();
            entity.Property(e => e.Qcid).HasColumnName("QCID");
            entity.Property(e => e.QuestionText).HasColumnType("text");

            entity.HasOne(d => d.Qc).WithMany(p => p.Questions)
                .HasForeignKey(d => d.Qcid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_QUESTIONS_QUESTIONS-CONTENT");
        });

        modelBuilder.Entity<QuestionsContent>(entity =>
        {
            entity.HasKey(e => e.Contentid);

            entity.ToTable("QUESTIONS-CONTENT");

            entity.Property(e => e.Contentid).HasColumnName("CONTENTID");
            entity.Property(e => e.Adudi)
                .HasMaxLength(200)
                .HasColumnName("ADUDI");
            entity.Property(e => e.Examid).HasColumnName("EXAMID");
            entity.Property(e => e.Picture)
                .HasMaxLength(200)
                .HasColumnName("PICTURE");
            entity.Property(e => e.QuestionsStyle)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("QUESTIONS-STYLE");
            entity.Property(e => e.TextContent)
                .HasMaxLength(200)
                .HasColumnName("TEXT-CONTENT");
            entity.Property(e => e.TextQuestionsbigIfhave)
                .HasMaxLength(200)
                .HasColumnName("Text-questionsbig-ifhave");

            entity.HasOne(d => d.Exam).WithMany(p => p.QuestionsContents)
                .HasForeignKey(d => d.Examid)
                .HasConstraintName("FK_QUESTIONS-CONTENT_EXAMS");
        });

        modelBuilder.Entity<Score>(entity =>
        {
            entity.HasKey(e => e.ScoreId).HasName("PK__Scores__7DD229F1216AFB9F");

            entity.Property(e => e.ScoreId)
                .ValueGeneratedNever()
                .HasColumnName("ScoreID");
            entity.Property(e => e.ExamId).HasColumnName("ExamID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Exam).WithMany(p => p.Scores)
                .HasForeignKey(d => d.ExamId)
                .HasConstraintName("FK__Scores__ExamID__5DCAEF64");

            entity.HasOne(d => d.User).WithMany(p => p.Scores)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Scores__UserID__5EBF139D");
        });

        modelBuilder.Entity<Testgrammar>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.Name }).HasName("PK__TESTGRAM__3F88F38728BD238A");

            entity.ToTable("TESTGRAMMAR");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name)
                .HasMaxLength(40)
                .HasColumnName("NAME");
            entity.Property(e => e.Correct)
                .HasMaxLength(25)
                .HasColumnName("CORRECT");
            entity.Property(e => e.Fail1)
                .HasMaxLength(25)
                .HasColumnName("FAIL1");
            entity.Property(e => e.Fail2)
                .HasMaxLength(25)
                .HasColumnName("FAIL2");
            entity.Property(e => e.Fail3)
                .HasMaxLength(25)
                .HasColumnName("FAIL3");
            entity.Property(e => e.Question)
                .HasMaxLength(100)
                .HasColumnName("QUESTION");

            entity.HasOne(d => d.IdNavigation).WithMany(p => p.Testgrammars)
                .HasForeignKey(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TESTGRAMMAR__ID__60A75C0F");

            entity.HasOne(d => d.NameNavigation).WithMany(p => p.Testgrammars)
                .HasForeignKey(d => d.Name)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TESTGRAMMA__NAME__5FB337D6");
        });

        modelBuilder.Entity<Testvocabulary>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.Nameen }).HasName("PK__TESTVOCA__9952ED4AEC33733F");

            entity.ToTable("TESTVOCABULARY");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Nameen)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("NAMEEN");
            entity.Property(e => e.Correct)
                .HasMaxLength(25)
                .HasColumnName("CORRECT");
            entity.Property(e => e.Fail1)
                .HasMaxLength(25)
                .HasColumnName("FAIL1");
            entity.Property(e => e.Fail2)
                .HasMaxLength(25)
                .HasColumnName("FAIL2");
            entity.Property(e => e.Fail3)
                .HasMaxLength(25)
                .HasColumnName("FAIL3");

            entity.HasOne(d => d.IdNavigation).WithMany(p => p.Testvocabularies)
                .HasForeignKey(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TESTVOCABULA__ID__628FA481");

            entity.HasOne(d => d.NameenNavigation).WithMany(p => p.Testvocabularies)
                .HasForeignKey(d => d.Nameen)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TESTVOCAB__NAMEE__619B8048");
        });

        modelBuilder.Entity<UserResponse>(entity =>
        {
            entity.HasKey(e => e.ResponseId).HasName("PK__UserResp__1AAA640CC9274199");

            entity.Property(e => e.ResponseId)
                .ValueGeneratedNever()
                .HasColumnName("ResponseID");
            entity.Property(e => e.ExamId).HasColumnName("ExamID");
            entity.Property(e => e.QuestionId).HasColumnName("QuestionID");
            entity.Property(e => e.UserAnswer)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Exam).WithMany(p => p.UserResponses)
                .HasForeignKey(d => d.ExamId)
                .HasConstraintName("FK__UserRespo__ExamI__6383C8BA");

            entity.HasOne(d => d.Question).WithMany(p => p.UserResponses)
                .HasForeignKey(d => d.QuestionId)
                .HasConstraintName("FK__UserRespo__Quest__4F7CD00D");

            entity.HasOne(d => d.User).WithMany(p => p.UserResponses)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__UserRespo__UserI__656C112C");
        });

        modelBuilder.Entity<Vocabulary>(entity =>
        {
            entity.HasKey(e => e.Nameen).HasName("PK__VOCABULA__B46016DB3D9E5339");

            entity.ToTable("VOCABULARY");

            entity.Property(e => e.Nameen)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("NAMEEN");
            entity.Property(e => e.Fchar)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("FCHAR");
            entity.Property(e => e.Namevn)
                .HasMaxLength(150)
                .HasColumnName("NAMEVN");
            entity.Property(e => e.Title)
                .HasMaxLength(300)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("TITLE");
            entity.Property(e => e.Type)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("TYPE");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
