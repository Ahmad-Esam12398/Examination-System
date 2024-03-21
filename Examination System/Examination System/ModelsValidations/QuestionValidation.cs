using Examination_System.Enums;
using Examination_System.Models;
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
    [StringLength(200 ,MinimumLength =5)]
    public string QuesTittle { get; set; }

    [Required]
    [EnumDataType(typeof(QuestionAnswer))]
    public string QuesAnswer { get; set; }

    [Required]
    [EnumDataType(typeof(QuestionType))]
    public string QuesType { get; set; }


    [EnumDataType(typeof(QuestionWeight))]
    public int QuesWeight { get; set; }
}
