using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using MemeThroneBot.Commands;

namespace MemeThroneBot
{
    public class CommandHandler
    {
        private char PREFIX = '!';

        private readonly DiscordSocketClient client;
        private readonly CommandService commands;

        public CommandHandler(DiscordSocketClient client, CommandService commands)
        {
            this.client = client;
            this.commands = commands;

            commands.Log += CommandLog;
        }

        private async Task CommandLog(LogMessage arg)
        {
            Console.WriteLine($"[CommandService] {arg}");
            await Task.CompletedTask;
        }

        public async Task InstallCommandsAsync()
        {
            client.MessageReceived += HandleCommandAsync;
            await commands.AddModulesAsync(Assembly.GetEntryAssembly(), null);
        }

        private async Task HandleCommandAsync(SocketMessage msgParam)
        {
            var message = msgParam as SocketUserMessage;
            if (message == null) { return; }

            int argPos = 0;

            if (!(message.HasCharPrefix(PREFIX, ref argPos) || message.HasMentionPrefix(client.CurrentUser, ref argPos)) || message.Author.IsBot)
            {
                return;
            }

            var context = new SocketCommandContext(client, message);
            await commands.ExecuteAsync(context, argPos, null);
        }
    }
}