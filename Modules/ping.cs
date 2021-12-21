using System.Net.Sockets;
using System.Threading.Tasks;
using Discord.Commands;

namespace DiscordMusicBot.Modules
{
    public class ping : ModuleBase<SocketCommandContext>
    {
        [Command("ping")]
        public async Task Pong()
        {
            await ReplyAsync("pong!");
        }
    }
}