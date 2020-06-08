using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Twilio.AspNet.Common;
using Twilio.AspNet.Core;
using Twilio.TwiML;
using WeBuyHouses.Models;
using WeBuyHouses.Services.Interfaces;
using WeBuyHouses.Shared;

namespace WeBuyHouses.Controllers
{
    [Route("[controller]")]
    public class WebhookController : TwilioController
    {
        private readonly ILogger<WebhookController> _logger;
        private readonly ITwilioService _messageService;
        public WebhookController(ILogger<WebhookController> logger, ITwilioService messageService)
        {
            _logger = logger;
            _messageService = messageService;
        }
        [HttpPost]
        public async Task<TwiMLResult> Index(SmsRequest incomingMessage)
        {
            var response = await _messageService.HandleWebhook(incomingMessage);
            return TwiML(response);
        }
    }
}