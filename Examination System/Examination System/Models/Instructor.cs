﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Examination_System.Models;

public partial class Instructor
{
    public string InsId { get; set; }

    public string InsName { get; set; }

    public string InsPassword { get; set; }

    public string InsMobile { get; set; }

    public virtual ICollection<Branch> Branches { get; set; } = new List<Branch>();

    public virtual ICollection<InstructorTeachCourseForTrackInBranch> InstructorTeachCourseForTrackInBranches { get; set; } = new List<InstructorTeachCourseForTrackInBranch>();

    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();

    public virtual ICollection<Track> Tracks { get; set; } = new List<Track>();

    public virtual ICollection<Branch> BranchesNavigation { get; set; } = new List<Branch>();
}