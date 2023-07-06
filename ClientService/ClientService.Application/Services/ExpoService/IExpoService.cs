using ClientService.Domain.Common.Enums.Notification;
using Expo.Server.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientService.Application.Services.ExpoService
{
    public interface IExpoService
    {

        public void sendList(List<string> tokens, Notification notification);
        public void sendTo(string token, Notification notification);
    }

    public class Notification
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public NotificationAction Action { get; set; }
        public string ReferenceId { get; set; }
    }
}
