using CallGate.ApiModels.User;
using CallGate.Filters;
using CallGate.Filters.NotReusable;
using CallGate.Services.Events;
using CallGate.Services.Group;
using CallGate.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CallGate.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class EventController : Controller
    {
        private readonly IEventService _eventService;

        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpGet("authorized")]
        public IActionResult GetAllAuthorizedUserEvents()
        {
            var events = _eventService.GetAllForAuthorizedUser();

            return Json(events);
        }
    }
}