using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRS.Core.Events;

namespace Post.Common.Events
{
    public class CommentAddedEvent : BaseEvent
    {
        public CommentAddedEvent() : base(nameof(CommentAddedEvent))
        {
        }

        public string Comment { get; set; }

        public string Username { get; set; }

        public string GuidCommentId { get; set; }

        public DateTime CommentDate { get; set; }
    }
}