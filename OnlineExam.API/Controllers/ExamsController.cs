using Microsoft.AspNetCore.Mvc;
using KFU.Common.Models;
using KFU.Data.Exams;
using KFU.Core.Interfaces.Exam;
using KFU.Core.Models.Exams;
using KFU.Core.Dtos;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using KFU.Core.Dtos.Request;
using KFU.Core.Dtos.Response;
using Microsoft.AspNetCore.Authorization;

namespace OnlineExam.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ExamsController : ControllerBase
    {
        private readonly IExam _examService;

        public ExamsController(IExam examService)
        {
            _examService = examService;
        }

        [HttpGet("Test") , AllowAnonymous]
        public IActionResult Test()
        {
            return Ok("Ok");
        }

        [HttpGet("GetExams")]
        public async Task<IActionResult> GetStudentExams([FromForm] string studentno )
        {
            var exams = new List<ExamTable>();
            try
            {
                // validate input
                // get all student exams for today
                var studentNoClaim = User.Claims.FirstOrDefault(c => c.Type == "StudentNo").Value;
                if (studentno == null || studentno != studentNoClaim)
                    return Ok(new { Exams = exams, Sucess = false , Status = 200, ErrorMessage = "رقم الطالب غير صحيح" });

                exams = await _examService.GetExamsForTodayAsync(studentno);
                return Ok(new {Exams = exams, Sucess = true, Status = 200 , ErrorMessage = ""});
            }
            catch (Exception ex)
            {
                return Ok(new { Exams = exams, Sucess = false, Status = 500, ErrorMessage = "عفوا حدث خطا في النظام يرجى المحاولة لاحقا" });
            }
        }

        [HttpGet("Questions")]
        public async Task<IActionResult> GetExamQuestions([FromForm] GetExamDto getExamDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //List<Answer> anss = await _examService.GetRandomAnswersAsync(5);
                    var examQuestions = await _examService.GetRandomQuestionsForExamAsync(getExamDto.ExamId);
                    if (examQuestions == null || examQuestions.Count == 0)
                        return Ok(new ExamQuestionsResponse("عفوا لا توجد اسئلة لهذا الامتحان", 204));

                    var qestionsResponse = new ExamQuestionsResponse();
                    qestionsResponse.Questions = examQuestions;
                    return Ok(qestionsResponse);
                }
                return Ok(new ExamQuestionsResponse(GetErrors(), 401));
            }
            catch(Exception ex)
            {
                return Ok(new ExamQuestionsResponse("عفوا حدث خطا في النظام يرجى المحاولة لاحقا", 500));
            }

        }
        [HttpPost("PostExam")]
        public async Task<IActionResult> PostAnswers( StudentAnswerDto studentAnswers)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // check if student data is correct
                    var studentNoClaim = User.Claims.FirstOrDefault(c => c.Type == "StudentNo").Value;
                    if (studentNoClaim != studentAnswers.StudentNo)
                        return Ok(new DefaultResponse { ErrorMessage = "عفوا لا يمكن حفظ اجابات طالب اخر", Status = 200, Success = false });

                    // check if student hav this exam for today
                    var todayExams = await _examService.GetExamsForTodayAsync(studentAnswers.StudentNo);
                    if(todayExams.FirstOrDefault(e => e.Id == studentAnswers.ExamId)  == null)
                        return Ok(new DefaultResponse { ErrorMessage = "عفوا لا يمكن حفظ اجابات اختبار  غير موجود ", Status = 200, Success = false });
                   

                    // check that all questions and answers are allowed
                    //if (!await _examService.ValidateQuestionsAndAnswers(studentAnswers.Answers))
                    //    return Ok(new DefaultResponse { ErrorMessage = "عفوا بيانات الاسئلة او الاجوبة غير صحيحة ", Status = 200, Success = false });


                    // Save all answers
                    foreach (var answer in studentAnswers.Answers)
                    {
                        await _examService.PostExamResultAsync(new SaveAnswerDto { AnswerId = answer.AnswerId, QuestionId = answer.QuestionId, ExamId = studentAnswers.ExamId, StudentNo = studentAnswers.StudentNo });
                    }
                    // Make a coorect for all exam answers
                    await _examService.CorrectSrudentExamAsync(new CorrectExam { ExamId = studentAnswers.ExamId , StudentNo = studentAnswers.StudentNo });
                    // return response
                    return Ok(new DefaultResponse { ErrorMessage="تم حفظ الاجابات بنجاح"});
                }
                return Ok(new DefaultResponse { ErrorMessage = GetErrors(), Status = 401, Success = false });
            }catch (Exception ex)
            {
                return Ok(new DefaultResponse { ErrorMessage = "حدث خطأ غير متوقع ، حاول لاحقا", Status = 500, Success = false });
            }
        }

        [HttpGet("StudentResult")]
        public async Task<IActionResult> StudentResult()
        {
            try
            {
                var result = await _examService.StudentResult("123456789", 1);
                return Ok(result);
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
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
