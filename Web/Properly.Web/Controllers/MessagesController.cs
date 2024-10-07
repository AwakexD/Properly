using Microsoft.AspNetCore.Mvc;
using Properly.Web.ViewModels.Messages;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Properly.Data.Models.User;
using Properly.Services.Data.Contracts;

namespace Properly.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class MessagesController : BaseController
    {
        private readonly IMessagesService messagesService;
        private readonly UserManager<ApplicationUser> userManager;

        public MessagesController(IMessagesService messagesService, UserManager<ApplicationUser> userManager)
        {
            this.messagesService = messagesService;
            this.userManager = userManager;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SendMessage([FromBody] MessageRequest messageRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var user = await this.userManager.GetUserAsync(this.User);

                if (string.IsNullOrEmpty(user.Id))
                {
                    return Unauthorized("User is not authenticated.");
                }

                await messagesService.CreateMessageAsync(user.Id, messageRequest);

                return Ok(new { success = true, message = "Message sent successfully!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }
    }
}
