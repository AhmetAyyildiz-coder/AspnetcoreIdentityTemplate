using System.ComponentModel.DataAnnotations;

namespace IdentityTemplate.Web.ViewModels;

public class LoginViewModel
{
    public LoginViewModel()
    {
    }

    public LoginViewModel(string email , string password)
    {
        this.Email = email;
        this.Password = password;
    }

    [Required(ErrorMessage = "Email Alanı Doldurmak Zorunludur.")]
    [EmailAddress(ErrorMessage = "Email Formatı Hatalıdır. ")]
    public string Email { get; set; }



    [Required(ErrorMessage = "Password alanı doldurmak zorunludur.")]
    public string Password { get; set; }

    public bool rememberMe { get; set; }
}