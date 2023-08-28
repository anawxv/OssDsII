using OsDsII.Data;
using OsDsII.Models;
using Microsoft.EntityFrameworkCore;

namespace OsDsII.Repositories
{
    public class CustomersRepository : ICustomersRepository
    {
        private readonly DataContext _dataContext;
        public CustomersRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            return await _dataContext.Customers.ToListAsync();
        }

        public async Task<Customer> GetByIdAsync(int id)
        {
            return await _dataContext.Customers.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task AddCustomerAsync(Customer customer)
        {
            await _dataContext.Customers.AddAsync(customer);
        }

        public async Task DeleteCustomer(Customer customer)
        {
            _dataContext.Customers.Remove(customer);
        }

        public async Task UpdateCustomerAsync(Customer customer)
        {
            _dataContext.Update(customer);
        }

        public async Task<Customer> FindUserByEmailAsync(string email)
        {
            return await _dataContext.Customers.FirstOrDefaultAsync(c => c.Email == email);
        }
    }
}