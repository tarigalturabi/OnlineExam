using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace KFU.Common.Models
{
    public class ResponseModel<T> where T : class
    {
        public bool Success { get; set; } = true;
        public int Status { get; set; } = 200;
        public string ErrorMessage { get; set; } = string.Empty;
        public string TextData { get; set; }
        public List<T> ListData { get; set; } = new List<T>();

        public ResponseModel() { }
        public ResponseModel(bool success , int status , string err , string textdata , List<T> listOfObjects)
        {
            Success = success;
            Status = status; 
            ErrorMessage = err;
            TextData = textdata;
            ListData = listOfObjects;
             
        }

        public  ResponseModel<T> ErrorResponse(int errcode , string message)
        {       
            return new ResponseModel<T>(false , errcode , message , null , new List<T>());
        }

        public  ResponseModel<T> OKResponse( List<T> listOfObjects , string textdata = "" , string message ="")
        {
            return new ResponseModel<T>(true, 200, message, textdata, listOfObjects);
        }
    }
}
