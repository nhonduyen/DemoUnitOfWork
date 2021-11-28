using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Management.API.Services;
using Management.Domain.Departments;

namespace Management.API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class DepartmentController : Controller
    {
        private readonly DepartmentService _departmentService;
        public DepartmentController(DepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var result = await _departmentService.AddAllEntityAsync();
            return Ok(result);
        }

        [HttpGet]
        public ActionResult GetDepartmentById(Guid id)
        {
            var result = _departmentService.GetDepartmentById(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> AddDepartment([FromBody] Department department)
        {
            var result = await _departmentService.AddDepartmentAsync(department);
            return Ok(result);
        }
    }
}
