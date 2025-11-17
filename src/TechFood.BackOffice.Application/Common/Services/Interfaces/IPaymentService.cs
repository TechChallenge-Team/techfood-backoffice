using System.Threading.Tasks;
using TechFood.BackOffice.Application.Common.Data;

namespace TechFood.BackOffice.Application.Common.Services.Interfaces;

public interface IPaymentService
{
    Task<QrCodePaymentResult> GenerateQrCodePaymentAsync(QrCodePaymentRequest request);
}
