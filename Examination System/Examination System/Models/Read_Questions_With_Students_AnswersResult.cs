﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Examination_System.Models
{
    public partial class Read_Questions_With_Students_AnswersResult
    {
        public int Question_id { get; set; }
        public string ques_tittle { get; set; }
        public string Choices { get; set; }
        [Column("Student Answer")]
        public string StudentAnswer { get; set; }
        [Column("Model Answer")]
        public string ModelAnswer { get; set; }
    }
}