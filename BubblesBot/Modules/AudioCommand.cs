using Discord.Commands;
using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Audio;
using System.Collections.Concurrent;

namespace BubblesBot.Modules
{
    public class AudioCommand : ModuleBase<SocketCommandContext>
    {
        private ulong _botID = 317204089438076940;
        [Command("join")]
        public async Task SummonChannel()
        {
            var channel = (Context.User as IGuildUser).VoiceChannel;
            if (channel == null)
            {
                await Context.Channel.SendMessageAsync("User must be in a voice channel");
                return;
            }
            var audioClient = await channel.ConnectAsync();
        }
        /*
         * 
         * Doesn't work yet
        [Command("dismiss")]
        public async Task LeaveChannel()
        {
            IAudioClient client;
            await Context.Channel.SendMessageAsync((CC.TryGetValue((Context.User as IGuildUser).VoiceChannel.Id, out client) + ""));
        }
        */
    }
}
