﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Examination_System.Models;

public partial class Topic
{
    public int TopId { get; set; }

    public string TopName { get; set; }

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
}