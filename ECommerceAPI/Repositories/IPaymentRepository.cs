using ECommerceAPI.Models;

namespace ECommerceAPI.Repositories
{
    public interface IPaymentRepository
    {
        Payment CreatePayment(Payment payment);
        List<Payment> GetPaymentsByUser(int userId);
        Payment? GetPaymentById(int id);
    }
}