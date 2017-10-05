using AutoMapper;
using Epc.API.Helpers.Paging;
using Epc.API.Models;
using Epc.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Epc.API.Controllers
{
    [Route("api/v1/users")]
    [Authorize]
    public class UsersController : Controller
    {

        #region Private Fields

        private readonly IEpcRepository _epcRepository;

        #endregion

        #region Constructor

        public UsersController(IEpcRepository epcRepository)
        {
            _epcRepository = epcRepository;
        }

        #endregion



        [HttpGet(Name = "GetUsers")]
        [Authorize(Roles="Administrator")]
        public IActionResult GetUsers(UsersResourceParameters usersResourceParameters)
        {
            if (!usersResourceParameters.IsOrderByValid())
            {
                return BadRequest();
            }

            var usersFromRepo = _epcRepository.GetUsers(usersResourceParameters);

            Response.Headers.Add("X-Pagination", usersFromRepo.GetPaginationHeaderJson());

            var users = Mapper.Map<IEnumerable<UserDto>>(usersFromRepo);

            

            return Ok(users);
        }

    }
}