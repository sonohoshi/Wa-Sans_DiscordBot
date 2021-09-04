using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace SansIsAlive.Bot
{
    [Group("와 샌즈")]
    public class BasicCommandModule : ModuleBase<SocketCommandContext>
    {
        [Command("intro")]
        [Alias("자소")]
        public async Task IntroduceSelf()
        {
            var builder = new EmbedBuilder
            {
                Color = Color.Green,
                Title = "와 샌즈! 봇",
                Description = @"와! 샌즈! 아시는구나!",
                Url = @"https://github.com/sonohoshi/Wa-Sans_DiscordBot"
            };

            builder.AddField("만든 새끼", "김선민(sonohoshi)");
            await Context.Channel.SendMessageAsync("", false, builder.Build());
        }
    }
}