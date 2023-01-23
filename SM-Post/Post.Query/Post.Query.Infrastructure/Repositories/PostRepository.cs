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
    public class PostRepository : IPostRepository
    {
        private readonly DatabaseContextFactory contextFactory;

        public PostRepository(DatabaseContextFactory contextFactory)
        {
            this.contextFactory = contextFactory;
        }
        public async Task CreateAsync(PostEntity post)
        {
            using var context = contextFactory.CreateDbContext();
            context.Posts.Add(post);
            _ = await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid postId)
        {
            using var context = contextFactory.CreateDbContext();
            var post = await GetByIdAsync(postId);

            if(post == null) return;

            context.Posts.Remove(post);
            _ = await context.SaveChangesAsync();
        }

        public async Task<PostEntity> GetByIdAsync(Guid postId)
        {
            using var context = contextFactory.CreateDbContext();
            return await context.Posts
                .Include(x => x.Comments)
                .FirstOrDefaultAsync(x => x.PostId == postId);
        }

        public async Task<List<PostEntity>> ListAllAsync()
        {
            using var context = contextFactory.CreateDbContext();
            return await context.Posts.AsNoTracking()
                .Include(x => x.Comments)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<PostEntity>> ListBytAuthorAsync(string author)
        {
            using var context = contextFactory.CreateDbContext();
            return await context.Posts.AsNoTracking()
                .Include(x => x.Comments)
                .AsNoTracking()
                .Where(x => x.Author.Contains(author))
                .ToListAsync();
        }

        public async Task<List<PostEntity>> ListWithCommentsAsync()
        {
            using var context = contextFactory.CreateDbContext();
            return await context.Posts.AsNoTracking()
                .Include(x => x.Comments)
                .AsNoTracking()
                .Where(x => x.Comments != null)
                .ToListAsync();
        }

        public async Task<List<PostEntity>> ListWithLikesAsync(int numberOfLikes)
        {
            using var context = contextFactory.CreateDbContext();
            return await context.Posts.AsNoTracking()
                .Include(x => x.Comments)
                .AsNoTracking()
                .Where(x => x.Likes >= numberOfLikes)
                .ToListAsync();
        }

        public async Task UpdateAsync(PostEntity post)
        {
            using var context = contextFactory.CreateDbContext();
            context.Posts.Update(post);

            _ = await context.SaveChangesAsync();
        }
    }
}