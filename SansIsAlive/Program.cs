using System;
using System.Configuration;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace SansIsAlive
{
    internal class Program
    {
        private static DiscordSocketClient client;
        private static CommandService commands;
        static void Main(string[] args)
        {
            var token = ConfigurationManager.AppSettings["Token"];
            Console.WriteLine("SANS IS ALIVE\n");
            new Program().BotMain(token).GetAwaiter().GetResult();
        }
        
        public async Task BotMain(string token)
        {
            client = new DiscordSocketClient(new DiscordSocketConfig
            {    
                LogLevel = LogSeverity.Verbose                       
            });
            commands = new CommandService(new CommandServiceConfig 
            {
                LogLevel = LogSeverity.Verbose                     
            });
            
            client.Log += OnClientLogReceived;    
            commands.Log += OnClientLogReceived;

            await client.LoginAsync(TokenType.Bot, token); 
            await client.StartAsync();                 

            client.MessageReceived += OnClientMessage;    

            await Task.Delay(-1);   
        }

        private async Task OnClientMessage(SocketMessage arg)
        {
            var message = arg as SocketUserMessage;
            if (message == null) return;

            int pos = 0;
            
            if (!(message.HasCharPrefix('!', ref pos) ||
                  message.HasMentionPrefix(client.CurrentUser, ref pos)) ||
                message.Author.IsBot)
                return;

            var context = new SocketCommandContext(client, message);             

            await context.Channel.SendMessageAsync($"Received command : {message.Content}"); 
        }

        private Task OnClientLogReceived(LogMessage msg)
        {
            Console.WriteLine(msg.ToString()); 
            return Task.CompletedTask;
        }
    }
}