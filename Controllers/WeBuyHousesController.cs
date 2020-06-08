using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WeBuyHouses.Models;
using WeBuyHouses.Services.Interfaces;

namespace WeBuyHouses.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeBuyHousesController : ControllerBase
    {
        private readonly ILogger<WeBuyHousesController> _logger;
        private readonly ITwilioService _twilioMessageService;

        public WeBuyHousesController(ILogger<WeBuyHousesController> logger, ITwilioService twilioMessageService)
        {
            _logger = logger;            
            _twilioMessageService = twilioMessageService;
        }

        [HttpPost]
        [Route("v1/SendMessage")]
        public async Task<IActionResult> SendMessage([FromBody] Customer customer)
        {
            _logger.LogInformation($"Received Customer Information: {customer}");
            var result = await _twilioMessageService.SendMessage(customer);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
    }
}