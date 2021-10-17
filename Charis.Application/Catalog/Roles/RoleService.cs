using Charis.Application.Catalog.Roles;
using Charis.Charis.ModelView.Catalog.RoleModel;
using Charis.Data.EF;
using Charis.Data.Entities;
using Charis.Utilities.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Charis.Application.Catalog
{
    public class RoleService : IRoleService
    {
        private readonly CharisDbContext _context;

        public RoleService(CharisDbContext context)
        {
            _context = context;
        }

        public async Task<int> Create(RoleCreateRequest roleCreateRequest)
        {
            try
            {
                var role = new Role()
                {
                    RoleName = roleCreateRequest.RoleName,
                    SoftDelete = false,
                };

                _context.Roles.Add(role);
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> Delete(int roleId)
        {
            try
            {
                var role = await _context.Roles.FindAsync(roleId);
                if (role == null) throw new CharisException($"Cannot find a role {roleId}");
                _context.Roles.Remove(role);
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<RoleViewModel>> GetAll()
        {
            var data = await _context.Roles.Where(i => i.SoftDelete == false).Select(x => new RoleViewModel()
            {
                Id = x.Id,
                RoleName = x.RoleName
            }
            ).ToListAsync();
            return data;
        }

        public async Task<int> Update(int roleId, RoleUpdateRequest roleUpdateRequest)
        {
            try
            {
                var role = await _context.Roles.FindAsync(roleId);
                role.RoleName = roleUpdateRequest.RoleName;
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> UpdateSoftDelete(int roleId)
        {
            try
            {
                var role = await _context.Roles.FindAsync(roleId);
                role.SoftDelete = true;
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<RoleViewModel> GetById(int Id)
        {
            var data = await _context.Roles.Where(i => i.SoftDelete == false && i.Id == Id).Select(x => new RoleViewModel()
            {
                Id = x.Id,
                RoleName = x.RoleName
            }
            ).FirstAsync();
            return data;
        }
    }
}