using KFU.Core.Dtos;
using KFU.Core.Dtos.Request;
using KFU.Core.Interfaces.Exam;
using KFU.Core.Models.Exams;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data;


namespace KFU.Data.Exams
{
    public class ExamSercive : DataServiceBase, IExam
    {
        public async Task<bool> CorrectSrudentExamAsync(CorrectExam correctExam)
        {
            var tuple = await ExecuteNonQueryAsync("ONLINEEXAM.SP_CORRECT_STUDENT_EXAM",
               CreateParameter("@EXAM_ID", OracleDbType.Int32, correctExam.ExamId),
               CreateParameter("@STUDENT_NO", OracleDbType.Varchar2, correctExam.StudentNo),
               CreateParameter("@DONE", OracleDbType.Int32, ParameterDirection.Output));
            
            if (tuple.Parameters["@DONE"] != null && Convert.ToInt32(tuple.Parameters["@DONE"].Value.ToString()) > 0)
                return true;
            else
                return false;
        }

        public async Task<List<ExamTable>> GetExamsForTodayAsync(string studentNo)
        {
            List < ExamTable > exams = new List < ExamTable >();
            var tuple = await ExecuteDataSetAsync("ONLINEEXAM.SP_GET_EXAMTABLE",
              CreateParameter("@TBL_ID", OracleDbType.Int32, paramValue:0),
              CreateParameter("@TBL_STUDENTNO", OracleDbType.Varchar2, studentNo),
              CreateParameter("@RET_VAL", OracleDbType.RefCursor, ParameterDirection.Output));

            if (tuple.Item1.Tables[0].Rows.Count > 0)            
                foreach (DataRow dr in tuple.Item1.Tables[0].Rows)
                {
                    ExamTable exam = new();
                    exam.MapData(dr);
                    exams.Add(exam);
                }
            return exams;
        }

        public async Task<List<Answer>> GetRandomAnswersAsync(int questionId)
        {
            List < Answer > answerList = new List < Answer >();
            var tuple = await ExecuteDataSetAsync("ONLINEEXAM.SP_GET_ANSWER",
              CreateParameter("@ANS_ID", OracleDbType.Int32, paramValue:0),
              CreateParameter("@ANS_QID", OracleDbType.Int32, questionId),
              CreateParameter("@RET_VAL", OracleDbType.RefCursor, ParameterDirection.Output));

            if (tuple.Item1.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in tuple.Item1.Tables[0].Rows)
                {
                    Answer answer = new();
                    answer.MapData(dr);
                    answerList.Add(answer);
                }
            }

            return answerList;            
        }

        public async Task<List<QuestionViewDto>> GetRandomQuestionsForExamAsync(int examid)
        {
            var tuple = await ExecuteDataSetAsync("ONLINEEXAM.SP_GET_QUESTIONFOREXAM",
               CreateParameter("@Q_EXAMID", OracleDbType.Int32, examid),
               CreateParameter("@RET_VAL", OracleDbType.RefCursor, ParameterDirection.Output));
            List<QuestionViewDto> Questions = new();
            if (tuple.Item1.Tables.Count > 0  && tuple.Item1.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in tuple.Item1.Tables[0].Rows)
                {
                    QuestionViewDto question = new();
                    question.MapData(dr);
                    question.Answers = await GetRandomAnswersAsync(question.QuestionId);
                    Questions.Add(question);
                }
            }
                       
            return Questions;
        }


        public async Task<bool> PostExamResultAsync(SaveAnswerDto studentAnswers)
        {
            //SP_SAVE_EXAMRESULT  (RES_ID IN NUMBER , RES_EXAMID IN NUMBER , RES_STUDENTNO IN NUMBER , RES_QUESTIONID IN NUMBER , RES_ANSWERID IN NUMBER , DONE OUT NUMBER)
            var tuple = await ExecuteNonQueryAsync("ONLINEEXAM.SP_SAVE_STUDENT_ANSWER",
               CreateParameter("@EXAM_ID", OracleDbType.Int32, studentAnswers.ExamId),
               CreateParameter("@STUDENT_NO", OracleDbType.Varchar2, studentAnswers.StudentNo),
               CreateParameter("@QUESTION_ID", OracleDbType.Int32, studentAnswers.QuestionId),
               CreateParameter("@ANSWER_ID", OracleDbType.Int32, studentAnswers.AnswerId),
               CreateParameter("@DONE", OracleDbType.Int32, ParameterDirection.Output));
            if (tuple.Parameters["@DONE"] != null && Convert.ToInt32(tuple.Parameters["@DONE"].Value.ToString()) > 0)
                return true;
            else
                return false;
        }

        public async Task<int> StudentResult(string studentno , int examid)
        {
            var tuple = await ExecuteNonQueryAsync("ONLINEEXAM.SP_CALCULATE_STUDENT_MARKS",
              CreateParameter("@EXAM_ID", OracleDbType.Int32, examid),
              CreateParameter("@STUDENT_NO", OracleDbType.Varchar2, studentno),
              CreateParameter("@DONE", OracleDbType.Int32, ParameterDirection.Output));
            if (tuple.Parameters["@DONE"] != null && Convert.ToInt32(tuple.Parameters["@DONE"].Value.ToString()) > 0)
                return Convert.ToInt32(tuple.Parameters["@DONE"].Value.ToString());
            else
                return 0;
        }

        public async Task<bool> ValidateQuestionsAndAnswers(List<QuestionAnswerDto> answers)
        {
            try
            {
                var tuple = await ExecuteNonQueryAsync("ONLINEEXAM.SP_ValidateQuestionsAndAnswers",
                 CreateParameter("@p_QuestionIDs", OracleDbType.Int32, OracleCollectionType.PLSQLAssociativeArray, answers.Select(q=> q.QuestionId).ToArray() ,answers.Count),
                 CreateParameter("@p_AnswerIDs", OracleDbType.Int32, OracleCollectionType.PLSQLAssociativeArray, answers.Select(a=> a.AnswerId).ToArray(),answers.Count));
                  return true;
            }
            catch(Exception ex)
            {
                return false;
            }
            throw new NotImplementedException();
        }



        // this method must deleted after chicking answers
        public void Validate_answes(List<int> questionIDs, List<int> answerIDs, string connectionString)
        {
            int[] q =  { 5, 4, 6, 12 }; 
            int[] a =  { 20 , 13 , 23 , 41 }; 
            using (var connection = new OracleConnection("user id=OnlineExam;password=123456;data source=10.146.24.83:1521;"))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "ONLINEEXAM.SP_ValidateQuestionsAndAnswers";

                    // Create input parameters for question IDs and answer IDs
                    var questionIDParameter = command.Parameters.Add("p_QuestionIDs", OracleDbType.Decimal);
                    questionIDParameter.CollectionType = OracleCollectionType.PLSQLAssociativeArray;
                    questionIDParameter.Value = q;
                    questionIDParameter.Size = questionIDs.Count;

                    var answerIDParameter = command.Parameters.Add("p_AnswerIDs", OracleDbType.Decimal);
                    answerIDParameter.CollectionType = OracleCollectionType.PLSQLAssociativeArray;
                    answerIDParameter.Value = a;
                    answerIDParameter.Size = answerIDs.Count;

                    try
                    {
                        command.ExecuteNonQuery();
                        Console.WriteLine("Validation successful.");
                    }
                    catch (OracleException ex)
                    {
                        // Handle the Oracle exception and display the error message
                        foreach (OracleError error in ex.Errors)
                        {
                            Console.WriteLine("Error code: " + error.Number);
                            Console.WriteLine("Error message: " + error.Message);
                        }
                    }
                }
            }
        }
    }
}
