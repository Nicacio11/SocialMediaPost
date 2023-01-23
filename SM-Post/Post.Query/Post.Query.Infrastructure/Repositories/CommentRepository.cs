using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Post.Query.Domain.Entities;
using Post.Query.Domain.Repositories;
using Post.Query.Infrastructure.DataAccess;

namespace Post.Query.Infrastructure.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly DatabaseContextFactory contextFactory;

        public CommentRepository(DatabaseContextFactory contextFactory)
        {
            this.contextFactory = contextFactory;
        }
        public async Task CreateAsync(CommentEntity commentEntity)
        {
            using var context = contextFactory.CreateDbContext();
            context.Comments.Add(commentEntity);
            _ = await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid commentId)
        {
            using var context = contextFactory.CreateDbContext();
            var comments = await GetByIdAsync(commentId);

            if(comments == null) return;

            context.Comments.Remove(comments);
            _ = await context.SaveChangesAsync();
        }

        public async Task<CommentEntity> GetByIdAsync(Guid commentId)
        {
            using var context = contextFactory.CreateDbContext();
            return await context.Comments
                .FirstOrDefaultAsync(x => x.CommentId == commentId);
        }

        public async Task UpdateAsync(CommentEntity commentEntity)
        {
            using var context = contextFactory.CreateDbContext();
            context.Comments.Update(commentEntity);

            _ = await context.SaveChangesAsync();
        }
    }
}