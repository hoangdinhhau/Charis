using Charis.Charis.ModelView.Catalog.RoleModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Charis.Application.Catalog.Roles
{
    public interface IRoleService
    {
        Task<int> Create(RoleCreateRequest roleCreateRequest);

        Task<int> Update(int roleId, RoleUpdateRequest roleUpdateRequest);

        Task<int> Delete(int roleId);

        Task<int> UpdateSoftDelete(int roleId);

        Task<List<RoleViewModel>> GetAll();

        Task<RoleViewModel> GetById(int Id);
    }
}