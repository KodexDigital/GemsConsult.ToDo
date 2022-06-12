using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mime;
using Todo.Core.Common;
using Todo.Core.Dtos;
using Todo.Services.Declarations;

namespace Presentation.Controllers.VersionOne
{
    [ApiVersion("1.0")]
    [Produces(MediaTypeNames.Application.Json)]
    public class Auth02Controller : BaseController
    {
        private readonly IApplicationUserService appUserService;

        public Auth02Controller(IApplicationUserService appUserService)
        {
            this.appUserService = appUserService;
        }

        /// <summary>
        /// This is used for creating a new account
        /// </summary>
        /// <param name="request">Request payload</param>
        /// <returns>Expected success</returns>
        [HttpPost, Route("userRegistration")]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UserRegistration([FromBody] CreateNewUserDto request)
            => Ok(await appUserService.AddNewUser(request));

        /// <summary>
        /// Authentication and Authorization endpoint
        /// </summary>
        /// <param name="request">Request payload</param>
        /// <returns>Expected success</returns>
        [HttpPost, Route("securedLogin")]
        [ProducesResponseType(typeof(ResponseModel<AuthenticateResponseDto>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseModel<AuthenticateResponseDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Login([FromBody] AuthenticationRequestDto request)
            => Ok(await appUserService.LoginAsync(request));

        /// <summary>
        /// Endpoint to get all existing users
        /// </summary>
        /// <returns>List of all users</returns>
        [HttpGet, Route("allUsers")]
        [ProducesResponseType(typeof(ResponseModel<List<AllApplicationUsersDto>>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseModel<List<AllApplicationUsersDto>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> AllExistingUsers()
           => Ok(await appUserService.GetAllUsers());
    }
}
