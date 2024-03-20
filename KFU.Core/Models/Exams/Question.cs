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
    public class Question : BaseObject 
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "الرجاء اختيار المقرر")]
        public string CourseNo { get; set; } = Constants.NullString;

        [Required(ErrorMessage = "الرجاء ادخال نص السؤال")]
        public string Text { get; set; } = Constants.NullString;

        [Required(ErrorMessage = "الرجاء تحديد الاجابة الصحيحة")]
        [Range(1, int.MaxValue, ErrorMessage = "الرجاء تحديد الاجابة الصحيحة")]
        public int TrueAnswerId { get; set; }

        [Required(ErrorMessage = "الرجاء تحديد حالة السؤال")]
        [Range(0, 2, ErrorMessage = "الرجاء ادخال قيمة صحيحة لحالة السؤال")]
        public int Status { get; set; } = (int)QuestionStatus.Active;

        [Required(ErrorMessage = "الرجاء تحديد حالة السؤال")]
        [Range(0, 2, ErrorMessage = "الرجاء ادخال قيمة صحيحة لمستوى  السؤال")]
        public int QLevel { get; set; } = (int)QuestionLevel.Easy;

        [Required(ErrorMessage = "الرجاء  ادخال درجة السؤال")]
        public decimal Grade { get; set; } = 1;

        [Required(ErrorMessage = "الرجاء  ادخال  اجابات السؤال")]
        public List<Answer> Answers { get; set; } = new List<Answer>();

        public override bool MapData(DataRow row)
        {
            Id =            GetInt(row , "ID");
            CourseNo =      GetString(row , "CourseNo");
            Text =          GetString(row , "TEXT");
            TrueAnswerId =  GetInt(row , "TrueAnswerId");
            Status =        GetInt(row , "Status");
            QLevel =         GetInt(row , "Level");
            Grade =         GetDecimal(row, "Grade");
            Answers =       new List<Answer>();

            return base.MapData(row);
        }

    }

    public enum QuestionStatus
    {
        Active = 1,
        InActive = 0,
        Deleted = 2,
    }

    public enum QuestionLevel
    {
        Easy = 0,
        Medium = 1,
        Hard = 2,
    }
}
