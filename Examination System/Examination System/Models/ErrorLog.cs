﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Examination_System.Models;

public partial class ErrorLog
{
    public int Id { get; set; }

    public int? ErrorNumber { get; set; }

    public string ErrorMessage { get; set; }

    public string ErrorProcedure { get; set; }

    public DateTime? ErrorTime { get; set; }
}