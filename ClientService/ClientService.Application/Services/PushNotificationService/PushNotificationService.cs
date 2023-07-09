using ClientService.Application.Services.ExpoService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientService.Application.Services.PushNotificationService
{
    public class PushNotificationService : IPushNotificationService
    {
        public bool sendAdmin(Notification notification)
        {
            throw new NotImplementedException();
        }

        public bool sendPublic(Notification notificationDto)
        {
            throw new NotImplementedException();
        }

        public bool sendTo(string accountId, Notification notification)
        {
            throw new NotImplementedException();
        }

        public bool sendToList(List<string> accountIds, Notification notification)
        {
            throw new NotImplementedException();
        }
    }
}
