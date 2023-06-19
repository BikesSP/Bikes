﻿using ClientService.Application.UserPost.Model;
using ClientService.Domain.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientService.Application.UserPost.Command
{
    public class GetPostByIdRequest: IRequest<Response<PostDetailResponse>>
    {
        public long Id { get; set; }
    }
}
