using Charis.Application.Catalog.Users;
using Charis.Charis.ModelView.Catalog.UserModel;
using Charis.ModelView.Catalog.UserModel;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Charis.BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _UserService;

        public UserController(IUserService UserService)
        {
            _UserService = UserService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var UserData = await _UserService.GetAll();
            return Ok(UserData);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(int Id)
        {
            var UserData = await _UserService.GetById(Id);
            return Ok(UserData);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserCreateRequest request)
        {
            var result = await _UserService.Create(request);
            if (result == 0)
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> Update(int Id, [FromBody] UserUpdateRequest request)
        {
            var result = await _UserService.Update(Id, request);
            if (result == 0)
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpPut("softDelete/{Id}")]
        public async Task<IActionResult> UpdateSoftDelete(int Id)
        {
            var result = await _UserService.UpdateSoftDelete(Id);
            if (result == 0)
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _UserService.Login(request);
            if (string.IsNullOrEmpty(result))
            {
                return BadRequest("Email or password is incorrect");
            }
            return Ok(result);
        }

        [HttpGet("email/{email}")]
        public async Task<IActionResult> GetByEmail(string email)
        {
            var UserData = await _UserService.GetByEmail(email);
            return Ok(UserData);
        }
    }
}