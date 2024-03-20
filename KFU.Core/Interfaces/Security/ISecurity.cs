using KFU.Core.Authentication;
using KFU.Core.Dtos.Request;
using KFU.Core.Dtos.Response;

namespace KFU.Core.Interfaces.Security
{
    public interface ISecurity
    {
         Task<LoginResponse> LogStudent(LoginDto loginDto);
    }
}
