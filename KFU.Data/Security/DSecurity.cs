using KFU.Core.Authentication;
using KFU.Core.Dtos.Request;
using KFU.Core.Dtos.Response;
using KFU.Core.Interfaces.Security;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace KFU.Data.Security
{
    public class DSecurity:DataServiceBase, ISecurity
    {
        private readonly JwtSettings _jwtSettings;
        public DSecurity(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<LoginResponse> LogStudent(LoginDto loginDto)
        {
            // Validate user credentials against the user repository
            var student = await CheckLogin(loginDto);

            if (student == null )
            {
                return new LoginResponse("عفوا ! . انت غير مخول للدخول الى النظام");
            }
             
            // Generate JWT token
            var token = GenerateJwtToken(student);
            student.Token = token;
            return student;
        }

        private string GenerateJwtToken(LoginResponse student)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.NameIdentifier, student.StudentId.ToString()),
                new Claim("StudentNo", student.StudentNo.ToString()),
                new Claim(ClaimTypes.Name, student.StudentName),
            }),
                Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }



        public async Task<LoginResponse> CheckLogin(LoginDto login)
        {
            var tuple = await ExecuteDataSetAsync("ONLINEEXAM.SP_LOG_STUDENT",
                 DataServiceBase.CreateParameter("@STUDENT_NO", OracleDbType.Varchar2, login.StudentNo),
                 DataServiceBase.CreateParameter("@STUDENT_PASSWORD", OracleDbType.Varchar2, login.StudentPassword),
                 DataServiceBase.CreateParameter("@RET_VAL", OracleDbType.RefCursor, ParameterDirection.Output));
            LoginResponse Object = new();
            if (!Object.MapData(tuple.Item1))
            {
                Object = null;
            }
            return Object;
        }
    }

    
}
