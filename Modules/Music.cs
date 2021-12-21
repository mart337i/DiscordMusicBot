using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordMusicBot.Services;
using Victoria;

namespace DiscordMusicBot.Modules
{
    public class Music : ModuleBase<SocketCommandContext>
    {
        private Musicservice _musicservice;

        public Music(Musicservice musicservice)
        {
            _musicservice = musicservice;
        }
        
        [Command("join")]
        public async Task join()
        {
            var user = Context.User as SocketGuildUser;
            if (user.VoiceChannel is null)
            {
                await ReplyAsync("you need to connect to a voice channel.");
                return;
            }
            else
            {
                await _musicservice.connectAsync(user.VoiceChannel, Context.Channel as ITextChannel);
                await ReplyAsync($"now connected to {user.VoiceChannel.Name}");
            }
        }

        [Command("Leave")]
        public async Task Leave()
        {
            var user = Context.User as SocketGuildUser;
            if (user.VoiceChannel is null)
            {
                await ReplyAsync("please join the channel the bot is in");
            }
            else
            {
                await _musicservice.LeaveAsync(user.VoiceChannel);
                await ReplyAsync($"the bot has left {user.VoiceChannel.Name}"); 
            }
        }

        [Command("play")]
        public async Task play([Remainder]string quary)
        {
            await ReplyAsync("proccessing");
            var result = await _musicservice.playAsync(quary, Context.Guild.Id);
            await ReplyAsync(result);
        }

        [Command("stop")]
        public async Task stop()
        {
            await _musicservice.stopAsync();
            await ReplyAsync("music stopped");
        }

        [Command("skip")]
        public async Task skipAsync()
        {
            var result = await _musicservice.skipAsync();
            await ReplyAsync(result);
        }

        [Command("help")]
        public async Task help()
        {
            await ReplyAsync("1. use 'join' to make the bot join your chat.\n2. @bot play and the link or the name of the song");
        }
        
    }
}