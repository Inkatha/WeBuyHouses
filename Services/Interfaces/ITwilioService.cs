using System.Threading.Tasks;
using Twilio.AspNet.Common;
using Twilio.Rest.Api.V2010.Account;
using Twilio.TwiML;
using WeBuyHouses.Models;

namespace WeBuyHouses.Services.Interfaces
{
    public interface ITwilioService
    {
        Task<MessageResource> SendMessage(Customer customer);
        Task<MessagingResponse> HandleWebhook(SmsRequest incomingMessage);
    }
}