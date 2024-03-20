using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KFU.Core.Dtos
{
    public class SaveAnswerDto
    {
        public int ExamId { get; set; }
        public string StudentNo { get; set; }        
        public int QuestionId { get; set; }
        public int AnswerId { get; set; }
    }
}
