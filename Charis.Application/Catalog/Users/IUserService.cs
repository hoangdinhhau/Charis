using Charis.Charis.ModelView.Catalog.UserModel;
using Charis.ModelView.Catalog.UserModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Charis.Application.Catalog.Users
{
    public interface IUserService
    {
        Task<int> Create(UserCreateRequest userCreateRequest);

        Task<int> Update(int userId, UserUpdateRequest userUpdateRequest);

        Task<int> Delete(int userId);

        Task<int> UpdateSoftDelete(int userId);

        Task<List<UserViewModel>> GetAll();

        Task<UserViewModel> GetById(int Id);

        Task<string> Login(LoginRequest request);

        Task<UserViewModel> GetByEmail(string email);
    }
}