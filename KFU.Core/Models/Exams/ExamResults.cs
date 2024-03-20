using KFU.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KFU.Core.Models.Exams
{
    public class ExamResults
    {
        public int Id { get; set; } = Constants.NullInt;
        public int ExamId { get; set; } = Constants.NullInt;
        public string StudentNo { get; set; } = Constants.NullString;
        public int QuestionId { get; set; } = Constants.NullInt;
        public int TrueAnswerId { get; set; } = Constants.NullInt;
        public int AnswerId { get; set; } = Constants.NullInt;
    } 
}
