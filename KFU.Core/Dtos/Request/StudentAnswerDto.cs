using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KFU.Core.Dtos.Request
{
    public class StudentAnswerDto
    {
        [Required(ErrorMessage = "الرجاء ادخال رقم الامتحان")]
        [RegularExpression("([0-9][0-9]*)", ErrorMessage = "الرجاء ادخال ارقام فقط")]
        public int ExamId { get; set; }
        
        [Required(ErrorMessage ="الرجاء ادخال رقم الطالب")]
        [RegularExpression("([0-9][0-9]*)", ErrorMessage = "الرجاء ادخال ارقام فقط")]
        [StringLength(9, MinimumLength = 9, ErrorMessage = "عفوا يجب ان يتكون رقم الطالب من 9 ارقام")]
        public string StudentNo { get; set; }

        [Required(ErrorMessage = "الرجاء ادخال  الاجابات")]
        public List<QuestionAnswerDto> Answers { get; set; } = new List<QuestionAnswerDto>();

    }
    public class QuestionAnswerDto
    {
        [Required(ErrorMessage = "الرجاء ادخال  رقم السؤال")]
        public int QuestionId { get; set; }
        [Required(ErrorMessage = "الرجاء ادخال  رقم الاجابة")]
        public int AnswerId { get; set; }
    }
}
