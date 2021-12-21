using System;
using System.Linq;
using System.Threading.Tasks;
using Victoria;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Victoria.Entities;

namespace DiscordMusicBot.Services
{
    public class Musicservice
    {
        private LavaRestClient _lavaRestClient;
        private LavaSocketClient _lavaSocketClient;
        private DiscordSocketClient _client;
        private LavaPlayer _player;
                
                
        public Musicservice(LavaRestClient lavaRestClient, LavaSocketClient lavaSocketClient, DiscordSocketClient client)
        {
            _client = client;
            _lavaRestClient = lavaRestClient;
            _lavaSocketClient = lavaSocketClient;
        }

        public Task InitializeAsync()
        {
            _client.Ready += ClientOnReadyAsync;
            _lavaSocketClient.Log += LogAsync;
            _lavaSocketClient.OnTrackFinished += TrackFinished;
            return Task.CompletedTask;
        }

        public async Task connectAsync(SocketVoiceChannel VoicecChannel, ITextChannel textChannel)
            => await _lavaSocketClient.ConnectAsync(VoicecChannel, textChannel);

        public async Task LeaveAsync(SocketVoiceChannel voiceChannel)
            => await _lavaSocketClient.DisconnectAsync(voiceChannel);


        public async Task<string> playAsync(string quary, ulong guildId)
        {
            _player = _lavaSocketClient.GetPlayer(guildId);
            var result = await _lavaRestClient.SearchYouTubeAsync(quary);
            if (result.LoadType == LoadType.NoMatches || result.LoadType == LoadType.LoadFailed)
            {
                return "no mathes found.";
            }

            var track = result.Tracks.FirstOrDefault();
            
            //testing
            Console.WriteLine("i am rigth here ");
            
            if (_player.IsPlaying)
            {
                _player.Queue.Enqueue(track);
                return $"{track.Title} has been to the queue";
            }
            else
            {
                await _player.PlayAsync(track);
                return $"now playing{track.Title}";
            }
        }
        
        private async Task ClientOnReadyAsync()
        {
            await _lavaSocketClient.StartAsync(_client, new Configuration // conf is importing for connecting
            {
                LogSeverity = LogSeverity.Verbose
            });
            
        }
        
        private async Task TrackFinished(LavaPlayer player, LavaTrack track, TrackEndReason reason)
        {
            if (!reason.ShouldPlayNext()) { return; }

            if (player.Queue.TryDequeue(out var item) || !(item is LavaTrack nextTrack))
            {
                await player.TextChannel.SendMessageAsync("no more items in Q");
                return;
            }

            await player.PlayAsync(nextTrack);
        }

        private Task LogAsync(LogMessage message)
        {
            Console.WriteLine(message.Message);
            return Task.CompletedTask;
        }


    }
}