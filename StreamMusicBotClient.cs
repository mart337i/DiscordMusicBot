using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordMusicBot.Modules;
using DiscordMusicBot.Services;
using Microsoft.Extensions.DependencyInjection;
using Victoria;

namespace DiscordMusicBot
{
    public class StreamMusicBotClient
    {
        private DiscordSocketClient _client;
        private CommandService _cmdService;
        private IServiceProvider _services;

        public StreamMusicBotClient(CommandService cmdService = null, DiscordSocketClient client = null)
        {
            _client = client ?? new DiscordSocketClient(new DiscordSocketConfig
            {
                AlwaysDownloadUsers = true,
                MessageCacheSize = 50,
                LogLevel = LogSeverity.Debug
            });
            _cmdService = cmdService ?? new CommandService(new CommandServiceConfig()
            {       
                LogLevel = LogSeverity.Verbose,
                CaseSensitiveCommands = false

            });

        }

        public async Task InitializeAsync()
        {
            await _client.LoginAsync(TokenType.Bot, "OTA1MDUxMzg1NzA2NDY3MzI4.YYEcyQ.nd8VudQNrQg5KwscvwvCHKVTDws");
            await _client.StartAsync();
            _client.Log += logAsync;
            _services = setupServiceProvider();

            
            
            var cmdHandler = new CommandHandler(_client, _cmdService, _services);
            await cmdHandler.InitializeAsync();

            await _services.GetRequiredService<Musicservice>().InitializeAsync();
            
            //makes it run forever
            await Task.Delay(-1);
        }

        private Task logAsync(LogMessage logMessage)
        {
            Console.WriteLine(logMessage.Message);
            return Task.CompletedTask;
        }

        private IServiceProvider setupServiceProvider()
            => new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton(_cmdService)
                .AddSingleton<LavaRestClient>()
                .AddSingleton<LavaSocketClient>()
                .AddSingleton<Musicservice>()
                .BuildServiceProvider();


    }
}