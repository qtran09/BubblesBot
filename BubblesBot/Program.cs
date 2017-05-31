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
        private string TOKEN = "MzE3MjA0MDg5NDM4MDc2OTQw.DA627A.nqv0LDKBK5THAsCaykNjXyfWZCs";
        public static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();
   
        public async Task MainAsync()
        {
            _client = new DiscordSocketClient();
            await _client.LoginAsync(TokenType.Bot, TOKEN);
            await _client.StartAsync();
            _handler = new CommandHandler(_client);
            await Task.Delay(-1);
        }
    }
}
