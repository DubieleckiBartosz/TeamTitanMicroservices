﻿using General.Domain.Entities;

namespace General.Application.Contracts;

public interface IPostRepository : IBaseRepository<Post>
{
    Task<Post?> GetPostWithCommentsAsync(int postId);
    Task<Post?> GetPostWithDetailsAsync(int postId);
}