using System.ComponentModel.DataAnnotations;

namespace IdentityServerHost.Pages.Register;

public class InputModel
{
    [Required]
    [Display(Name = "User name")]
    public string Username { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [Compare("Password")]
    [Display(Name = "Confirm password")]
    public string ConfirmPassword { get; set; }

    public string ReturnUrl { get; set; }
}
