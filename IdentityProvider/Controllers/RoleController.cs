using IdentityProvider.Dtos;
using IdentityProvider.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace IdentityProvider.Controllers
{
    /// <summary>
    /// Controller for managing roles.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleController"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public RoleController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Adds a new role.
        /// </summary>
        /// <param name="dto">The role data.</param>
        /// <returns>The result of the operation.</returns>
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

        /// <summary>
        /// Edits an existing role.
        /// </summary>
        /// <param name="dto">The role data.</param>
        /// <returns>The result of the operation.</returns>
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

        /// <summary>
        /// Deletes a role.
        /// </summary>
        /// <param name="id">The role ID.</param>
        /// <returns>The result of the operation.</returns>
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

        /// <summary>
        /// Gets a role by ID.
        /// </summary>
        /// <param name="id">The role ID.</param>
        /// <returns>The role data.</returns>
        [HttpGet("get-by-id")]
        [ProducesResponseType(typeof(ApiResult<RoleDto>), 200)]
        public async Task<IActionResult> GetById([FromQuery] Guid id)
        {
            var role = await _dbContext.Roles.FindAsync(id);
            return Ok(ApiResultHandler<RoleDto>.Ok(new RoleDto { Id = role.Id, Name = role.Name }));
        }

        /// <summary>
        /// Gets all roles.
        /// </summary>
        /// <returns>The list of roles.</returns>
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

        /// <summary>
        /// Gets all roles for dropdown.
        /// </summary>
        /// <returns>The list of roles for dropdown.</returns>
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

        /// <summary>
        /// Gets the permissions for a role.
        /// </summary>
        /// <param name="roleId">The role ID.</param>
        /// <returns>The role permissions.</returns>
        [HttpGet("get-role-permission")]
        public async Task<IActionResult> GetRolePermission(Guid roleId)
        {
            var rolePermisisona = await _dbContext.RoleActions.Where(x => x.RoleId == roleId).ToListAsync();
            var result = rolePermisisona.Select(x => new { x.ActionId });
            return Ok(ApiResultHandler<object>.Ok(result));
        }

        /// <summary>
        /// Adds permissions to a role.
        /// </summary>
        /// <param name="dto">The role permission data.</param>
        /// <returns>The result of the operation.</returns>
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

        /// <summary>
        /// Gets all permissions.
        /// </summary>
        /// <returns>The list of permissions.</returns>
        [HttpGet("get-all-permissions")]
        public async Task<IActionResult> GetAllPermissions()
        {
            var actions = await _dbContext.Actions.Select(x => new PermissionListDto { Id = x.Id, Name = x.Name }).ToListAsync();
            return Ok(ApiResultHandler<List<PermissionListDto>>.Ok(actions));
        }
    }
}
