using KFU.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KFU.Core.Models.Exams
{
    public class Answer : BaseObject
    {
        [Key]
        public int Id { get; set; }
        
        [Required(ErrorMessage = "الرجاء اختيار سؤال ")]
        [Range(1, int.MaxValue, ErrorMessage = "الرجاء اختيار سؤال")]
        public int QuestionId { get; set; }

        [Required(ErrorMessage ="الرجاء ادخال نص الاجابة")]
        public string Text { get; set; } = Constants.NullString;

        public override bool MapData(DataRow row)
        {
            Id = GetInt(row, "ID");
            QuestionId = GetInt(row, "QUESTIONID");
            Text = GetString(row, "TEXT");
            return base.MapData(row);
        }
    }
}
