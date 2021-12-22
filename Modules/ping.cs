using System;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace DiscordMusicBot.Modules
{
    public class ping : ModuleBase<SocketCommandContext>
    {
        [Command("ping")]
        public async Task Pong()
        {
            await ReplyAsync("pong!");
        }

        [Command("!delete")] // creates a 
        [RequireOwner]
        public async Task Delete([Remainder]int msg)
        {
            try
            {
                if (msg == null || msg <= 0)
                {
                    await ReplyAsync("you need to add a number to that command");
                }else if(msg > 1000)
                {
                    await ReplyAsync("you cant delete more the 1000 msg");
                }
            
                var messages = Context.Channel.GetMessagesAsync(msg).Flatten();
                foreach (var h in  await messages.ToArrayAsync())
                {
                    await Context.Channel.DeleteMessageAsync(h);
                }
            }
            catch (Exception e)
            {
                await ReplyAsync($"{e}");
                Console.WriteLine(e);
                throw;
            }
        }
    }
}