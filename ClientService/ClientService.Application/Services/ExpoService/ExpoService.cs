using Expo.Server.Client;
using Expo.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientService.Application.Services.ExpoService
{
    public class ExpoService : IExpoService
    {
        private PushApiClient _client;

        public ExpoService()
        {
            _client = new PushApiClient();
        }

        public async void sendList(List<string> tokens, Notification notification)
        {
            var pushTicketReq = new PushTicketRequest()
            {
                PushTo = tokens,
                PushBody = notification.Body,
                PushTitle = notification.Title,
            };

            var result = await _client.PushSendAsync(pushTicketReq);
            if (result?.PushTicketErrors?.Count() > 0)
            {
                foreach (var error in result.PushTicketErrors)
                {
                    Console.WriteLine($"Error: {error.ErrorCode} - {error.ErrorMessage}");
                }
            }
        }

        public async void sendTo(string token, Notification notification)
        {
            if (token == null) return;
            var pushTicketReq = new PushTicketRequest()
            {
                PushTo = new List<string>() { token },
                PushBody = notification.Body,
                PushTitle = notification.Title,
            };

            var result = await _client.PushSendAsync(pushTicketReq);
            if (result?.PushTicketErrors?.Count() > 0)
            {
                foreach (var error in result.PushTicketErrors)
                {
                    Console.WriteLine($"Error: {error.ErrorCode} - {error.ErrorMessage}");
                }
            }
        }
    }
}
