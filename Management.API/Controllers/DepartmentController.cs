using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Management.API.Services;

namespace Management.API.Controllers
{
    [ApiController]
    [Route("departments")]
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
            await _departmentService.AddAllEntityAsync();
            return Ok();
        }
    }
}
