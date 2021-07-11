using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace MemeThroneBot
{
    public class ViewFactory
    {
        private readonly DiscordSocketClient client;
        public ViewFactory(DiscordSocketClient client)
        {
            this.client = client;
        }

        public async Task<IGameView> CreateView<T>(ICommandContext context, GameState gameState) where T : IGameView, new()
        {
            var view = new T();
            await view.BuildAsync(context, gameState);
            return view;
        }

        public async Task<IUserMessage> RenderViewAsync(IMessageChannel channel, IGameView view)
        {
            var gameMessage = await channel.SendMessageAsync(text: view.Text, embed: view.EmbedBuilder.Build());
            return gameMessage;
        }

        public async Task<IUserMessage> RenderViewAsDMAsync(IUser user, IGameView view)
        {
            var gameMessage = await user.SendMessageAsync(text: view.Text, embed: view.EmbedBuilder.Build());
            return gameMessage;
        }

        public async Task<IUserMessage> RenderViewAsReplyAsync(MessageReference messageReference, IGameView view)
        {
            var channel = client.GetGuild(messageReference.GuildId.Value).GetTextChannel(messageReference.ChannelId);
            var gameMessage = await channel.SendMessageAsync(text: view.Text, embed: view.EmbedBuilder.Build(), messageReference: messageReference);
            return gameMessage;
        }

        public async Task<IUserMessage> RenderViewAsReplyAsync(IUserMessage message, IGameView view)
        {
            var gameMessage = await message.ReplyAsync(text: view.Text, embed: view.EmbedBuilder.Build());
            return gameMessage;
        }

        public async Task<IUserMessage> UpdateViewAsync(MessageReference messageReference, IGameView view)
        {
            var message = await client
                .GetGuild(messageReference.GuildId.Value)
                .GetTextChannel(messageReference.ChannelId)
                .GetMessageAsync(messageReference.MessageId.Value) as IUserMessage;
            return await UpdateViewAsync(message, view);
        }

        public async Task<IUserMessage> UpdateViewAsync(IUserMessage message, IGameView view)
        {
            await message.ModifyAsync(m =>
            {
                m.Content = view.Text;
                m.Embed = view.EmbedBuilder.Build();
            });
            return message;
        }
    }
}