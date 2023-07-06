using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientService.Application.Common.Constants
{
    public class NotificationConstant
    {
        public class Title
        {
            public static string VEHICLE_REGISTRATION_CREATE = "Vehicle registration";
        public static string VEHICLE_REGISTRATION_APPROVE = "Your vehicle registration has been approved";
        public static string VEHICLE_REGISTRATION_DENIED = "Your vehicle registration has been denied";
        public static string VEHICLE_UPDATE = "Vehicle Update";
        public static string POST_NEW_APPLICATION = "Post application";
        public static string POST_ACCEPT_APPLICATION = "Accept post application";
        public static string POST_REJECT_APPLICATION = "Reject post application";
        public static string POST_DELETED = "Post deleted";
        public static string POST_EXPIRED = "Post expired";
        public static string IN_COMING_TRIP = "Incoming trip";
        public static string TRIP_CANCELED = "Trip canceled";
        public static string TRIP_STARTED = "Trip started";
        public static string TRIP_FINISHED = "Trip finished";
        public static string TRIP_FEEDBACK = "Trip feedback";
    }

        public class Body
        {
            public static string VEHICLE_REGISTRATION_CREATE = "User {0} has registered their vehicle";
        public static string VEHICLE_REGISTRATION_APPROVE = "Your vehicle {0} has been approved";
        public static string VEHICLE_REGISTRATION_DENIED = "Your vehicle {0}} has been denied";
        public static string VEHICLE_UPDATE = "User {0} has updated their vehicle";
        public static string POST_NEW_APPLICATION = "{0} had applied to your post";
        public static string POST_ACCEPT_APPLICATION = "{0} accepted your application to his/her post. Let's track your in-coming trip";
        public static string POST_REJECT_APPLICATION = "{0} rejected your application to his/her post";
        public static string POST_DELETED = "{0} deleted his/her post, which you applied before";
        public static string POST_EXPIRED = "Your post has been deleted because of expiration";
        public static string IN_COMING_TRIP = "Your trip with {0} from {1} to {2} is going to start in {3} minutes";
        public static string TRIP_CANCELED = "{0} canceled his/her trip with you from {1} to {2}";
        public static string TRIP_STARTED = "{0} has started his/her trip with you from {1} to {2}. Let's track your on-going trip";
        public static string TRIP_FINISHED = "{0} finished his/her trip with you from {1} to {2}";
        public static string TRIP_FEEDBACK = "{0} feedback his/her trip with you from {1} to {2}";
    }
    }

}
