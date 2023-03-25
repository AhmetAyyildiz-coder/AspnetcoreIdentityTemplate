using System.ComponentModel.DataAnnotations;

namespace IdentityTemplate.Web.ViewModels;

public class ResetPasswordViewModel
{
    [Required(ErrorMessage = "Email Alanı Doldurulması Zorunludur.")]
    public string Email { get; set; }
}