﻿using Amazon.Runtime.Internal;
using ClientService.Application.Auth.Command;
using ClientService.Application.Auth.Model;
using ClientService.Domain.Wrappers;
using ClientService.Infrastructure.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientService.Application.Auth.Handler
{
    public class UpdateUserProfileHandler : IRequestHandler<UpdateUserProfileRequest, Response<UserProfileResponse?>>
    {
        private readonly ILogger<UpdateUserProfileHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateUserProfileHandler(
            ILogger<UpdateUserProfileHandler> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
        public async Task<Response<UserProfileResponse?>> Handle(UpdateUserProfileRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var user = _unitOfWork.AccountRepository.FirstOrDefault(account => account.Email == request.Email);
                if (user == null)
                {
                    return new Response<UserProfileResponse?>(code: -1, message: "Internal server error");
                }

                user.AvartarUlr = request.Avatar;
                user.Name= request.Name;
                user.Phone= request.Phone;
                user.Card= request.Card;
                user.IsUpdated= true;

                _unitOfWork.AccountRepository.Update(user);
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
                return new Response<UserProfileResponse?>(code: -1, message: "Internal server error");
            }
            finally
            {
            }
        }
    }
}
