using AutoMapper;
using Epc.API.Helpers;
using Epc.API.Helpers.Paging;
using Epc.API.Models;
using Epc.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Epc.API.Controllers
{
    [Route("api/v1/users")]
    [Authorize]
    public class UsersController : Controller
    {

        #region Private Fields

        private readonly IEpcRepository _epcRepository;

        private readonly IUrlHelper _urlHelper;

        #endregion

        #region Constructor

        public UsersController(
            IEpcRepository epcRepository,
            IUrlHelper urlHelper)
        {
            _epcRepository = epcRepository;
            _urlHelper = urlHelper;
        }

        #endregion


        #region Private Methods

        private string CreateUsersResourceUri(
            UsersResourceParameters usersResourceParameters,
            ResourceUriType type)
        {

            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return _urlHelper.Link("GetUsers",
                        new
                        {
                            orderBy = usersResourceParameters.OrderBy,
                            searchQuery = usersResourceParameters.SearchQuery,
                            Type = usersResourceParameters.Type,
                            pageNumber = usersResourceParameters.PageNumber - 1,
                            pageSize = usersResourceParameters.PageSize
                        });
                case ResourceUriType.NextPage:
                    return _urlHelper.Link("GetUsers",
                        new
                        {
                            orderBy = usersResourceParameters.OrderBy,
                            searchQuery = usersResourceParameters.SearchQuery,
                            Type = usersResourceParameters.Type,
                            pageNumber = usersResourceParameters.PageNumber + 1,
                            pageSize = usersResourceParameters.PageSize
                        });

                default:
                    throw new NotImplementedException();
            }
        }

        #endregion

        #region Public Actions


        [HttpGet(Name = "GetUsers")]
        [Authorize(Roles="Administrator")]
        public IActionResult GetUsers(UsersResourceParameters usersResourceParameters)
        {
            if (!usersResourceParameters.IsOrderByValid())
            {
                return BadRequest();
            }

            var usersFromRepo = _epcRepository.GetUsers(usersResourceParameters);

            Response.Headers.Add(
                "X-Pagination", 
                usersFromRepo.GetPaginationHeaderJson(
                    CreateUsersResourceUri(usersResourceParameters, ResourceUriType.PreviousPage),
                    CreateUsersResourceUri(usersResourceParameters, ResourceUriType.NextPage)));

            var users = Mapper.Map<IEnumerable<UserDto>>(usersFromRepo);

            

            return Ok(users);
        }

        #endregion

    }
}