using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace DiscordMusicBot
{
    public class CommandHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _cmdService;
        private readonly IServiceProvider _services;

        public CommandHandler(DiscordSocketClient client, CommandService cmdService, IServiceProvider services)
        {
            _client = client;
            _cmdService = cmdService;
            _services = services;
        }

        public async Task InitializeAsync()
        {
            await _cmdService.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
            _cmdService.Log += LogAsync;
            _client.MessageReceived += HandelMessegeAsync;
        }

        public async Task HandelMessegeAsync(SocketMessage msg)
        {
            var argpos = 0;
            if (msg.Author.IsBot) { return; }

            var usermessage = msg as SocketUserMessage;
            if (msg is null) { return; }

            if (!usermessage.HasMentionPrefix(_client.CurrentUser, ref argpos)){return;}

            var context = new SocketCommandContext(_client, usermessage);
            var Result = await _cmdService.ExecuteAsync(context, argpos, _services);
        }
        public Task LogAsync(LogMessage logMessage)
        {
            Console.WriteLine(logMessage.Message);
            return Task.CompletedTask;
        }
    }
    
}