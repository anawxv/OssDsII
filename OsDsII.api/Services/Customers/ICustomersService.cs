using OsDsII.Models;

namespace OsDsII.Services
{
    public interface ICustomersService
    {
        public Task<IEnumerable<Customer>> GetAllAsync();
        public Task<Customer> GetByIdAsync(int id);
        public Task<Customer> CreateCustomerAsync(Customer customer);
        public Task DeleteCustomerAsync(int id);
        public Task UpdateCustomerAsync(int id, Customer customer);
    }
}