using KFU.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KFU.Core.Models.Exams
{
    public class StudentAnswers
    {
        [Required(ErrorMessage ="الرجاء عدم ترك السؤال فارغا")]
        public int QuestionId { get; set; }

        [Required(ErrorMessage = "الرجاء عدم الاجابة السؤال فارغا")]
        public int AnswerId { get; set; } = Constants.NullInt;
    }
}
