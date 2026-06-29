using ECommerceAPI.Data;
using ECommerceAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly AppDbContext _context;
        public PaymentRepository(AppDbContext context)
        {
            _context = context;
        }

        public Payment CreatePayment(Payment payment)
        {
            _context.Payments.Add(payment);
            _context.SaveChanges();

            return payment;
        }

        public List<Payment> GetPaymentsByUser(int userId)
        {
            return _context.Payments.Include(p => p.Order).Where(p => p.Order!.UserId == userId).ToList();
        }

        public Payment? GetPaymentById(int id)
        {
            return _context.Payments.Include(p => p.Order).FirstOrDefault(p => p.Id == id);
        }
    }
}