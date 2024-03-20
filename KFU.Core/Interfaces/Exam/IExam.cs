using KFU.Core.Dtos;
using KFU.Core.Dtos.Request;
using KFU.Core.Models.Exams;

namespace KFU.Core.Interfaces.Exam
{
    public interface IExam
    {
        Task<List<ExamTable>> GetExamsForTodayAsync(string studentId );
        Task<List<QuestionViewDto>> GetRandomQuestionsForExamAsync(int examid );
        Task<List<Answer>> GetRandomAnswersAsync(int questionId);

        Task<bool> PostExamResultAsync(SaveAnswerDto studentAnswers);
        Task<bool> CorrectSrudentExamAsync(CorrectExam correctExam);
        Task<int> StudentResult(string studentno, int examid);
        Task<bool> ValidateQuestionsAndAnswers(List<QuestionAnswerDto> answers);

        // this method must deleted after chicking answer
        void Validate_answes(List<int> questionIDs, List<int> answerIDs, string connectionString);

        //Task<StudentData> GetStudentDataAsync(string studentId );

        //int AddExam(CourseExam exam );
        //bool UpdateExam(CourseExam exam );
        //bool DeleteExam(int examId );
        //Task<CourseExam> GetExamByIdAsync( int ExamId );



        //int AddQuestion(Question question);
        //bool DeleteQuestion(Question question);
        //bool UpdateQuestion(Question question);
        //Task<Question> GetQuestionAsync(int questionId);
        //Task<List<Question>> GetAllQuestionsAsync();

        //int AddAnswer(Answer answer);
        //bool UpdateAnswer(Answer answer);
        //bool DeleteAnswer(Answer answer);
        //Task<Answer> GetAnswerAsync(int AnswerId);

        //Task<CourseExam> GetExamViewAsync(string studentNo , string CourseNo );

    }
}
