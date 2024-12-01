using HotelService.Application.AuthorizedPerson.Command;
using HotelService.Application.ContactInfo;
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
    public class HotelController : ControllerBase
    {
        private readonly IMediator _mediator;
        public HotelController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> GetHotels()
        {
            var query = new GetAllHotelsQuery();
            var hotels = await _mediator.Send(query);
            return Ok(hotels);
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetHotelById (Guid id)
        {
            var query = new GetHotelByIdQuery(id);
            var hotel = await _mediator.Send(query);
            if (hotel == null)
                return NotFound();
            
            return Ok(hotel);
        }
        
        [HttpGet("location/{location}")]
        public async Task<IActionResult> GetHotelByLocation([FromRoute] string location)
        {
            if (string.IsNullOrWhiteSpace(location))
                return BadRequest("Location parameter cannot be null or empty.");

            var query = new GetHotelByLocationQuery(location);
            var hotel = await _mediator.Send(query);

            if (hotel == null)
                return NotFound(new { Message = "No hotel found for the specified location." });

            return Ok(hotel);
        }


        [HttpPost]
        public async Task<IActionResult>Hotel([FromBody] CreateHotelCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Hotel (Guid id, [FromBody] UpdateHotelCommand command)
        {
            command.Id = id;

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Hotel(Guid id)
        {
            var command = new DeleteHotelCommand(id);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("authorized-person")]
        public async Task<IActionResult> AuthorizedPerson([FromBody]CreateAuthorizedPersonCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        
        [HttpDelete("authorized-person/{id}")]
        public async Task<IActionResult> DeleteAuthorizedPerson(Guid id)
        {
            var command = new DeleteAuthorizedPersonCommand(id);
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        
        [HttpPost ("contact-info")]
        public async Task<IActionResult> CreateContactInfo([FromBody]CreateContactInfoCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        
        [HttpDelete("contact-info/{id}")]
        public async Task<IActionResult> DeleteContactInfo(Guid id)
        {
            var command = new DeleteContactInfoCommand(id);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("{id}/contact-info")]
        public async Task<IActionResult> GetContactInfoByHotelId(Guid id)
        {
            var result =await _mediator.Send(new GetContactInfoByHotelIdQuery(id));
            return Ok(result);
        }
    }
}
