using Examination_System.Enums;
using Examination_System.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Examination_System.ModelsValidations;


[MetadataType(typeof(QuestionValidation))]
public partial class Question
{    }


public class QuestionValidation
{
    [Key]
    public int QuesId { get; set; }

    [Required]
    [StringLength(250, MinimumLength = 5)]
    [DisplayName("Title")]
    public string QuesTittle { get; set; }

    [Required]
    [EnumDataType(typeof(QuestionAnswer))]
    [Display(Name = "Answer")]

    public string QuesAnswer { get; set; }

    [Required]
    [EnumDataType(typeof(QuestionType))]
    [Display(Name ="Type")]
    public string QuesType { get; set; }


    [EnumDataType(typeof(QuestionWeight))]
    [Display(Name = "Weight")]
    public int QuesWeight { get; set; }
}
