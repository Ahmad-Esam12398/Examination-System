﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Examination_System.Models;

public partial class StudentExamGrade
{
    public string StdId { get; set; }

    public int ExamId { get; set; }

    public double? Grade { get; set; }

    public virtual Exam Exam { get; set; }

    public virtual Student Std { get; set; }
}