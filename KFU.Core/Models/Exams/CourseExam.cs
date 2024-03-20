using KFU.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace KFU.Core.Models.Exams
{
    public class CourseExam
    {
        public int ExamId { get; set; } = Constants.NullInt;
        public string CourseNo { get; set; } = Constants.NullString;
        public double FullMark { get; set; } = 100;
        public double passMark { get; set; } = 50;
        public int NoOfQuestions { get; set; } = 1;
        public int Complexity { get; set; } = 50;
    }
}
