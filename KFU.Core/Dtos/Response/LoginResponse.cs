using KFU.Common;
using KFU.Common.Models;
using KFU.Core.Models.Exams;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KFU.Core.Dtos.Response
{
    public class LoginResponse : BaseObject 
    {
        public int StudentId { get; set; }
        public string? StudentNo { get; set; }
        public string? StudentName { get; set; }
        public string? CollageName { get; set; }
        public List<ExamTable> Exams { get; set; } = new List<ExamTable>();
        public DefaultResponse ApiResponse { get; set; } = new DefaultResponse();
        public string? Token { get; set; }

        public override bool MapData(DataRow row)
        {
            StudentId = GetInt(row, "StudentId");
            StudentNo = GetString(row, "StudentNo");
            StudentName = GetString(row, "StudentName");
            CollageName = GetString(row, "CollageName");
            return base.MapData(row);
        }

        public LoginResponse()
        {
            
        }

        public LoginResponse(string errorMsg , int statusCode = 301)
        {
            ApiResponse = new DefaultResponse { Status= statusCode, Success = false , ErrorMessage = errorMsg};
        }
    } 
}
