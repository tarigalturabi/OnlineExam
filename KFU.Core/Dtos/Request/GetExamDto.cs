using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KFU.Core.Dtos.Request
{
    public class GetExamDto
    {
        [Required(ErrorMessage = "الرجاء ادخال رقم الامتحان")]
        public int ExamId { get; set; }
        [Required(ErrorMessage = "الرجاء ادخال رقم الطالب")]
        public string StudentNo { get; set; } = string.Empty;
    }
}
