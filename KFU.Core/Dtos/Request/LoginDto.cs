using System.ComponentModel.DataAnnotations;
using KFU.Common;
namespace KFU.Core.Dtos.Request
{
    public class LoginDto
    {
        [Required(ErrorMessage = "الرجاء ادخال رقم الطالب")]
        [StringLength(9, MinimumLength = 9, ErrorMessage = "عفوا يجب ان يتكون رقم الطالب من 9 ارقام")]
        [RegularExpression("([0-9][0-9]*)", ErrorMessage = "الرجاء ادخال ارقام فقط")]
        public string StudentNo { get; set; } = Constants.NullString;

        [Required(ErrorMessage = "الرجاء ادخال كلمة المرور ")]
        [StringLength(16, MinimumLength = 8, ErrorMessage = "عفوا! . يجب ان يكون طول كلمة المرور بين 8 و 16 حرف ")]
        [RegularExpression("(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?=.*[*+\\/|!\"£$%^&*()#[\\]@~'?><,.=-_]).{6,}", ErrorMessage = "عفوا!. يجب ان تحتوي كلمة المرور على حروف كبيرة , وحروف صغيرة , وارقام , ورموز")]
        [DataType(DataType.Password)]
        public string StudentPassword { get; set; } = Constants.NullString;

    }
}
