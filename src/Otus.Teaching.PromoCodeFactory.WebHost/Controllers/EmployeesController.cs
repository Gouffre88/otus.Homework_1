using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain.Administration;
using Otus.Teaching.PromoCodeFactory.WebHost.Models;

namespace Otus.Teaching.PromoCodeFactory.WebHost.Controllers
{
    /// <summary>
    /// Сотрудники
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EmployeesController
        : ControllerBase
    {
        private readonly IRepository<Employee> _employeeRepository;

        public EmployeesController(IRepository<Employee> employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        /// <summary>
        /// Получить данные всех сотрудников
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<EmployeeShortResponse>> GetEmployeesAsync()
        {
            var employees = await _employeeRepository.GetAllAsync();

            var employeesModelList = employees.Select(x =>
                new EmployeeShortResponse()
                {
                    Id = x.Id,
                    Email = x.Email,
                    FullName = x.FullName,
                }).ToList();

            return employeesModelList;
        }

        /// <summary>
        /// Получить данные сотрудника по Id
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<EmployeeResponse>> GetEmployeeByIdAsync(Guid id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);

            if (employee == null)
                return NotFound();

            var employeeModel = new EmployeeResponse()
            {
                Id = employee.Id,
                Email = employee.Email,
                Roles = employee.Roles.Select(x => new RoleItemResponse()
                {
                    Name = x.Name,
                    Description = x.Description
                }).ToList(),
                FullName = employee.FullName,
                AppliedPromocodesCount = employee.AppliedPromocodesCount
            };

            return employeeModel;
        }
        /// <summary>
        /// Добавить данные сотрудника 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<CreateUser>> CreateUser(CreateUser model)
        {
            model.Id = Guid.NewGuid();
            var user = new Employee()
            {
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Roles = model.Roles.Select(x => new Role()
                {
                    Name = x.Name,
                    Description = x.Description
                }).ToList()??new List<Role>()
            };

            var newUser = await _employeeRepository.CreateAsync(user);
            var userModel = new EmployeeResponse()
            {
                Id = newUser.Id,
                Email = newUser.Email,
                Roles = newUser.Roles.Select(x => new RoleItemResponse()
                {
                    Name = x.Name,
                    Description = x.Description
                }).ToList(),
                FullName = newUser.FullName,
                AppliedPromocodesCount = newUser.AppliedPromocodesCount 
            };
            return Ok(userModel);
        }

        /// <summary>
        /// Обновить данные сотрудника 
        /// </summary>
        /// <returns></returns>
        [HttpPut("update/{id}")]
        public async Task<ActionResult> UpdateUser(Guid id, CreateUser model)
         {
            var user = await _employeeRepository.GetByIdAsync(id);
            if (user == null) return BadRequest();
            user.Email = model.Email;
            user.Roles = model.Roles.Select(x => new Role()
            {
                Name = x.Name,
                Description = x.Description,
                Id = x.Id
            }).ToList();
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            var response = await _employeeRepository.UpdateAsync(user);
            return Ok(response);
        }

        /// <summary>
        /// Удалить данные сотрудника 
        /// </summary>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteUser(Guid id)
        {
            var user = await _employeeRepository.GetAllAsync();
            var us = user.FirstOrDefault(x => x.Id == id);
            if (us == null) return BadRequest();
            await _employeeRepository.DeleteAsync(us);
            return Ok();
        }
    }
}