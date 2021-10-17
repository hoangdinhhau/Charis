using Charis.Charis.ModelView.Catalog.UserModel;
using Charis.Data.EF;
using Charis.Data.Entities;
using Charis.ModelView.Catalog.UserModel;
using Charis.Utilities.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Charis.Application.Catalog.Users
{
    public class UserService : IUserService
    {
        private readonly CharisDbContext _context;
        private readonly IConfiguration _config;

        public UserService(CharisDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<int> Create(UserCreateRequest userCreateRequest)
        {
            try
            {
                var user = new User()
                {
                    FullName = userCreateRequest.FullName,
                    BirthDay = userCreateRequest.BirthDay,
                    Address = userCreateRequest.Address,
                    PassWord = Hash.hash(userCreateRequest.PassWord),
                    Email = userCreateRequest.Email,
                    RoleId = userCreateRequest.RoleId,
                    SoftDelete = false,
                };

                _context.Users.Add(user);
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> Delete(int userId)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null) throw new CharisException($"Cannot find a user {userId}");
                _context.Users.Remove(user);
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<UserViewModel>> GetAll()
        {
            var query = from u in _context.Users
                        join r in _context.Roles on u.RoleId equals r.Id
                        select new { u, r };
            var data = await query.Select(p => new UserViewModel()
            {
                Id = p.u.Id,
                FullName = p.u.FullName,
                BirthDay = p.u.BirthDay,
                Address = p.u.Address,
                PassWord = Hash.hash(p.u.PassWord),
                Email = p.u.Email,
                RoleName = p.r.RoleName
            }).ToListAsync();
            return data;
        }

        public async Task<UserViewModel> GetById(int Id)
        {
            var query = from u in _context.Users
                        join r in _context.Roles on u.RoleId equals r.Id
                        select new { u, r };
            var data = await query.Where(i => i.u.Id == Id).Select(p => new UserViewModel()
            {
                Id = p.u.Id,
                FullName = p.u.FullName,
                BirthDay = p.u.BirthDay,
                Address = p.u.Address,
                PassWord = Hash.hash(p.u.PassWord),
                Email = p.u.Email,
                RoleName = p.r.RoleName
            }).FirstAsync();
            return data;
        }

        public async Task<UserViewModel> GetByEmailAndPassWord(string email, string password)
        {
            var query = from u in _context.Users
                        join r in _context.Roles on u.RoleId equals r.Id
                        select new { u, r };
            var data = await query.Where(i => i.u.Email == email && i.u.PassWord == password).Select(p => new UserViewModel()
            {
                Id = p.u.Id,
                FullName = p.u.FullName,
                BirthDay = p.u.BirthDay,
                Address = p.u.Address,
                PassWord = p.u.PassWord,
                Email = p.u.Email,
                RoleName = p.r.RoleName
            }).FirstAsync();
            return data;
        }

        public async Task<UserViewModel> GetByEmail(string email)
        {
            var query = from u in _context.Users
                        join r in _context.Roles on u.RoleId equals r.Id
                        select new { u, r };
            var data = await query.Where(i => i.u.Email == email).Select(p => new UserViewModel()
            {
                Id = p.u.Id,
                FullName = p.u.FullName,
                BirthDay = p.u.BirthDay,
                Address = p.u.Address,
                PassWord = Hash.hash(p.u.PassWord),
                Email = p.u.Email,
                RoleName = p.r.RoleName
            }).FirstAsync();
            return data;
        }

        public async Task<int> Update(int userId, UserUpdateRequest userUpdateRequest)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null) throw new CharisException($"Cannot find a user {userId}");
                user.FullName = userUpdateRequest.FullName;
                user.BirthDay = userUpdateRequest.BirthDay;
                user.Address = userUpdateRequest.Address;
                user.PassWord = Hash.hash(userUpdateRequest.PassWord);
                user.Email = userUpdateRequest.Email;
                user.RoleId = userUpdateRequest.RoleId;
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> UpdateSoftDelete(int userId)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null) throw new CharisException($"Cannot find a user {userId}");
                user.SoftDelete = true;
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<string> Login(LoginRequest request)
        {
            var user = await GetByEmailAndPassWord(request.Email, Hash.hash(request.Password));
            if (user == null) throw new CharisException("Email or password not correc");

            var claims = new[]
            {
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.GivenName,user.FullName),
                new Claim(ClaimTypes.Role,user.RoleName),
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Tokens:Issuer"],
                _config["Tokens:Issuer"],
                claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}