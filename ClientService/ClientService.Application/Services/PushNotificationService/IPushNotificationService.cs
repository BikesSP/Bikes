using ClientService.Application.Services.ExpoService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ClientService.Application.Services.PushNotificationService
{
    public interface IPushNotificationService
    {
        public bool sendPublic(Notification notificationDto);

        public bool sendTo(string accountId, Notification notification);
        public bool sendAdmin(Notification notification);

        bool sendToList(List<string> accountIds, Notification notification);
    }
}
