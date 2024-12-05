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
    }
}