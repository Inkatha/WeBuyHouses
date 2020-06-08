
using System;
using System.Linq;
using System.Threading.Tasks;
using Twilio;
using Twilio.AspNet.Common;
using Twilio.Rest.Api.V2010.Account;
using Twilio.TwiML;
using WeBuyHouses.DataAccess;
using WeBuyHouses.Models;
using WeBuyHouses.Services.Interfaces;
using WeBuyHouses.Shared;

namespace WeBuyHouses.Services
{
    public class TwilioService : ITwilioService
    {
        readonly IDealFinderRepository _dealFinderRepository;
        readonly ICustomerRepository _customerRepository;
        readonly IMessageRepository _messageRepository;

        public TwilioService(IDealFinderRepository dealFinderRepository, ICustomerRepository customerRepository, IMessageRepository messageRepository)
        {
            _dealFinderRepository = dealFinderRepository;
            _customerRepository = customerRepository;
            _messageRepository = messageRepository;
        }
        public async Task<MessageResource> SendMessage(Customer customer)
        {
            if (string.IsNullOrWhiteSpace(customer.PhoneNumber))
            {
                return null;
            }
            // Find your Account Sid and Token at twilio.com/console
            // DANGER! This is insecure. See http://twil.io/secure
            const string accountSid = "AC6a6b5847e781ea61d82604a5cb21bb0f";
            const string authToken = "7cc7b6c5fe71b0a2d3ab9e02508144df";

            TwilioClient.Init(accountSid, authToken);

            var message = await MessageResource.CreateAsync(
                body: "Join Earth's mightiest heroes.",
                from: new Twilio.Types.PhoneNumber("19892828233"),
                to: new Twilio.Types.PhoneNumber(customer.PhoneNumber)
            );

            if (message.ErrorCode == null)
            {
                return message;
            }
            return null;
        }

        public async Task<MessagingResponse> HandleWebhook(SmsRequest incomingMessage)
        {
            var messagingResponse = new MessagingResponse();

            if (incomingMessage.Body.Length > 1000)
            {
                messagingResponse.Message("Sorry... Your message was too long to process.");
            }

            var customer = new Customer
            {
                PhoneNumber = incomingMessage.From,
                City = incomingMessage.FromCity,
                Zip = incomingMessage.FromZip,
                State = incomingMessage.FromState
            };

            var dealFinderCodes = await _dealFinderRepository.GetAllDealFinderCodes();

            if (incomingMessage.Body.Contains(Constants.DealFinderCodePrefix))
            {
                var incomingMessageWords = incomingMessage.Body
                    .Split(new[] {' ', '\t', '\n', '\r'}, StringSplitOptions.RemoveEmptyEntries);
                    
                var dealFinderCode = incomingMessageWords
                    .Where(x => dealFinderCodes.Contains(x, StringComparer.InvariantCultureIgnoreCase))
                    .FirstOrDefault();

                customer.DealFinderCode = !string.IsNullOrWhiteSpace(dealFinderCode) ? dealFinderCode : string.Empty;
            }

            StoreCustomerAndMessage(customer, incomingMessage);

            if (string.IsNullOrWhiteSpace(customer.DealFinderCode))
            {
                return messagingResponse.Message(Constants.DealFinderCodeNotFound);
            }

            var dealFinder = await _dealFinderRepository.GetDealFinder(customer.DealFinderCode);
            
            return dealFinder != null ? 
                SuccessfulMessage(messagingResponse, dealFinder) : 
                messagingResponse.Message(Constants.DealFinderCodeNotFound);
        }

        private MessagingResponse SuccessfulMessage(MessagingResponse messagingResponse, DealFinder dealFinder)
        {
            return messagingResponse.Message($@"
                    Deal Finder Code {dealFinder.DealFinderCode}
                    Deals Found {dealFinder.DealsFound}
                    Deals Closed {dealFinder.DealsClosed}");
        }

        private async void StoreCustomerAndMessage(Customer customer, SmsRequest incomingMessage)
        {
            await _customerRepository.CreateNewCustomer(customer);

            var message = new WeBuyHouses.Models.Message 
            {
                Body = incomingMessage.Body,
                FromPhoneNumber = incomingMessage.From.ToString(),
                ToPhoneNumber = incomingMessage.To.ToString()
            };

            await _messageRepository.CreateNewMessage(message);
        }
    }
}