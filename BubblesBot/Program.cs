using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace BubblesBot
{
    class Program
    {
        private DiscordSocketClient _client;
        private CommandHandler _handler;
        private string TOKEN = "HIDING MY KEY";
        public static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();
   
        public async Task MainAsync()
        {
            _client = new DiscordSocketClient();
            _handler = new CommandHandler(_client);
            await _client.LoginAsync(TokenType.Bot, TOKEN);
            await _client.StartAsync();
            await Task.Delay(-1);
        }
        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }
}
