﻿using eRentSolution.Application.System.Roles;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eRentSolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }
        public async Task<IActionResult> GetAll()
        {
            var result = await _roleService.GetAll();
            if (result == null)
                return BadRequest();
            return Ok(result);
        }
    }
}
