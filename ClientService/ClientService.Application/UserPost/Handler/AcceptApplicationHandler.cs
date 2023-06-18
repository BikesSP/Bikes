using ClientService.Application.Common.Enums;
using ClientService.Application.Common.Extensions;
using ClientService.Application.Services.CurrentUserService;
using ClientService.Application.UserPost.Command;
using ClientService.Domain.Common;
using ClientService.Domain.Entities;
using ClientService.Domain.Wrappers;
using ClientService.Infrastructure.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientService.Application.UserPost.Handler
{
    public class AcceptApplicationHandler: IRequestHandler<AcceptApplicationRequest, Response<bool>>
    {
        private readonly ILogger<AcceptApplicationHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public AcceptApplicationHandler(
            ILogger<AcceptApplicationHandler> logger, IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<Response<bool>> Handle(AcceptApplicationRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var postQuery = await _unitOfWork.PostRepository.GetAsync(expression: x => x.Id == request.PostId, includeFunc: x => x.Include(post => post.Applier).Include(post => post.Author), disableTracking: false);
                var post = postQuery.FirstOrDefault();

                if (post?.Status != PostStatus.Created)
                {
                    return new Response<bool>(code: (int)ResponseCode.PostErrorNotFound, message: ResponseCode.PostErrorNotFound.GetDescription());
                }

                var user = await _currentUserService.GetCurrentAccount();
                if(post.AuthorId == user.Id)
                {
                    return new Response<bool>(code: (int)ResponseCode.Failed, message: ResponseCode.Failed.GetDescription());
                }

                var acceptedApplierQuery = await _unitOfWork.AccountRepository.GetAsync(x => x.Id.ToString() == request.ApplierId);
                var acceptedApplier = acceptedApplierQuery.FirstOrDefault();
                if (post.Applier == null)
                {
                    post.Applier = new List<Account>();
                }

                if(acceptedApplier == null || post.Applier.All(x => x.Id.ToString() != request.ApplierId))
                {
                    return new Response<bool>(code: (int)ResponseCode.PostErrorNotExistApplier, message: ResponseCode.PostErrorNotExistApplier.GetDescription());
                }

                Account grabber;
                Account passenger;

                if(post.TripRole == Role.Grabber)
                {
                    grabber = post.Author;
                    passenger = acceptedApplier;
                } else
                {
                    passenger = post.Author;
                    grabber = acceptedApplier;
                }

                Trip trip = new Trip()
                {
                    GrabberId = grabber.Id,
                    PassengerId = passenger.Id,
                    TripStatus = TripStatus.Created,
                    StartAt = post.StartTime,
                    Description = post.Description,
                    Post = post,
                    StartStationId = post.StartStationId,
                    EndStationId = post.EndStationId,
                };

                await _unitOfWork.TripRepository.AddAsync(trip);


                //TODO: schedule reminder to remind coming trip?

                post.Status = PostStatus.Completed;
                await _unitOfWork.PostRepository.UpdateAsync(post);

                var res = await _unitOfWork.SaveChangesAsync();

                return res > 0 ? new Response<bool>(code: 0, data: true) : new Response<bool>(code: (int)ResponseCode.Failed, message: ResponseCode.Failed.GetDescription());
            }
            catch (Exception ex)
            {
                return new Response<bool>(code: (int)ResponseCode.Failed, message: ResponseCode.Failed.GetDescription());
            }
            finally
            {
            }
        }
    }
}
