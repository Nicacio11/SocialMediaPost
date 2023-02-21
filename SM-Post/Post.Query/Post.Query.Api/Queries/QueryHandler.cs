using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Post.Query.Domain.Entities;
using Post.Query.Domain.Repositories;

namespace Post.Query.Api.Queries
{
    public class QueryHandler : IQueryHandler
    {
        private readonly IPostRepository _postRepository;

        public QueryHandler(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task<List<PostEntity>> HandleAsync(FindAllPostsQuery query)
        {
            _ = query ?? throw new ArgumentNullException(nameof(query));
            return await _postRepository.ListAllAsync();
        }

        public async Task<List<PostEntity>> HandleAsync(FindPostByIdQuery query)
        {
            _ = query ?? throw new ArgumentNullException(nameof(query));
            var post = await _postRepository.GetByIdAsync(query.Id);

            return new List<PostEntity>(){ post };
        }

        public async Task<List<PostEntity>> HandleAsync(FindPostsByAuthorQuery query)
        {
            _ = query ?? throw new ArgumentNullException(nameof(query));
            return await _postRepository.ListBytAuthorAsync(query.Author);
        }

        public async Task<List<PostEntity>> HandleAsync(FindPostsWithCommentsQuery query)
        {
            _ = query ?? throw new ArgumentNullException(nameof(query));
            return await _postRepository.ListWithCommentsAsync();
        }

        public async Task<List<PostEntity>> HandleAsync(FindPostsWithLikesQuery query)
        {
            _ = query ?? throw new ArgumentNullException(nameof(query));
            return await _postRepository.ListWithLikesAsync(query.NumberOfLikes);
        }
    }
}