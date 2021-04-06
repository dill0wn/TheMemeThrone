using System;
using Discord.WebSocket;

namespace MemeThroneBot
{
    public class ReactionContext
    {
        public bool IsReaction { get; private set; }
        public SocketReaction Reaction { get; private set; }
        
        internal void Init(SocketReaction reaction)
        {
            IsReaction = true;
            this.Reaction = reaction;
        }
    }
}