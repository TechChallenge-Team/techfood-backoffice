using System;

namespace TechFood.BackOffice.Application.Authentication.Dto;

public record UserDto(Guid Id, string Name, string Username, string? Email, string Role);
