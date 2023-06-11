using MediatR;
using Microsoft.AspNetCore.Mvc;
using static Google.Apis.Requests.BatchRequest;
using System.Net;
using ClientService.Domain.Wrappers;

namespace ClientService.API.Controllers
{
    public class ApiControllerBase : ControllerBase
    {
        /*private ISender? _mediator;

        protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();*/
        protected readonly IMediator mediator;
        private readonly ILogger logger;

        protected ApiControllerBase(
            IMediator mediator,
            ILogger logger)
        {
            this.mediator = mediator;
            this.logger = logger;
        }

        public IActionResult Ok<T>(Response<T?> result)
        {
            string? message = result?.Message;
            Dictionary<string, string?>? errors = new();

            return StatusCode((int)HttpStatusCode.OK, new Response<T?>()
            {
                Data = result.Data,
                Code = result.Code,
                Message = message
            });
        }

        public IActionResult Ok<T>(PaginationResponse<T?> result)
        {
            string? message = result?.Message;
            Dictionary<string, string?>? errors = new();

            return StatusCode((int)HttpStatusCode.OK, new PaginationResponse<T?>()
            {
                Data = result.Data,
                Code = result.Code,
                Message = message,
            });
        }
    }

}
