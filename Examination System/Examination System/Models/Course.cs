﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Examination_System.Models;

public partial class Course
{
    public int CrsId { get; set; }

    public string CrsName { get; set; }

    public int? CrsDuration { get; set; }

    public virtual ICollection<InstructorTeachCourseForTrackInBranch> InstructorTeachCourseForTrackInBranches { get; set; } = new List<InstructorTeachCourseForTrackInBranch>();

    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();

    public virtual ICollection<Topic> Topics { get; set; } = new List<Topic>();

    public virtual ICollection<Track> Tracks { get; set; } = new List<Track>();
}