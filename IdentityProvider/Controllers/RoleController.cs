using IdentityProvider.Dtos;
using IdentityProvider.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace IdentityProvider.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        public RoleController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpPost("add")]
        [ProducesResponseType(typeof(ApiResult), 200)]
        public IActionResult Add(AddRoleDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);

            var existRole = _dbContext.Roles.Any(x => x.Name == dto.Name);
            if (existRole)
                return BadRequest(ApiResultHandler.Failed("نقش با این نام وجود دارد"));
            var role = new Role { Name = dto.Name };
            _dbContext.Roles.Add(role);
            _dbContext.SaveChanges();
            return Ok(ApiResultHandler.Ok());
        }
        [HttpPut("edit")]
        [ProducesResponseType(typeof(ApiResult), 200)]
        public async Task<IActionResult> Edit(EditRoleDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);

            var role = await _dbContext.Roles.FindAsync(dto.Id);
            if (role is null)
                return NotFound(ApiResultHandler.Failed("نقش یاقت نشد", HttpStatusCode.NotFound));

            var existRole = _dbContext.Roles.Any(x => x.Name == dto.Name && x.Id != dto.Id);
            if (existRole)
                return BadRequest(ApiResultHandler.Failed("نقش با این نام وجود دارد"));
            role.Name = dto.Name;
            _dbContext.SaveChanges();
            return Ok(ApiResultHandler.Ok());
        }
        [HttpDelete("delete")]
        [ProducesResponseType(typeof(ApiResult), 200)]
        public async Task<IActionResult> Add([FromQuery] Guid id)
        {
            var role = await _dbContext.Roles.FindAsync(id);
            if (role is null)
                return NotFound(ApiResultHandler.Failed("نقش یاقت نشد", HttpStatusCode.NotFound));
            _dbContext.Roles.Remove(role);
            _dbContext.SaveChanges();
            return Ok(ApiResultHandler.Ok());
        }
        [HttpGet("get-by-id")]
        [ProducesResponseType(typeof(ApiResult<RoleDto>), 200)]
        public async Task<IActionResult> GetById([FromQuery] Guid id)
        {
            var role = await _dbContext.Roles.FindAsync(id);
            return Ok(ApiResultHandler<RoleDto>.Ok(new RoleDto { Id = role.Id, Name = role.Name }));
        }
        [HttpGet("get-all")]
        [ProducesResponseType(typeof(ApiResult<List<RoleDto>>), 200)]
        public async Task<IActionResult> GetAll()
        {
            var roles = await _dbContext.Roles.AsNoTracking().Select(x => new RoleDto
            {
                Id = x.Id,
                Name = x.Name,
            }).ToListAsync();
            return Ok(ApiResultHandler<List<RoleDto>>.Ok(roles));
        }


        [HttpGet("get-all-dropdown")]
        [ProducesResponseType(typeof(ApiResult<List<RoleForDropDownDto>>), 200)]
        public async Task<IActionResult> GetAllDropDown()
        {
            var roles = await _dbContext.Roles.AsNoTracking().Select(x => new RoleForDropDownDto
            {
                Key = x.Id.ToString(),
                Value = x.Name,
            }).ToListAsync();
            return Ok(ApiResultHandler<List<RoleForDropDownDto>>.Ok(roles));
        }


        [HttpGet("get-role-permission")]
        public async Task<IActionResult> GetRolePermission(Guid roleId)
        {
            var rolePermisisona = await _dbContext.RoleActions.Where(x => x.RoleId == roleId).ToListAsync();
            var result = rolePermisisona.Select(x => new { x.ActionId });
            return Ok(ApiResultHandler<object>.Ok(result));
        }

        [HttpPost("add-role-permission")]
        public IActionResult AddRolePermission(AddRolePermissionDto dto)
        {
            _dbContext.RoleActions.RemoveRange(_dbContext.RoleActions.Where(x => x.RoleId == dto.RoleId));
            _dbContext.RoleActions.AddRange(dto.ActionId.Select(x => new RoleAction
            {
                RoleId = dto.RoleId,
                ActionId = x
            }));
            _dbContext.SaveChanges();
            return Ok(ApiResultHandler.Ok());

        }

        [HttpGet("get-all-permissions")]
        public async Task<IActionResult> GetAllPermissions()
        {
            var actions = await _dbContext.Actions.Select(x => new PermissionListDto { Id = x.Id, Name = x.Name }).ToListAsync();
            return Ok(ApiResultHandler<List<PermissionListDto>>.Ok(actions));
        }
    }
}
