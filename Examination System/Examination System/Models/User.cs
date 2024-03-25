﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Examination_System.Models;

public partial class User
{
    public string Id { get; set; }

    public string Name { get; set; }

    public string Password { get; set; }

    public string Mobile { get; set; }

    public int RoleId { get; set; }

    public virtual ICollection<Instructor> Instructors { get; set; } = new List<Instructor>();

    public virtual Role Role { get; set; }

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
}