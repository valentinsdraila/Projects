using PaymentService.DataLayer;
using PaymentService.Model.Entities;

namespace PaymentService.Services
{
    public class PaymentsService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentsService(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public async Task AddPayment(Payment payment)
        {
            await _paymentRepository.Add(payment);
        }

        public async Task DeletePaymentById(Guid id)
        {
            await _paymentRepository.DeleteById(id);
        }

        public async Task<List<Payment>> GetAllPaymentsForUser(Guid userId)
        {
            return await _paymentRepository.GetAllPaymentsForUser(userId);
        }

        public async Task<Payment> GetPaymentById(Guid id)
        {
            return await _paymentRepository.GetById(id);
        }

        public async Task UpdatePayment(Payment payment)
        {
            await _paymentRepository.Update(payment);
        }

    }
}
