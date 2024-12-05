using App.Core.Results;
using Microsoft.AspNetCore.Mvc;

namespace HotelService.Api.Controllers
{
    public class BaseController : ControllerBase
    {
        protected IActionResult HandleResponse<T>(Result<T> response)
        {
            if (response.IsSuccessful) return Ok(response.Data);
            return BadRequest(response.Message);
        }
    }

    public class Response<T>
    {
        public bool IsSuccessful { get; set; }
        public T Data { get; set; }
        public string Message { get; set; }
    }
}