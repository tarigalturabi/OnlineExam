using KFU.Common;
using KFU.Common.Models;
using KFU.Core.Models.Exams;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KFU.Core.Dtos
{ 
    public class QuestionViewDto : BaseObject
    { 
        public int QuestionId { get; set; } = Constants.NullInt;
        public string QuestionText { get; set; } = Constants.NullString;
        public List<Answer> Answers { get; set; } = new List<Answer>();


        public override bool MapData(DataRow row)
        {
            QuestionId = GetInt(row, "ID");
            QuestionText = GetString(row, "QText");
            return base.MapData(row);
        }

       
    }
}
