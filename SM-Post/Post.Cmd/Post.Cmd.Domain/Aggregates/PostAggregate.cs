using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRS.Core.Domain;
using Post.Common.Events;

namespace Post.Cmd.Domain.Aggregates
{
    public class PostAggregate: AggregateRoot
    {
        private bool _active;

        private string _author;

        private readonly Dictionary<Guid, Tuple<string, string>> _comments = new();

        public bool Active
        {
            get => _active; set => _active = value;
        }

        public PostAggregate()
        {
        }
        public PostAggregate(Guid id, string author, string message)
        {
            RaiseEvent( new PostCreatedEvent()
            {
                Id = id,
                Author = author,
                Message = message,
                DatePosted = DateTime.Now,
            });
        }

        public void Apply(PostCreatedEvent @event)
        {
            _id = @event.Id;
            _active = true;
            _author = @event.Author;
        }

        public void EditMessage(string message)
        {
            if(!_active)
            {
                throw new InvalidOperationException("you cannot edit the message of an inactive post!");
            }

            if(string.IsNullOrWhiteSpace(message))
            {
                throw new InvalidOperationException($"The value of {message} cannot be null or empy. Please provide a valid {nameof(message)}");
            }

            RaiseEvent(new MessageUpdatedEvent()
            {
                Id = _id,
                Message = message,
            });
        }

        public void Apply(MessageUpdatedEvent @event)
        {
            _id = @event.Id;
        }

        public void LikeMessage()
        {
            if(!_active)
            {
                throw new InvalidOperationException("you cannot like an inactive post!");
            }

            RaiseEvent(new PostLikedEvent()
            {
                Id = _id,
            });
        }

        public void Apply(PostLikedEvent @event)
        {
            _id = @event.Id;
        }

        public void AddComment(string comment, string username)
        {
            if(!_active)
            {
                throw new InvalidOperationException("you cannot add the message of an inactive post!");
            }

            if(string.IsNullOrWhiteSpace(comment))
            {
                throw new InvalidOperationException($"The value of {comment} cannot be null or empy. Please provide a valid {nameof(comment)}");
            }

            if(string.IsNullOrWhiteSpace(username))
            {
                throw new InvalidOperationException($"The value of {username} cannot be null or empy. Please provide a valid {nameof(username)}");
            }

            RaiseEvent(new CommentAddedEvent()
            {
                Id = _id,
                CommentId = Guid.NewGuid(),
                Comment = comment,
                Username = username,
                CommentDate = DateTime.Now,
            });
        }

        public void Apply(CommentAddedEvent @event)
        {
            _id = @event.Id;
            _comments.Add(@event.CommentId, new Tuple<string, string>(@event.Comment, @event.Username));
        }

        public void EditComment(Guid commentId, string comment, string username)
        {
            if(!_active)
            {
                throw new InvalidOperationException("you cannot edit a commment of an inactive post!");
            }
            if(!_comments[commentId].Item2.Equals(username, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new InvalidOperationException("you are not allowed to edit a comment that was made by another user!");
            }

            if(string.IsNullOrWhiteSpace(comment))
            {
                throw new InvalidOperationException($"The value of {comment} cannot be null or empy. Please provide a valid {nameof(comment)}");
            }

            RaiseEvent(new CommentUpdatedEvent()
            {
                EditDate = DateTime.Now,
                Comment = comment,
                CommentId = Id,
                Id = _id,
                Username = username
            });
        }

        public void Apply(CommentUpdatedEvent @event)
        {
            _id = @event.Id;
            _comments[@event.CommentId] = new Tuple<string, string>(@event.Comment, @event.Username);
        }

        public void RemoveComment (Guid commentId, string username)
        {
            if(!_active)
            {
                throw new InvalidOperationException("you cannot remove a commment of an inactive post!");
            }

            if(!_comments[commentId].Item2.Equals(username, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new InvalidOperationException("you are not allowed to edit a comment that was made by another user!");
            }
            RaiseEvent(new CommentRemovedEvent
            {
                Id = _id,
                CommentId = commentId
            });
        }

        public void Apply(CommentRemovedEvent @event)
        {
            _id = @event.Id;
            _comments.Remove(@event.CommentId);
        }

        public void DeletePost(string username)
        {
            if(!_active)
            {
                throw new InvalidOperationException("The post has already been removed!");
            }

            if(!_author.Equals(username, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new InvalidOperationException("You are not allowd to delete a post that was made by someone else!");
            }

            RaiseEvent(new PostRemovedEvent(){
                Id = _id
            });
        }

        public void Apply(PostRemovedEvent @event)
        {
            _id = @event.Id;
            _active = false;
        }
    }
}