using Microsoft.EntityFrameworkCore;
using PaymentService.Model.Entities;

namespace PaymentService.DataLayer
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly PaymentDbContext context;

        public PaymentRepository(PaymentDbContext context)
        {
            this.context = context;
        }

        public async Task Add(Payment payment)
        {
            await context.Set<Payment>().AddAsync(payment);
            await context.SaveChangesAsync();
        }

        public async Task DeleteById(Guid id)
        {
            var payment = await context.Set<Payment>().FindAsync(id);
            if (payment != null)
            {
                context.Set<Payment>().Remove(payment);

                await context.SaveChangesAsync();
            }
        }

        public async Task<List<Payment>> GetAllPaymentsForUser(Guid userId)
        {
            return await context.Set<Payment>()
                .Where(p => p.UserId == userId)
                .ToListAsync();
        }

        public async Task<Payment> GetById(Guid id)
        {
            return await context.Set<Payment>().FindAsync(id) ?? new Payment();
        }

        public async Task Update(Payment payment)
        {
            this.context.Entry<Payment>(payment).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }
    }
}
