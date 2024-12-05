using HotelService.Application.AuthorizedPerson.Command;
using HotelService.Application.ContactInfo.Command;
using HotelService.Application.ContactInfo.Query;
using HotelService.Application.Hotel.Command;
using HotelService.Application.Hotel.Query;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HotelService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HotelController(IMediator mediator) : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> GetHotels()
        {
            var response = await mediator.Send(new GetAllHotelsQuery());
            return HandleResponse(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetHotelById(Guid id)
        {
            var response = await mediator.Send(new GetHotelByIdQuery(id));
            return HandleResponse(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateHotel([FromBody] CreateHotelCommand command)
        {
            var response = await mediator.Send(command);
            return HandleResponse(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateHotel(Guid id, [FromBody] UpdateHotelCommand command)
        {
            command.Id = id;
            var response = await mediator.Send(command);
            return HandleResponse(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHotel(Guid id)
        {
            var response = await mediator.Send(new DeleteHotelCommand(id));
            return HandleResponse(response);
        }
        
        [HttpPost("authorized-person")]
        public async Task<IActionResult> AuthorizedPerson([FromBody]CreateAuthorizedPersonCommand command)
        {
            var response = await mediator.Send(command);
            return HandleResponse(response);
        }

        [HttpDelete("authorized-person/{id}")]
        public async Task<IActionResult> DeleteAuthorizedPerson(Guid id)
        {
            var command = new DeleteAuthorizedPersonCommand(id);
            var response = await mediator.Send(command);
            return HandleResponse(response);
        }

        [HttpPost ("contact-info")]
        public async Task<IActionResult> CreateContactInfo([FromBody]CreateContactInfoCommand command)
        {
            var response = await mediator.Send(command);
            return HandleResponse(response);
        }

        [HttpDelete("contact-info/{id}")]
        public async Task<IActionResult> DeleteContactInfo(Guid id)
        {
            var command = new DeleteContactInfoCommand(id);
            var response = await mediator.Send(command);
            return HandleResponse(response);
        }

        [HttpGet("{id}/contact-info")]
        public async Task<IActionResult> GetContactInfoByHotelId(Guid id)
        {
            var response = await mediator.Send(new GetContactInfoByHotelIdQuery(id));
            return HandleResponse(response);
        }
        
        
    }
}