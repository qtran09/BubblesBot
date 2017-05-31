using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BubblesBot.Modules
{
    public class MessageCommands : ModuleBase<SocketCommandContext>
    {
        [Command("e")]
        public async Task echoWithMention(string msg)
        {
           await Context.Channel.SendMessageAsync(Context.User.Mention.ToString() + " " + msg);
        }

        [Command("enm")]
        public async Task echoNoMention(string msg)
        {
            await removeMessages(1);
            await Context.Channel.SendMessageAsync(msg);
        }


        [Command("rm")]
        public async Task removeMessages([Remainder] int delete = 0)
        {
            if (delete == 0)
            {
                await Context.Channel.SendMessageAsync("Specify amount of messages to clear | !rm amount");
                return;
            }
            int MAXDEL = 100;
            while (delete > 0)
            {
                if (delete < 100)
                {
                    MAXDEL = delete;
                }
                await Context.Channel.DeleteMessagesAsync(await Context.Channel.GetMessagesAsync(MAXDEL).Flatten());
                delete -= MAXDEL;
            }
        }
    }
}
