using System.Net;
using ClientService.Application.Common.Enums;
using ClientService.Application.Common.Exceptions;
using ClientService.Application.Common.Extensions;
using FluentValidation;

namespace ClientService.Application.Common.Models.Response;

public class ExceptionResponse
{
    public ExceptionResponse()
    {
        Code = (int)ResponseCode.CommonError;
        Error = ResponseCode.CommonError.ToString();
        Message = ResponseCode.CommonError.GetDescription();
    }

    public ExceptionResponse(ApiException apiException)
    {
        Code = apiException.ErrorCode;
        Error = apiException.Error;
        Message = apiException.ErrorMessage;
    }

    public ExceptionResponse(Exception exception)
    {
        Code = (int)HttpStatusCode.InternalServerError;
        Error = HttpStatusCode.InternalServerError.ToString();
        Message = exception.Message;
    }

    public ExceptionResponse(ValidationException exception)
    {
        Code = (int)ResponseCode.ErrorValidation;
        Error = ResponseCode.ErrorValidation.GetDescription();
        Message = exception.Message;
        Details ??= new Dictionary<string, List<string>>();

        foreach (var error in exception.Errors)
            if (Details.TryGetValue(error.PropertyName, out var value))
                value.Add(error.ErrorMessage);
            else
                Details.Add(error.PropertyName, new List<string> { error.ErrorMessage });
    }

    public int Code { get; }

    public string Error { get; }

    public string Message { get; }

    public Dictionary<string, List<string>>? Details { get; set; }
}
