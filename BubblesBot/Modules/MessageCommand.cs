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
        public async Task echoWithMention([Remainder] string msg = null)
        {
            if (msg == null)
            {
                await Context.Channel.SendMessageAsync("Write something");
                return;
            }
           await Context.Channel.SendMessageAsync(Context.User.Mention + " " + msg);
        }

        [Command("enm")]
        public async Task echoNoMention([Remainder]string msg = null)
        {

            await removeMessages(1);
            if(msg == null)
            {
                await Context.Channel.SendMessageAsync("Write something");
                return;
            }
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
