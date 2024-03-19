﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Examination_System.Procedures;

public partial class Track
{
    public int TrackId { get; set; }

    public string TrackName { get; set; }

    public string SupId { get; set; }

    public virtual ICollection<InstructorTeachCourseForTrackInBranch> InstructorTeachCourseForTrackInBranches { get; set; } = new List<InstructorTeachCourseForTrackInBranch>();

    public virtual Instructor Sup { get; set; }

    public virtual ICollection<TrackCourseExam> TrackCourseExams { get; set; } = new List<TrackCourseExam>();

    public virtual ICollection<Branch> Brs { get; set; } = new List<Branch>();

    public virtual ICollection<Course> Crs { get; set; } = new List<Course>();
}