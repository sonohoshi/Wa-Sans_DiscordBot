using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace SansIsAlive.Bot
{
    public enum CommandCondition
    {
        None = -1,
        BasicCommand = 0,
        Wa,
        Sans
    }
    
    public class BotInitializer
    {
        private static DiscordSocketClient _client;
        private static CommandService _commands;

        private const string WaString = "와!", SansString = "샌즈!";

        public static async Task BotMain(string token)
        {
            _client ??= new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Verbose                       
            });
            _commands = new CommandService(new CommandServiceConfig 
            {
                LogLevel = LogSeverity.Verbose                     
            });
            
            _client.Log += OnClientLogReceived;    
            _commands.Log += OnClientLogReceived;

            await _client.LoginAsync(TokenType.Bot, token); 
            await _client.StartAsync();                 

            _client.MessageReceived += OnClientMessage;

            //await _commands.AddModuleAsync<BasicCommandModule>(null);
            
            Console.WriteLine($"\n\n{_commands.Modules.Count()}\n\n");
            
            await Task.Delay(-1);
        }

        private static async Task OnClientMessage(SocketMessage arg)
        {
            if (!(arg is SocketUserMessage message)) return;

            int pos = 0;

            var commandType = CheckMessageCondition(message);

            if (CheckMessageIsCommand(message)/* || commandType == CommandCondition.None*/)
                return;

            var context = new SocketCommandContext(_client, message);

            if (commandType == CommandCondition.Wa)
            {
                await context.Channel.SendMessageAsync(SansString);
            }
            else if (commandType == CommandCondition.Sans)
            {
                await context.Channel.SendMessageAsync(WaString);
            }
            else
            {
                await _commands.ExecuteAsync(context, pos, null);
            }
        }

        private static bool CheckMessageIsCommand(SocketUserMessage msg)
        {
            return msg.Author.IsBot;
        }

        private static Task OnClientLogReceived(LogMessage msg)
        {
            Console.WriteLine($"[{DateTime.Now}] {msg}"); 
            return Task.CompletedTask;
        }

        public static CommandCondition CheckMessageCondition(SocketUserMessage msg)
        {
            int pos = 0;
            if (msg.Content.Contains("와")) return CommandCondition.Wa;
            if (msg.Content.Contains("샌즈")) return CommandCondition.Sans;
            if (msg.HasCharPrefix('$', ref pos)) return CommandCondition.BasicCommand;
            return CommandCondition.None;
        }
    }
}