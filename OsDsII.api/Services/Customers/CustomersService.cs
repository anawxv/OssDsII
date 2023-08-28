using OsDsII.Repositories;
using OsDsII.DAL;
using OsDsII.Models;

namespace OsDsII.Services
{
    public class CustomersService : ICustomersService
    {
        private readonly ICustomersRepository _customersRepository;
        private readonly IUnitOfWork _unitOfWork;
        public CustomersService(ICustomersRepository customersRepository, IUnitOfWork unitOfWork)
        {
            _customersRepository = customersRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            return await _customersRepository.GetAllAsync();
        }

        public async Task<Customer> GetByIdAsync(int id)
        {
            return await _customersRepository.GetByIdAsync(id) ?? throw new Exception("Customer not found");
        }

        public async Task<Customer> CreateCustomerAsync(Customer customer)
        {
            Customer customerExists = await _customersRepository.FindUserByEmailAsync(customer.Email);

            if (customerExists != null && !customerExists.Equals(customer))
            {
                throw new Exception("Customer already exists");
            }

            await _customersRepository.AddCustomerAsync(customer);
            await _unitOfWork.SaveChangesAsync();
            return customer;
        }

        public async Task DeleteCustomerAsync(int id)
        {
            Customer customerExists = await _customersRepository.GetByIdAsync(id);

            if (customerExists is null)
            {
                throw new Exception("Customer not found");
            }

            await _customersRepository?.DeleteCustomer(customerExists);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateCustomerAsync(int id, Customer customer)
        {
            Customer customerExists = await GetByIdAsync(id);
            if (customerExists is null)
            {
                throw new Exception("Customer not found");
            }
            customerExists.Email = customer.Email;
            customerExists.Name = customer.Name;
            customerExists.Phone = customer.Phone;
            await _customersRepository.UpdateCustomerAsync(customer);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}