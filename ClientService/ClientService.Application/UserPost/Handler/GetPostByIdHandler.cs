using ClientService.Application.Common.Enums;
using ClientService.Application.Common.Extensions;
using ClientService.Application.Services.CurrentUserService;
using ClientService.Application.User.Model;
using ClientService.Application.UserPost.Command;
using ClientService.Application.UserPost.Model;
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
    public class GetPostByIdHandler: IRequestHandler<GetPostByIdRequest, Response<PostDetailResponse>>
    {
        private readonly ILogger<GetPostByIdHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public GetPostByIdHandler(
            ILogger<GetPostByIdHandler> logger, IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<Response<PostDetailResponse>> Handle(GetPostByIdRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _currentUserService.GetCurrentAccount();
                var postQuery = await _unitOfWork.PostRepository.GetAsync(
                        expression: x => x.Id == request.Id,
                        includeFunc: (query) => query.Include(post => post.Author).Include(post => post.EndStation).Include(post => post.StartStation).Include(post => post.Applier)
                    );

                var post = postQuery.FirstOrDefault();

                if(post == null)
                {
                    return new Response<PostDetailResponse>(code: (int)ResponseCode.PostErrorNotFound, message: ResponseCode.PostErrorNotFound.GetDescription());
                }

                return new Response<PostDetailResponse>(code: 0,
                        data: new PostDetailResponse()
                        {
                            Id = post.Id,
                            Role = post.TripRole.GetDescription().ToUpper(),
                            Description = post.Description,
                            Status = post.Status.ToString().ToUpper(),
                            StartTime = post.StartTime,
                            FeedbackContent = post.FeedbackContent,
                            FeedbackPoint = post.FeedbackPoint,
                            StartStation = post.StartStation,
                            EndStation = post.EndStation,
                            CreatedAt = post.CreatedAt,
                            UpdatedAt = post.UpdatedAt,
                            Applications = post.Applier.ConvertAll(applier => new UserProfileResponse()
                            {
                                Avatar = applier.AvartarUlr,
                                AveragePoint = applier.averagePoint,
                                Email = applier.Email,
                                Id = applier.Id.ToString(),
                                IsUpdated = applier.IsUpdated,
                                Name = applier.Name,
                                Phone = applier.Phone
                            }),
                            Author= new UserProfileResponse()
                            {
                                Avatar = post.Author.AvartarUlr,
                                AveragePoint = post.Author.averagePoint,
                                Email = post.Author.Email,
                                Id = post.Author.Id.ToString(),
                                IsUpdated = post.Author.IsUpdated,
                                Name = post.Author.Name,
                                Phone = post.Author.Phone
                            }

                        }
                    );
            }
            catch (Exception ex)
            {
                return new Response<PostDetailResponse>(code: (int)ResponseCode.Failed, message: ResponseCode.Failed.GetDescription());
            }
            finally
            {
            }
        }
    }
}
