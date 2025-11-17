using System.Threading.Tasks;
using TechFood.BackOffice.Application.Common.Services.Interfaces;

namespace TechFood.BackOffice.Application.Common.Services;

public class EmailSender : IEmailSender
{
    public Task Send(string from, string to, string message)
    {
        return Task.CompletedTask;
    }
}
