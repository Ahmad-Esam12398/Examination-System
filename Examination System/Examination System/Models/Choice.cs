﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Examination_System.Models;

public partial class Choice
{
    public int QuesId { get; set; }

    public string A { get; set; }

    public string B { get; set; }

    public string C { get; set; }

    public string D { get; set; }

    public virtual Question Ques { get; set; }
}