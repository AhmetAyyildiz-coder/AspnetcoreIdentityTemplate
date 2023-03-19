using System.Collections;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace IdentityTemplate.Web.Extensions;

public static class ModelStateExtensions
{
    public static void AddModelErrorList(this ModelStateDictionary modelstate , List<string> errors)
    {
        foreach (var error in errors)
        {
            modelstate.AddModelError(string.Empty , error);
        }
    }
}