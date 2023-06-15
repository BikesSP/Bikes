using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientService.Application.Common.Enums
{
    public enum ResponseCode
    {
        [Description("Success")]
        Success = 0,
        [Description("Failed")]
        Failed = 1,
        [Description("Common Error")]
        CommonError = 2,
        [Description("Invalid param")]
        InvalidParam = 3,
        [Description("Invalid session")]
        InvalidSession = 4,
        [Description("Unhandled request")]
        UnhandledRequest = 5,
        [Description("Error when calling third party")]
        ThirdPartyError = 6,
        [Description("Error when processing JSON")]
        JsonProcessingError = 7,
        [Description("Invalid response code")]
        ResponseCodeInvalid = 8,
        [Description("Key is conflict")]
        Conflict = 9,

        // Auth
        [Description("Invalid username or password")]
        InvalidUsernameOrPassword = 10,
        [Description("Error occurs when login with Google")]
        GoogleAuthError = 11,
        [Description("Invalid refresh token")]
        RefreshTokenInvalid = 12,
        [Description("Authentication failed")]
        AuthenticationFailed = 13,
        [Description("Authentication failed: Outside email")]
        AuthenticationFailedOutsideEmail = 14,
        [Description("Invalid code")]
        CodeInvalid = 15,

        [Description("Unauthorized")]
        UnauthorizedRequest = 11,

        // POST
        [Description("Start time must be greater than now")]
        PostErrorInvalidTime = 20,
        [Description("Conflict time, There are one or more active post(s) at this start time (threshold: 30 minutes)")]
        PostErrorConflictTime = 22,
        [Description("Invalid station. Start station and end station must be different")]
        PostErrorInvalidStation = 23,
        [Description("Station not found")]
        PostErrorStationNotFound = 24,
        [Description("Post not found or closed")]
        PostErrorNotFound = 25,
        [Description("Can't update post. Post is currently being applied")]
        PostErrorInvalidStatus = 26,
        [Description("Can't self apply for your post")]
        PostErrorSelfApply = 27,
        [Description("Existed applier")]
        PostErrorExistedApplier = 28,
        [Description("Not existed applier")]
        PostErrorNotExistApplier = 29,
        [Description("End station can't be accessed from start station")]
        PostErrorInvalidEndStation = 30,

        // User
        [Description("Account has not registered the vehicle yet")]
        PostErrorUnregisteredVehicle = 31,
        [Description("Account has not updated information yet")]
        PostErrorUnupdatedAccount = 32,

        
        [Description("User not found")]
        UserNotFound = 30,
        [Description("User missing required field")]
        UserMissingField = 31,
        [Description("User has registered vehicle")]
        UserHasRegisteredVehicle = 32,

        // Trip
        [Description("Trip not found")]
        TripErrorNotFound = 40,
        [Description("Trip's current status is invalid for this action")]
        TripErrorInvalidStatus = 41,
        [Description("Current logged in user has no access on the trip in this action")]
        TripErrorInvalidAccess = 42,
        [Description("User is busy. There is an ongoing trip")]
        TripErrorOngoingTrip = 43,
        [Description("Existed feedback")]
        TripErrorExistedFeedback = 44,
        [Description("You must cancel the trip at least 30 minutes before the start time")]
        TripErrorCannotCancelTrip = 45,

        //Station
        [Description("Station is not found")]
        StationErrorNotFound = 51,
        [Description("Station is used")]
        StationErrorIsUsed = 52,
        [Description("Station is inactive")]
        StationErrorIsInactive = 53,

        [Description("Vehicle is not found")]
        VehicleErrorNotFound = 61,

        //Account
        [Description("Account is not found")]
        AccountErrorNotFound = 71,
        [Description("Account is inactive")]
        AccountErrorInactive = 72,

        //Accoun
        [Description("Notification is not found")]
        NotificationErrorNotFound = 81,

        [Description("Validation Error")] ErrorValidation = 90,
        [Description("Unauthorized")] Unauthorized = 91,
        [Description("Google Id Token is invalid")]AuthErrorInvalidGoogleIdToken = 92,
        [Description("Refresh Token is invalid")] AuthErrorInvalidRefreshToken = 93,

    }
}
