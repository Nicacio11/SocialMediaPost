using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Post.Query.Domain.Entities;

namespace Post.Query.Domain.Repositories
{
    public interface ICommentRepository
    {
        Task CreateAsync(CommentEntity commentEntity);

        Task UpdateAsync(CommentEntity commentEntity);

        Task DeleteAsync(Guid commentId);

        Task<CommentEntity> GetByIdAsync(Guid commentId);
    }
}