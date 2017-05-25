using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BubblesBot{
    class BubbleBot{
        DiscordClient client;
        CommandService commands;
        public BubbleBot()
        {
            client = new DiscordClient(x =>
            {
                x.LogLevel = LogSeverity.Info;
                x.LogHandler = Log;
            });
            client.UsingCommands(x =>
            {
                x.PrefixChar = '!';
                x.AllowMentionPrefix = true;

            });
            commands = client.GetService<CommandService>();
            echo();
            Disconnect();
            join();
            client.ExecuteAndWait(async () =>
            {
                await client.Connect("MzE3MjA0MDg5NDM4MDc2OTQw.DAgo_Q.OhiJqTauiKJZisJsmK0q7o3L_D4", TokenType.Bot);
            });
        }
        private void echo() {
            commands.CreateCommand("echo")
                .Alias("echo")
                .Description("Echo's a message")
                .Parameter("message", ParameterType.Optional)
                .Do(async e =>
                {
                    if(e.GetArg("message") == "")
                    {
                        await e.Channel.SendMessage("The inner machinations of this program are an enigma");
                        return;
                    }
                    else
                    {
                        await e.Channel.SendMessage(e.User.Mention + " " + e.GetArg("message"));
                    }
                });
        }
        
        private void Disconnect()
        {
            commands.CreateCommand("disconnect").Do(e =>
            {
                client.SetStatus(UserStatus.Invisible);
            });
        }
        private void join()
        {
            commands.CreateCommand("join").Do(e =>
            {
                client.SetStatus(UserStatus.Online);
            });
        }
        private void Log(object sender, LogMessageEventArgs error){
            Console.WriteLine(error.Message);
        }

    }
}
