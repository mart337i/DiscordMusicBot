using System.Threading.Tasks;

namespace DiscordMusicBot
{
    class Program
    {
        static async Task Main(string[] args)
            => await new StreamMusicBotClient().InitializeAsync();
    }
} 