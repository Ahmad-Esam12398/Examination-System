﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Examination_System.Models;

public partial class Exam
{
    public int ExId { get; set; }

    public int ExDuration { get; set; }

    public int ExGrade { get; set; }

    public int ExPassGrade { get; set; }

    public virtual ICollection<StudentExamGrade> StudentExamGrades { get; set; } = new List<StudentExamGrade>();

    public virtual ICollection<StudentTakeExam> StudentTakeExams { get; set; } = new List<StudentTakeExam>();

    public virtual ICollection<TrackCourseExam> TrackCourseExams { get; set; } = new List<TrackCourseExam>();

    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
}