using ECommerceAPI.Models;

namespace ECommerceAPI.Services
{
    public interface IPaymentService
    {
        Payment CreatePayment(Payment payment);

        List<Payment> GetPaymentsByUser(int userId);

        Payment? GetPaymentById(int id);
    }
}