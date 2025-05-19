using PaymentService.Model.Entities;

namespace PaymentService.DataLayer
{
    public interface IPaymentRepository
    {
        Task Add(Payment payment);
        Task Update(Payment payment);
        Task DeleteById(Guid id);
        Task<Payment> GetById(Guid id);
        Task<List<Payment>> GetAllPaymentsForUser(Guid userId);
    }
}
