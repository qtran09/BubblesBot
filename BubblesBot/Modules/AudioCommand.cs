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
    public class AudioCommand : ModuleBase<ICommandContext>
    {
        private AudioService _service;
        public AudioCommand(AudioService service)
        {
            _service = service;
        }

        [Command("join", RunMode = RunMode.Async)]
        public async Task JoinCmd()
        {           
            await _service.JoinAudio(Context.Guild, (Context.User as IVoiceState).VoiceChannel);   
        }

        [Command("leave", RunMode = RunMode.Async)]
        public async Task LeaveCmd()
        {
            await _service.LeaveAudio(Context.Guild);
        }
        
        [Command("add",RunMode = RunMode.Async)]
        public async Task playAudio(string url)
        {
            await _service.PlayAudio(Context.Guild, (Context.Channel as IMessageChannel), url);
        }
        
    }
}
