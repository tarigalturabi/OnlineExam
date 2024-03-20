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
    public class ExamTable : BaseObject
    {
        public int Id { get; set; }
        public string CourseNo { get; set; } = Constants.NullString;
        public string Title { get; set; } = Constants.NullString;
        public DateTime ExamDate { get; set; } = Constants.NullDateTime;
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        // duration on minutes
        public int Duration { get; set; } = 60;

        public override bool MapData(DataRow row)
        {
            Id = GetInt(row, "ID");
            CourseNo = GetString(row, "CourseNo");
            Title = GetString(row, "Title");
            ExamDate = GetDateTime(row, "ExamDate").Date;
            StartTime = GetDateTime(row, "StartTime").TimeOfDay;
            EndTime = GetDateTime(row, "EndTime").TimeOfDay;
            Duration = GetInt(row, "EXAMDuration");
            return base.MapData(row);
        }
    }
}
