using MediatR;
using Microsoft.AspNetCore.Mvc;
using ReportService.Application.Report.Command;
using ReportService.Application.Report.Query;

namespace ReportService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReportController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        
        [HttpGet]
        public async Task<IActionResult> GetReports()
        {
            var query = new GetAllReportsQuery();
            var reports = await _mediator.Send(query);
            return Ok(reports);
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetReportsById (Guid id)
        {
            var query = new GetReportsByIdQuery(id);
            var report = await _mediator.Send(query);
            if (report == null)
                return NotFound();
            
            return Ok(report);
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateReport ([FromBody] CreateReportCommand report)
        {
            var command = await _mediator.Send(report);
            return Ok(report);
        }
    }
}
