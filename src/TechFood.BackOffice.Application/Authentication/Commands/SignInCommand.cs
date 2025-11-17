using MediatR;
using TechFood.BackOffice.Application.Authentication.Dto;

namespace TechFood.BackOffice.Application.Authentication.Commands;

public record SignInCommand(string Username, string Password) : IRequest<SignInResultDto>;
