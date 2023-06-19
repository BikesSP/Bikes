using ClientService.Application.Common.Constants;
using ClientService.Application.Common.Enums;
using ClientService.Application.User.Model;
using ClientService.Domain.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientService.Application.UserPost.Command
{
    public class GetApplicationsByPostIdRequest: IRequest<PaginationResponse<UserProfileResponse>>
    {

        private long _id;
        public long getId() { return _id; }
        public void setId(long id) { _id = id; }

        private int _pageNumber = DefaultPagination.DefaultPageNumber;

        private int _pageSize = DefaultPagination.DefaultPageSize;

        [FromQuery]
        public int PageNumber
        {
            get => _pageNumber;
            set => _pageNumber = value > 0
                ? value
                : DefaultPagination.DefaultPageNumber;
        }

        [FromQuery]
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > 0 && value <= DefaultPagination.MaxPageSize
                ? value
                : DefaultPagination.DefaultPageSize;
        }

        [FromQuery]
        public string? SortColumn { get; set; }

        [FromQuery]
        public SortDirection SortDir { get; set; } = SortDirection.Asc;
    }
}
