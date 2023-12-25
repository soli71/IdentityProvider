namespace IdentityProvider.Dtos;

public class RegisterDto
{
    public string Mobile { get; set; }
}

public class EditCustomerDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Mobile { get; set; }
    public string Email { get; set; }
}
public class RegisterResponse
{
    public RegisterResponse(int verifyCode, string token)
    {
        VerifyCode = verifyCode;
        Token = token;
    }
    public int VerifyCode { get; set; }
    public string Token { get; set; }
}
public class ConfirmationDto
{
    public int VerifyCode { get; set; }
    public string Token { get; set; }
}
public class CustomerDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Mobile { get; set; }
    public string Email { get; set; }
}
public class GetAllDropdownDto
{
    public Guid Key { get; set; }
    public string Value { get; set; }
}
public class AddCustomerRoleDto
{
    public Guid CustomerId { get; set; }
    public List<Guid> RoleIds { get; set; }
}

public class CustomerRolesDto
{
    public Guid Id { get; set; }
}


