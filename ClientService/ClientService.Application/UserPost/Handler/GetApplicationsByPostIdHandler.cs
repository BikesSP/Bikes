using ClientService.Application.Common.Enums;
using ClientService.Application.Common.Extensions;
using ClientService.Application.Services.CurrentUserService;
using ClientService.Application.User.Model;
using ClientService.Application.UserPost.Command;
using ClientService.Application.UserPost.Model;
using ClientService.Domain.Wrappers;
using ClientService.Infrastructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientService.Application.UserPost.Handler
{
    public class GetApplicationsByPostIdHandler: IRequestHandler<GetApplicationsByPostIdRequest, PaginationResponse<UserProfileResponse>>
    {
        private readonly ILogger<GetApplicationsByPostIdHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public GetApplicationsByPostIdHandler(
            ILogger<GetApplicationsByPostIdHandler> logger, IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<PaginationResponse<UserProfileResponse>> Handle(GetApplicationsByPostIdRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var postQuery = await _unitOfWork.PostRepository.GetAsync(
                        expression: x => x.Id == request.getId(),
                        includeFunc: (query) => query.Include(x => x.Applier)
                    );

                

                var post = postQuery.FirstOrDefault();

                if(post == null)
                {
                    return new PaginationResponse<UserProfileResponse>(code: (int)ResponseCode.PostErrorNotFound, message: ResponseCode.PostErrorNotFound.GetDescription());
                }
                if(post.Applier == null)
                {
                    return new PaginationResponse<UserProfileResponse>(code: 0,
                        data: new PaginationData<UserProfileResponse>()
                        {
                            Page = request.PageNumber,
                            PageSize = request.PageSize,
                            TotalSize = 0,
                            TotalPage = 0,
                            Items = new List<UserProfileResponse>()
                        }
                    );
                }

                var finalAppliers = post.Applier.Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize).ToList();

                return new PaginationResponse<UserProfileResponse>(code: 0,
                        data: new PaginationData<UserProfileResponse>()
                        {
                            Page = request.PageNumber,
                            PageSize = request.PageSize,
                            TotalSize = post.Applier.Count(),
                            TotalPage = (int?)((post.Applier.Count() + (long)request.PageSize - 1) / (long)request.PageSize) ?? 0,
                            Items = finalAppliers.ConvertAll(user => new UserProfileResponse()
                            {
                                Avatar = user.AvartarUlr,
                                AveragePoint = 0,
                                Email = user.Email,
                                Id = user.Id.ToString(),
                                IsUpdated = user.IsUpdated,
                                Name = user.Name,
                                Phone = user.Phone
                            })
                        }
                    );
            }
            catch (Exception ex)
            {
                return new PaginationResponse<UserProfileResponse>(code: (int)ResponseCode.Failed, message: ResponseCode.Failed.GetDescription());
            }
            finally
            {
            }
        }
    }
}
