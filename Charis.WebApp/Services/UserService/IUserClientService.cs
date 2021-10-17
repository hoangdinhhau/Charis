using Charis.Charis.ModelView.Catalog.UserModel;
using Charis.ModelView.Catalog.UserModel;
using Charis.ModelView.Common;
using System.Threading.Tasks;

namespace Charis.WebApp.Services.UserService
{
    public interface IUserClientService
    {
        Task<string> Login(LoginRequest loginRequest);

        Task<ApiResult<UserViewModel>> GetByEmail(string email);

        Task<ApiResult<bool>> CreateUser(UserCreateRequest equest);
    }
}