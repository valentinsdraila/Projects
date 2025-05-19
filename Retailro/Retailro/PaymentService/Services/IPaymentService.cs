using PaymentService.Model.Entities;

namespace PaymentService.Services
{
    public interface IPaymentService
    {
        Task AddPayment(Payment payment);
        Task DeletePaymentById(Guid id);
        Task<Payment> GetPaymentById(Guid id);
        Task<List<Payment>> GetAllPaymentsForUser(Guid userId);
        Task UpdatePayment(Payment payment);
    }
}
