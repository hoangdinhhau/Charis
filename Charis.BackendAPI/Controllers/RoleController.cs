using Charis.Application.Catalog.Roles;
using Charis.Charis.ModelView.Catalog.RoleModel;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Charis.BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var roleData = await _roleService.GetAll();
            return Ok(roleData);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(int Id)
        {
            var roleData = await _roleService.GetById(Id);
            return Ok(roleData);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RoleCreateRequest request)
        {
            var result = await _roleService.Create(request);
            if (result == 0)
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> Update(int Id, [FromBody] RoleUpdateRequest request)
        {
            var result = await _roleService.Update(Id, request);
            if (result == 0)
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpPut("softDelete/{Id}")]
        public async Task<IActionResult> UpdateSoftDelete(int Id)
        {
            var result = await _roleService.UpdateSoftDelete(Id);
            if (result == 0)
            {
                return BadRequest();
            }
            return Ok();
        }
    }
}