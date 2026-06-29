using ECommerceAPI.Models;
using ECommerceAPI.Repositories;

namespace ECommerceAPI.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentService(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public Payment CreatePayment(Payment payment)
        {
            return _paymentRepository.CreatePayment(payment);
        }

        public List<Payment> GetPaymentsByUser(int userId)
        {
            return _paymentRepository.GetPaymentsByUser(userId);
        }

        public Payment? GetPaymentById(int id)
        {
            return _paymentRepository.GetPaymentById(id);
        }
    }
}