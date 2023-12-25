using IdentityProvider.Dtos;
using IdentityProvider.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static IdentityProvider.Models.Confirmation;

namespace IdentityProvider.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _dbcontext;
        private readonly ITokenService _tokenService;
        public UserController(AppDbContext dbcontext, ITokenService tokenService)
        {
            _dbcontext = dbcontext;
            _tokenService = tokenService;
        }

        /// <summary>
        /// Registers a user.
        /// </summary>
        /// <param name="dto">The registration data.</param>
        /// <returns>The registration response.</returns>
        [HttpPost("register")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResult<RegisterResponse>), 200)]
        public IActionResult Register(RegisterDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);
            var existCu = _dbcontext.Users.FirstOrDefault(x => x.Mobile == dto.Mobile);
            if (existCu is not null)
            {
                var verifyCode = InserConfirmation(existCu);
                _dbcontext.SaveChanges();
                return Ok(ApiResultHandler<RegisterResponse>.Ok(verifyCode));
            }
            else
            {
                var cu = new User { Mobile = dto.Mobile };
                _dbcontext.Users.Add(cu);
                var verifyCode = InserConfirmation(cu);
                _dbcontext.SaveChanges();
                return Ok(ApiResultHandler<RegisterResponse>.Ok(verifyCode));
            }
        }

        [HttpPost("customer-register")]
        [ProducesResponseType(typeof(ApiResult), 200)]
        public IActionResult CustomerRegister(RegisterDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);
            var existCu = _dbcontext.Users.FirstOrDefault(x => x.Mobile == dto.Mobile);
            if (existCu is null)
            {
                var cu = new User { Mobile = dto.Mobile };
                _dbcontext.Users.Add(cu);
                _dbcontext.SaveChanges();
                return Ok(ApiResultHandler.Ok());
            }
            return BadRequest(ApiResultHandler.Failed(message: "مشتری با این مشخصات وجود دارد."));
        }


        [HttpPost("confirmation")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResult<string>), 200)]
        [ProducesResponseType(typeof(ApiResult), 400)]
        public IActionResult Confirmation(ConfirmationDto dto)
        {
            var confirmation = _dbcontext.Confirmations.FirstOrDefault(x => x.Token == dto.Token && x.Active);

            if (confirmation is not null && confirmation.Code == dto.VerifyCode)
            {
                var customer = _dbcontext.Users.Find(confirmation.UserId);
                _dbcontext.Confirmations.Remove(confirmation);
                _dbcontext.SaveChanges();
                return Ok(_tokenService.GenerateToken(customer));
            }
            return BadRequest(ApiResultHandler.Failed("کد وارد شده صحیح نیست"));
        }



        [HttpPost("add-customer-role")]
        [ProducesResponseType(typeof(ApiResult), 200)]
        public IActionResult AddCustomerRole(AddCustomerRoleDto dto)
        {
            var customerRoles = _dbcontext.CustomerRoles.Where(x => x.CustomerId == dto.CustomerId).ToList();
            _dbcontext.CustomerRoles.RemoveRange(customerRoles);

            customerRoles = new List<CustomerRole>();
            foreach (var item in dto.RoleIds)
            {
                customerRoles.Add(new CustomerRole() { CustomerId = dto.CustomerId, RoleId = item });
            }
            _dbcontext.CustomerRoles.AddRange(customerRoles);
            _dbcontext.SaveChanges();
            return Ok(ApiResultHandler.Ok());
        }

        [HttpGet("get-customer-roles")]
        [ProducesResponseType(typeof(ApiResult<List<CustomerRolesDto>>), 200)]

        public async Task<IActionResult> GetCustomerRoles([FromQuery] Guid id)
        {
            var customerRoles = await _dbcontext.CustomerRoles.AsNoTracking().Where(x => x.CustomerId == id).Select(x => new CustomerRolesDto { Id = x.RoleId }).ToListAsync();
            return Ok(ApiResultHandler<List<CustomerRolesDto>>.Ok(customerRoles));
        }


        [HttpGet("get-all")]
        [ProducesResponseType(typeof(ApiResult<List<CustomerDto>>), 200)]

        public async Task<IActionResult> GetAllCustomer()
        {
            var customers = await _dbcontext.Users.AsNoTracking().Select(c => new CustomerDto
            {
                Id = c.Id,
                Email = c.Email,
                FirstName = c.FirstName,
                LastName = c.LastName,
                Mobile = c.Mobile
            }).ToListAsync();
            return Ok(ApiResultHandler<List<CustomerDto>>.Ok(customers));
        }

        [HttpGet("get-all-dropdown")]
        [ProducesResponseType(typeof(ApiResult<List<GetAllDropdownDto>>), 200)]

        public async Task<IActionResult> GetAllDropDown()
        {
            var customers = await _dbcontext.Users.AsNoTracking().Select(c => new GetAllDropdownDto
            {
                Key = c.Id,
                Value = c.Mobile
            }).ToListAsync();
            return Ok(ApiResultHandler<List<GetAllDropdownDto>>.Ok(customers));
        }

        [HttpGet("get-by-id")]
        [ProducesResponseType(typeof(ApiResult<CustomerDto>), 200)]
        [ProducesResponseType(typeof(CustomerDto), 400)]

        public async Task<IActionResult> GetById(Guid id)
        {
            var customer = await _dbcontext.Users.FindAsync(id);
            if (customer == null)
                return BadRequest(ApiResultHandler.Failed("مشتری وجود ندارد"));
            return Ok(ApiResultHandler<CustomerDto>.Ok(new CustomerDto
            {
                Id = id,
                Email = customer.Email,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Mobile = customer.Mobile
            }));
        }

        [HttpPut("edit")]
        [ProducesResponseType(typeof(ApiResult), 200)]

        public async Task<IActionResult> Edit(EditCustomerDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);
            var customer = await _dbcontext.Users.FindAsync(dto.Id);
            if (customer == null)
                return BadRequest(ApiResultHandler.Failed("مشتری وجود ندارد"));
            customer.Email = dto.Email;
            customer.FirstName = dto.FirstName;
            customer.LastName = dto.LastName;
            customer.Mobile = dto.Mobile;
            await _dbcontext.SaveChangesAsync();
            return Ok(ApiResultHandler.Ok());
        }
        private RegisterResponse InserConfirmation(User customer)
        {
            _dbcontext.Confirmations.RemoveRange(_dbcontext.Confirmations.Where(x => x.UserId == customer.Id && x.Type == ConfirmatonType.Mobile));
            var verifyCode = new Random().Next(10000, 20000);
            Confirmation confirmation = new Confirmation
            {
                Code = verifyCode,
                ExpireTime = DateTime.Now.AddSeconds(130),
                Active = true,
                Type = ConfirmatonType.Mobile,
                UserId = customer.Id,
                Token = Guid.NewGuid().ToString(),
            };
            _dbcontext.Confirmations.Add(confirmation);
            var message = $"کد تایید شما : {verifyCode}  \n Tahoorashop.com";
            //To DO: Send Sms
            return new RegisterResponse(verifyCode, confirmation.Token);
        }
    }
}
