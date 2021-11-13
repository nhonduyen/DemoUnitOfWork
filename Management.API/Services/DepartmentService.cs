using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Management.Domain.Interfaces;
using Management.Infrastructure.Repositories;
using Management.Domain.Departments;
using Management.Domain.Salaries;
using Management.Domain.Users;

namespace Management.API.Services
{
    public class DepartmentService
    {
        private readonly IAppUnitOfWork _unitOfWork;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IUserRepository _userRepository;
        private readonly ISalaryRepository _salaryRepository;
        private readonly CryptoService _cryptoService;

        public DepartmentService(IAppUnitOfWork unitOfWork, IDepartmentRepository departmentRepository, IUserRepository userRepository, ISalaryRepository salaryRepository, CryptoService cryptoService)
        {
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _salaryRepository = salaryRepository;
            _departmentRepository = departmentRepository;
            _cryptoService = cryptoService;
        }

        public async Task<int> AddAllEntityAsync()
        {
            var departmentName = $"department_{Guid.NewGuid():N}";
            var department = _departmentRepository.AddDepartment(departmentName);

            var username = $"user_{Guid.NewGuid():N}";
            var password = _cryptoService.ComputeSha256Hash(username);
            var email = $"{Guid.NewGuid():N}@gmail.com";
            var user = _userRepository.NewUser(username, email, department, password);

            decimal coefficientSalary = new Random().Next(1, 15);
            decimal workdays = 22;
            var salary = _salaryRepository.AddUserSalary(user, coefficientSalary, workdays);

            var result = await _unitOfWork.CommitAsync();

            return result;
        }
    }
}
