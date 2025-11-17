using System.ComponentModel.DataAnnotations;

namespace TechFood.BackOffice.Contracts.Authentication;

public record SignInRequest(
    [Required] string Username,
    [Required] string Password);
