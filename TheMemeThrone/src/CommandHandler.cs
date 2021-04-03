using System;
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
        private readonly char PREFIX = '!';

        private readonly IServiceProvider services;
        private readonly DiscordSocketClient client;
        private readonly CommandService commands;
        private readonly CommandListeners listeners;

        public CommandHandler(IServiceProvider services, DiscordSocketClient client, CommandService commands, CommandListeners listeners)
        {
            this.services = services;
            this.client = client;
            this.commands = commands;
            this.listeners = listeners;

            commands.Log += CommandLog;
        }

        public async Task InstallCommandsAsync()
        {
            client.MessageReceived += HandleCommandAsync;
            await commands.AddModulesAsync(Assembly.GetEntryAssembly(), services);
        }

        private async Task CommandLog(LogMessage arg)
        {
            Console.WriteLine($"[CommandService] {arg}");
            await Task.CompletedTask;
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
            await commands.ExecuteAsync(context, argPos, services);
        }
    }
}