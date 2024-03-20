using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KFU.Common.Models
{
    public class DefaultResponse
    {
        public bool Success { get; set; } = true;
        public int Status { get; set; } = 200;
        public string ErrorMessage { get; set; } = string.Empty;
    }
}
