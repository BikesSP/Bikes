﻿using ClientService.Application.Common.Enums;
using ClientService.Application.Common.Extensions;
using ClientService.Application.Services.CurrentUserService;
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
    public class GetActivePostHandler: IRequestHandler<GetActivePostsRequest, PaginationResponse<PostResponse>>
    {
        private readonly ILogger<GetActivePostHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public GetActivePostHandler(
            ILogger<GetActivePostHandler> logger, IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<PaginationResponse<PostResponse>> Handle(GetActivePostsRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var user = _currentUserService.GetCurrentAccount();
                request.ExceptUserId = user.Id.ToString();
                var result = await _unitOfWork.PostRepository.PaginationAsync(
                        page: request.PageNumber,
                        pageSize: request.PageSize,
                        filter: request.GetExpressions(),
                        includeFunc: (query) => query.Include(post => post.Author).Include(post => post.EndStation).Include(post => post.StartStation)
                    );
            

                return new PaginationResponse<PostResponse>(code: 0,
                        data: new PaginationData<PostResponse>()
                        {
                            Page= request.PageNumber,
                            PageSize= request.PageSize,
                            TotalSize=result.Total,
                            TotalPage= (int?)((result?.Total + (long)request.PageSize - 1) / (long)request.PageSize) ?? 0,
                            Items = result.Data.ConvertAll(post => new PostResponse()
                            {
                                Id = post.Id,
                                Role = post.TripRole.GetDescription().ToUpper(),
                                Description = post.Description,
                                StartStationId = post.StartStationId,
                                EndStationId = post.EndStationId,
                                AuthorId = post.AuthorId,
                                AuthorName = post.Author?.Name,
                                Status = post.Status.ToString().ToUpper(),
                                StartTime = post.StartTime,
                                FeedbackContent = post.FeedbackContent,
                                FeedbackPoint = post.FeedbackPoint,
                                StartStation = post.StartStation?.Name,
                                EndStation = post.EndStation?.Name,
                                CreatedAt = post.CreatedAt,
                                UpdatedAt = post.UpdatedAt
                            })
                        }
                    );
            }
            catch (Exception ex)
            {
                return new PaginationResponse<PostResponse>(code: (int)ResponseCode.Failed, message: ResponseCode.Failed.GetDescription());
            }
            finally
            {
            }
        }
    }
}
