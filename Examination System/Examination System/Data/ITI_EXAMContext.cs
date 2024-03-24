﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using Examination_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Examination_System.Data;

public partial class ITI_EXAMContext : DbContext
{
    public ITI_EXAMContext()
    {
    }

    public ITI_EXAMContext(DbContextOptions<ITI_EXAMContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Branch> Branches { get; set; }

    public virtual DbSet<Choice> Choices { get; set; }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<ErrorLog> ErrorLogs { get; set; }

    public virtual DbSet<Exam> Exams { get; set; }

    public virtual DbSet<ExamQuestion> ExamQuestions { get; set; }

    public virtual DbSet<Instructor> Instructors { get; set; }

    public virtual DbSet<InstructorTeachCourseForTrackInBranch> InstructorTeachCourseForTrackInBranches { get; set; }

    public virtual DbSet<Question> Questions { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<StudentExamGrade> StudentExamGrades { get; set; }

    public virtual DbSet<StudentTakeExam> StudentTakeExams { get; set; }

    public virtual DbSet<Topic> Topics { get; set; }

    public virtual DbSet<Track> Tracks { get; set; }

    public virtual DbSet<TrackCourseExam> TrackCourseExams { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=ITI_EXAM;Integrated Security=True;Encrypt=True;trust server certificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Branch>(entity =>
        {
            entity.ToTable("Branch");

            entity.Property(e => e.BranchName)
                .IsRequired()
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.MgrId)
                .IsRequired()
                .HasMaxLength(14)
                .IsUnicode(false);

            entity.HasOne(d => d.Mgr).WithMany(p => p.Branches)
                .HasForeignKey(d => d.MgrId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Branch_Instructor");

            entity.HasMany(d => d.Tracks).WithMany(p => p.Brs)
                .UsingEntity<Dictionary<string, object>>(
                    "BranchTrack",
                    r => r.HasOne<Track>().WithMany()
                        .HasForeignKey("TrackId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Branch_Track_Track"),
                    l => l.HasOne<Branch>().WithMany()
                        .HasForeignKey("BrId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Branch_Track_Branch"),
                    j =>
                    {
                        j.HasKey("BrId", "TrackId");
                        j.ToTable("Branch_Track");
                        j.IndexerProperty<int>("BrId").HasColumnName("br_id");
                        j.IndexerProperty<int>("TrackId").HasColumnName("track_id");
                    });
        });

        modelBuilder.Entity<Choice>(entity =>
        {
            entity.HasKey(e => e.QuesId);

            entity.ToTable("Choice");

            entity.Property(e => e.QuesId)
                .ValueGeneratedNever()
                .HasColumnName("ques_id");
            entity.Property(e => e.A)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.B)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.C)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.D)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Ques).WithOne(p => p.Choice)
                .HasForeignKey<Choice>(d => d.QuesId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Choice_Question");
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.CrsId);

            entity.ToTable("Course");

            entity.Property(e => e.CrsId).HasColumnName("crs_id");
            entity.Property(e => e.CrsDuration).HasColumnName("crs_duration");
            entity.Property(e => e.CrsName)
                .IsRequired()
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("crs_name");

            entity.HasMany(d => d.Topics).WithMany(p => p.Courses)
                .UsingEntity<Dictionary<string, object>>(
                    "CourseTopic",
                    r => r.HasOne<Topic>().WithMany()
                        .HasForeignKey("TopicId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Course_Topic_Topic"),
                    l => l.HasOne<Course>().WithMany()
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Course_Topic_Course"),
                    j =>
                    {
                        j.HasKey("CourseId", "TopicId");
                        j.ToTable("Course_Topic");
                        j.IndexerProperty<int>("CourseId").HasColumnName("courseId");
                        j.IndexerProperty<int>("TopicId").HasColumnName("topicId");
                    });
        });

        modelBuilder.Entity<ErrorLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__error_lo__3213E83F0F687C98");

            entity.ToTable("error_log");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ErrorMessage)
                .HasMaxLength(4000)
                .HasColumnName("errorMessage");
            entity.Property(e => e.ErrorNumber).HasColumnName("errorNumber");
            entity.Property(e => e.ErrorProcedure)
                .HasMaxLength(200)
                .HasColumnName("errorProcedure");
            entity.Property(e => e.ErrorTime)
                .HasColumnType("datetime")
                .HasColumnName("errorTime");
        });

        modelBuilder.Entity<Exam>(entity =>
        {
            entity.HasKey(e => e.ExId);

            entity.ToTable("Exam");

            entity.Property(e => e.ExId).HasColumnName("Ex_id");
            entity.Property(e => e.ExDuration).HasColumnName("Ex_duration");
            entity.Property(e => e.ExGrade).HasColumnName("Ex_grade");
            entity.Property(e => e.ExPassGrade).HasColumnName("Ex_passGrade");
        });

        modelBuilder.Entity<ExamQuestion>(entity =>
        {
            entity.HasKey(e => new { e.ExamId, e.QuestionId });

            entity.ToTable("Exam_Question");

            entity.Property(e => e.ExamId).HasColumnName("Exam_id");
            entity.Property(e => e.QuestionId).HasColumnName("Question_id");

            entity.HasOne(d => d.Question).WithMany(p => p.ExamQuestions)
                .HasForeignKey(d => d.QuestionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Exam_Question_Question");
        });

        modelBuilder.Entity<Instructor>(entity =>
        {
            entity.HasKey(e => e.InsId);

            entity.ToTable("Instructor");

            entity.Property(e => e.InsId)
                .HasMaxLength(14)
                .IsUnicode(false)
                .HasColumnName("Ins_id");
            entity.Property(e => e.InsMobile)
                .HasMaxLength(11)
                .IsFixedLength()
                .HasColumnName("Ins_mobile");
            entity.Property(e => e.InsName)
                .IsRequired()
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("Ins_name");
            entity.Property(e => e.InsPassword)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("Ins_password");

            entity.HasMany(d => d.BranchesNavigation).WithMany(p => p.Insts)
                .UsingEntity<Dictionary<string, object>>(
                    "InstAssignBranch",
                    r => r.HasOne<Branch>().WithMany()
                        .HasForeignKey("BranchId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Inst_assign_Branch_Branch"),
                    l => l.HasOne<Instructor>().WithMany()
                        .HasForeignKey("InstId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Inst_assign_Branch_Instructor"),
                    j =>
                    {
                        j.HasKey("InstId", "BranchId");
                        j.ToTable("Inst_assign_Branch");
                        j.IndexerProperty<string>("InstId")
                            .HasMaxLength(14)
                            .IsUnicode(false)
                            .HasColumnName("inst_id");
                        j.IndexerProperty<int>("BranchId").HasColumnName("branch_id");
                    });
        });

        modelBuilder.Entity<InstructorTeachCourseForTrackInBranch>(entity =>
        {
            entity.HasKey(e => new { e.InsId, e.CrsId, e.TrackId, e.BranchId }).HasName("PK_Instructor_Teach_Course");

            entity.ToTable("Instructor_Teach_Course_For_Track_In_Branch");

            entity.Property(e => e.InsId)
                .HasMaxLength(14)
                .IsUnicode(false)
                .HasColumnName("ins_id");
            entity.Property(e => e.CrsId).HasColumnName("crs_id");
            entity.Property(e => e.TrackId).HasColumnName("track_id");
            entity.Property(e => e.BranchId).HasColumnName("branch_id");

            entity.HasOne(d => d.Branch).WithMany(p => p.InstructorTeachCourseForTrackInBranches)
                .HasForeignKey(d => d.BranchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Instructor_Teach_Course_Branch");

            entity.HasOne(d => d.Crs).WithMany(p => p.InstructorTeachCourseForTrackInBranches)
                .HasForeignKey(d => d.CrsId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Instructor_Teach_Course_Course");

            entity.HasOne(d => d.Ins).WithMany(p => p.InstructorTeachCourseForTrackInBranches)
                .HasForeignKey(d => d.InsId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Instructor_Teach_Course_Instructor");

            entity.HasOne(d => d.Track).WithMany(p => p.InstructorTeachCourseForTrackInBranches)
                .HasForeignKey(d => d.TrackId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Instructor_Teach_Course_Track");
        });

        modelBuilder.Entity<Question>(entity =>
        {
            entity.HasKey(e => e.QuesId);

            entity.ToTable("Question");

            entity.Property(e => e.QuesId).HasColumnName("ques_id");
            entity.Property(e => e.CrsId).HasColumnName("crs_id");
            entity.Property(e => e.InsId)
                .IsRequired()
                .HasMaxLength(14)
                .IsUnicode(false)
                .HasColumnName("ins_id");
            entity.Property(e => e.QuesAnswer)
                .IsRequired()
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("ques_answer");
            entity.Property(e => e.QuesTittle)
                .IsRequired()
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("ques_tittle");
            entity.Property(e => e.QuesType)
                .IsRequired()
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("ques_type");
            entity.Property(e => e.QuesWeight).HasColumnName("ques_weight");

            entity.HasOne(d => d.Crs).WithMany(p => p.Questions)
                .HasForeignKey(d => d.CrsId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("question_course_fk");

            entity.HasOne(d => d.Ins).WithMany(p => p.Questions)
                .HasForeignKey(d => d.InsId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("question_instructor_fk");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.StdId);

            entity.ToTable("Student");

            entity.Property(e => e.StdId)
                .HasMaxLength(14)
                .IsUnicode(false)
                .HasColumnName("std_id");
            entity.Property(e => e.BranchId).HasColumnName("branch_id");
            entity.Property(e => e.StdBirthDate).HasColumnName("std_birthDate");
            entity.Property(e => e.StdMobile)
                .IsRequired()
                .HasMaxLength(11)
                .IsUnicode(false)
                .HasColumnName("std_mobile");
            entity.Property(e => e.StdName)
                .IsRequired()
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("std_name");
            entity.Property(e => e.StdPassword)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("std_password");
            entity.Property(e => e.TrackId).HasColumnName("track_id");
        });

        modelBuilder.Entity<StudentExamGrade>(entity =>
        {
            entity.HasKey(e => new { e.StdId, e.ExamId });

            entity.ToTable("Student_Exam_Grade");

            entity.Property(e => e.StdId)
                .HasMaxLength(14)
                .IsUnicode(false)
                .HasColumnName("std_id");
            entity.Property(e => e.ExamId).HasColumnName("Exam_id");
            entity.Property(e => e.Grade).HasColumnName("grade");

            entity.HasOne(d => d.Exam).WithMany(p => p.StudentExamGrades)
                .HasForeignKey(d => d.ExamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Student_Exam_Grade_Exam");

            entity.HasOne(d => d.Std).WithMany(p => p.StudentExamGrades)
                .HasForeignKey(d => d.StdId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Student_Exam_Grade_Student");
        });

        modelBuilder.Entity<StudentTakeExam>(entity =>
        {
            entity.HasKey(e => new { e.StdId, e.ExamId, e.QuestionId });

            entity.ToTable("Student_Take_Exam");

            entity.Property(e => e.StdId)
                .HasMaxLength(14)
                .IsUnicode(false)
                .HasColumnName("std_id");
            entity.Property(e => e.ExamId).HasColumnName("Exam_id");
            entity.Property(e => e.QuestionId).HasColumnName("Question_id");
            entity.Property(e => e.Answer)
                .IsRequired()
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("answer");

            entity.HasOne(d => d.Exam).WithMany(p => p.StudentTakeExams)
                .HasForeignKey(d => d.ExamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Student_Take_Exam_Exam1");

            entity.HasOne(d => d.Question).WithMany(p => p.StudentTakeExams)
                .HasForeignKey(d => d.QuestionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Student_Take_Exam_Question");

            entity.HasOne(d => d.Std).WithMany(p => p.StudentTakeExams)
                .HasForeignKey(d => d.StdId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Student_Take_Exam_Student");
        });

        modelBuilder.Entity<Topic>(entity =>
        {
            entity.HasKey(e => e.TopId);

            entity.ToTable("Topic");

            entity.Property(e => e.TopId).HasColumnName("top_id");
            entity.Property(e => e.TopName)
                .IsRequired()
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("top_name");
        });

        modelBuilder.Entity<Track>(entity =>
        {
            entity.ToTable("Track");

            entity.Property(e => e.TrackId).HasColumnName("track_id");
            entity.Property(e => e.SupId)
                .IsRequired()
                .HasMaxLength(14)
                .IsUnicode(false)
                .HasColumnName("sup_id");
            entity.Property(e => e.TrackName)
                .IsRequired()
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("track_name");

            entity.HasOne(d => d.Sup).WithMany(p => p.Tracks)
                .HasForeignKey(d => d.SupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Track_Instructor");

            entity.HasMany(d => d.Crs).WithMany(p => p.Tracks)
                .UsingEntity<Dictionary<string, object>>(
                    "TrackCourse",
                    r => r.HasOne<Course>().WithMany()
                        .HasForeignKey("CrsId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Track_Course_Course"),
                    l => l.HasOne<Track>().WithMany()
                        .HasForeignKey("TrackId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Track_Course_Track"),
                    j =>
                    {
                        j.HasKey("TrackId", "CrsId");
                        j.ToTable("Track_Course");
                        j.IndexerProperty<int>("TrackId").HasColumnName("track_id");
                        j.IndexerProperty<int>("CrsId").HasColumnName("crs_id");
                    });
        });

        modelBuilder.Entity<TrackCourseExam>(entity =>
        {
            entity.HasKey(e => new { e.ExamId, e.ExameDate });

            entity.ToTable("Track_Course_Exam");

            entity.Property(e => e.ExamId).HasColumnName("Exam_id");
            entity.Property(e => e.ExameDate).HasColumnName("Exame_date");
            entity.Property(e => e.CrsId).HasColumnName("crs_id");
            entity.Property(e => e.TrId).HasColumnName("tr_id");

            entity.HasOne(d => d.Crs).WithMany(p => p.TrackCourseExams)
                .HasForeignKey(d => d.CrsId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Track_Course_Exam_Course");

            entity.HasOne(d => d.Exam).WithMany(p => p.TrackCourseExams)
                .HasForeignKey(d => d.ExamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Track_Course_Exam_Exam");

            entity.HasOne(d => d.Tr).WithMany(p => p.TrackCourseExams)
                .HasForeignKey(d => d.TrId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Track_Course_Exam_Track");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}