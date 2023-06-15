using Amazon.Runtime.Internal;
using ClientService.Application.Common.Enums;
using ClientService.Application.Common.Extensions;
using ClientService.Application.Services.CurrentUserService;
using ClientService.Application.User.Command;
using ClientService.Application.User.Model;
using ClientService.Domain.Wrappers;
using ClientService.Infrastructure.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientService.Application.User.Handler
{
    public class UpdateUserProfileHandler : IRequestHandler<UpdateUserProfileRequest, Response<UserProfileResponse?>>
    {
        private readonly ILogger<UpdateUserProfileHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public UpdateUserProfileHandler(
            ILogger<UpdateUserProfileHandler> logger, IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }
        public async Task<Response<UserProfileResponse?>> Handle(UpdateUserProfileRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _currentUserService.GetCurrentAccount();
                if (user == null)
                {
                    return new Response<UserProfileResponse?>(code: (int)ResponseCode.Failed, message: ResponseCode.Failed.GetDescription());
                }

                user.AvartarUlr = request.Avatar;
                user.Name = request.Name;
                user.Phone = request.Phone;
                user.Card = request.Card;
                user.IsUpdated = true;

                await _unitOfWork.AccountRepository.UpdateAsync(user);
                await _unitOfWork.SaveChangesAsync();
                return new Response<UserProfileResponse?>(code: 0,
                    data: new UserProfileResponse()
                    {
                        Avatar = user.AvartarUlr,
                        AveragePoint = 0,
                        Email = user.Email,
                        Id = user.Id.ToString(),
                        IsUpdated = user.IsUpdated,
                        Name = user.Name,
                        Phone = user.Phone
                    }
                    );
            }
            catch (Exception ex)
            {
                return new Response<UserProfileResponse?>(code: (int)ResponseCode.Failed, message: ResponseCode.Failed.GetDescription());
            }
            finally
            {
            }
        }
    }
}
