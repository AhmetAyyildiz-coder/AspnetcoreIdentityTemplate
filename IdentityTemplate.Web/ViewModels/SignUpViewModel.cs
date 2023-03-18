using System.ComponentModel.DataAnnotations;

namespace IdentityTemplate.Web.ViewModels;

public class SignUpViewModel
{
    [Required(ErrorMessage = "Kullanıcı Adı Zorunludur.")]
    public string Username { get; set; }
    [Required(ErrorMessage = "Email Gereklidir.")]
    public string Email { get; set; }
    [Required(ErrorMessage = "Telefon Numarası Gereklidir.")]
    public string Phone { get; set; }
    [Required(ErrorMessage = "Şifre Gereklidir.")]
    public string Password { get; set; }
}