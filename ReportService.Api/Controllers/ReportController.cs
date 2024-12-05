using MediatR;
using Microsoft.AspNetCore.Mvc;
using ReportService.Application.Report.Command;
using ReportService.Application.Report.Query;

namespace ReportService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportController(IMediator mediator) : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> GetReports()
        {
            var response = await mediator.Send(new GetAllReportsQuery());
            return HandleResponse(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetReportsById(Guid id)
        {
            var response = await mediator.Send(new GetReportsByIdQuery(id));
            return HandleResponse(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateReport([FromBody] CreateReportCommand command)
        {
            var response = await mediator.Send(command);
            return HandleResponse(response);
        }
    }
}