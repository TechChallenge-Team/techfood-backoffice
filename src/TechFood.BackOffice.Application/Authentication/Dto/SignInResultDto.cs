namespace TechFood.BackOffice.Application.Authentication.Dto;

public record SignInResultDto(string AccessToken, string RefreshToken, int ExpiresIn, UserDto User);
