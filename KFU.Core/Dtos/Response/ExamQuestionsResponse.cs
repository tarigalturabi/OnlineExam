using KFU.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KFU.Core.Dtos.Response
{
    public class ExamQuestionsResponse
    {
        public DefaultResponse ApiResponse { get; set; } = new DefaultResponse();
        public List<QuestionViewDto> Questions { get; set; } = new List<QuestionViewDto>();


        public ExamQuestionsResponse() { }
        public ExamQuestionsResponse(string errorMsg, int statusCode = 301)
        {
            ApiResponse = new DefaultResponse { Status = statusCode, Success = false, ErrorMessage = errorMsg };
        }
    }
}
