
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BubblesBot
{
    class CommandHandler
    {
        private DiscordSocketClient _client;
        private CommandService _service;
        private IServiceCollection _collection;
        private IServiceProvider _provide;

        public CommandHandler(DiscordSocketClient client)
        {
            _client = client;
            _service = new CommandService();
            _collection = new ServiceCollection();
            _collection.AddSingleton(new AudioService());
            _service.AddModulesAsync(Assembly.GetEntryAssembly());
            _client.MessageReceived += HandleCommandAsync;
            _provide = _collection.BuildServiceProvider();
        }

        private async Task HandleCommandAsync(SocketMessage s)
        {
            var message = s as SocketUserMessage;
            if (message == null) return;
            var context = new SocketCommandContext(_client, message);
            int argPos = 0;
            if(message.HasCharPrefix('!', ref argPos))
            {
                var result = await _service.ExecuteAsync(context, argPos,_provide);
                if (!result.IsSuccess && result.Error != CommandError.UnknownCommand) await context.Channel.SendMessageAsync(result.ErrorReason);
            }
        }
    }
}
