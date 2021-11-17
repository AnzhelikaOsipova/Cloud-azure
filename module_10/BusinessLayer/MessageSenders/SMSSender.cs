using Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace BusinessLayer.MessageSenders
{
    public class SMSSender : ISMSSender
    {
        public PhoneNumber PhoneNumberOfSender { get; }

        public SMSSender(PhoneNumber phoneNumberOfSender)
        {
            PhoneNumberOfSender = phoneNumberOfSender;
        }

        public void Send(PhoneNumber phoneNumber, string message)
        {
            try
            {
                string accountSid = Environment.GetEnvironmentVariable("TWILIO_ACCOUNT_SID");
                string authToken = Environment.GetEnvironmentVariable("TWILIO_AUTH_TOKEN");

                TwilioClient.Init(accountSid, authToken);

                var phoneMessage = MessageResource.Create(
                    body: message,
                    from: new Twilio.Types.PhoneNumber(PhoneNumberOfSender.CorrectPhoneNumber),
                    to: new Twilio.Types.PhoneNumber(phoneNumber.CorrectPhoneNumber)
                );
            }
            catch(Exception e)
            {
                return;
            }
        }
    }
}
