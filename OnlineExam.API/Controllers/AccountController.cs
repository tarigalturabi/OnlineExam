using KFU.Common.Models;
using KFU.Core.Dtos.Request;
using KFU.Core.Dtos.Response;
using KFU.Core.Interfaces.Exam;
using KFU.Core.Interfaces.Security;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OnlineExam.API.Controllers
{
    [Route("api/[controller]")]
    //[ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ISecurity _security;
        private readonly IExam _examService;
        public AccountController(ISecurity security, IExam examService)
        {
            _security = security;
            _examService = examService;
        }
         
        [HttpPost("StudentLogin")]
        public async Task<IActionResult> StudentLogin([FromForm] LoginDto loginDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var loginResponse = await _security.LogStudent(loginDto);
                    if (loginResponse.ApiResponse.Success) {
                        var todayExams = await _examService.GetExamsForTodayAsync(loginDto.StudentNo);
                        loginResponse.Exams = todayExams;
                    }
                    return Ok(loginResponse);
                }
                return Ok(new LoginResponse(GetErrors(), 401));
            }catch (Exception ex)
            {
                return Ok(new LoginResponse("عفوا!. حدث خطأ في النظام يرجى المحاولة مرة اخرى" , 500));
            }
        }
        

        private string GetErrors()
        {
            string errors = string.Empty;
            foreach (var modelState in ModelState.Values)
            {
                foreach (var modelError in modelState.Errors)
                {
                    errors += modelError.ErrorMessage + " , ";
                }

            }
            return errors;
        }               
    }
}
