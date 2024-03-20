using KFU.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KFU.Core.Dtos.Request
{
    public class CreateExamTableDto 
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "الرجاء اختيار المقرر")]
        public string CourseNo { get; set; } = Constants.NullString;

        public string Title { get; set; } = Constants.NullString;

        [Required(ErrorMessage = "الرجاء تحديد تاريخ الامتحان")]
        [DataType(DataType.Date)]
        public DateTime ExamDate { get; set; } = Constants.NullDateTime;

        [Required(ErrorMessage = "الرجاء تحديد وقت بداية الامتحان")]
        [DataType(DataType.Time)]
        public TimeSpan StartTime { get; set; }

        [Required(ErrorMessage = "الرجاء تحديد وقت نهاية الامتحان")]
        [DataType(DataType.Time)]
        [Compare(nameof(StartTime), ErrorMessage = "الرجاء ادخال وقت نهاية الامتحان بشكل صحيح")]
        public TimeSpan EndTime { get; set; }

        // duration on minutes
        public int Duration { get; set; } = 60;

    }
}
