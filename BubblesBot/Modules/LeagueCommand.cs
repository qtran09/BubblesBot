using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RiotSharp;

namespace BubblesBot.Modules
{
    public class LeagueCommand : ModuleBase<SocketCommandContext>
    {
        private RiotApi api = RiotApi.GetInstance("DEVKEYGOESHERE");
        private StaticRiotApi staticapi = StaticRiotApi.GetInstance("DEVKEYGOESHERE");


        [Command("lol")]
        public async Task basicHelp()
        {
            await Context.Channel.SendMessageAsync("```COMMANDS\n" +
                "lol match (username) ==> Search a player in game to see champs being played, ranks, and summoners\n" +
                "lol opgg (username) ==> Creates a link to a player's op.gg profile\n" +
                "lol guide (champion) ==> Creates a link to probuilds for a champion```");
        }
        //Current issues are names with spaces
        [Command("lol match")]
        public async Task matchInfo([Remainder] string username = null)
        {
            if(username == null)
            {
                await Context.Channel.SendMessageAsync("Enter a name of a player ingame to search.");
                return;
            }
            string indent = "-----";
  
            var x = api.GetCurrentGame(Platform.NA1, api.GetSummoner(Region.na,username).Id);
            var list = x.Participants;
            long team = list.First().TeamId;
            string nameroleleague = "```\nName " + indent + "League " + indent + "Champion " + indent + "Summoners " + indent + "\n\n" + x.GameQueueType + "\n\nTeam 1\n\n";
            for (int i = 0; i < list.Count; i++)
            {
                var player = list.ElementAt(i);
                try {
                    if (team != player.TeamId)
                    {
                        nameroleleague += "\n\nTeam 2\n\n";
                        team = player.TeamId;
                    }
                    var ids = new List<long>();
                    string url = "https://na.op.gg/summoner/userName=" + player.SummonerName;
                    ids.Add(player.SummonerId);
                    var currPlayer = api.GetLeagues(Region.na, ids).First().Value.First();
                    nameroleleague += player.SummonerName + indent + 
                        currPlayer.Tier + " " + currPlayer.Entries.First().Division + " " + currPlayer.Entries.First().LeaguePoints + " LP" + 
                        indent + staticapi.GetChampion(Region.na,(int)player.ChampionId).Name + indent +
                        staticapi.GetSummonerSpells(Region.na,RiotSharp.StaticDataEndpoint.SummonerSpellData.basic).SummonerSpells.First(s => s.Value.Id == player.SummonerSpell1).Value.Name + indent + staticapi.GetSummonerSpells(Region.na, RiotSharp.StaticDataEndpoint.SummonerSpellData.basic).SummonerSpells.First(s => s.Value.Id == player.SummonerSpell2).Value.Name + "\n\n";
                } catch (RiotSharpException e)
                {
                    Console.Write(e.StackTrace);
                    nameroleleague += player.SummonerName + indent + "Unranked" + indent + staticapi.GetChampion(Region.na,(int)player.ChampionId).Name +  indent +
                        staticapi.GetSummonerSpells(Region.na, RiotSharp.StaticDataEndpoint.SummonerSpellData.basic).SummonerSpells.First(s => s.Value.Id == player.SummonerSpell1).Value.Name + indent + staticapi.GetSummonerSpells(Region.na, RiotSharp.StaticDataEndpoint.SummonerSpellData.basic).SummonerSpells.First(s => s.Value.Id == player.SummonerSpell2).Value.Name + "\n\n";

                    continue;
                }
            }
            nameroleleague += "```\nTeam 1\n";
            int j = 0;
            foreach(RiotSharp.CurrentGameEndpoint.Participant url in list)
            {
                nameroleleague += "["  + url.SummonerName +  "] https://na.op.gg/summoner/userName=" + url.SummonerName.Replace(" ","+") + "\n";
                j++;
                if (j == 5)
                {
                    nameroleleague += "\nTeam2\n";
                }
            }
            await Context.Channel.SendMessageAsync(nameroleleague);
        }

        [Command("lol opgg")]
        public async Task opgg([Remainder]string username = null)
        {
            if(username == null)
            {
                await Context.Channel.SendMessageAsync("Enter a username");
                return;
            }
            try
            {
                Console.Write(api.GetSummoner(Region.na, username).Name);
                await Context.Channel.SendMessageAsync("[" + username + "] https://na.op.gg/summoner/userName=" + username.Replace(" ", "+"));
            }catch(RiotSharpException e)
            {
                Console.Write(e.StackTrace);
                await Context.Channel.SendMessageAsync("User doesn't exist");
            }
        }
        [Command("lol guide")]
        public async Task champGuide([Remainder] string champ = null)
        {
            if(champ == null)
            {
                await Context.Channel.SendMessageAsync("Enter a champion name");
                return;
            }
            try
            {
                if (champ.ToLower().Equals("sol")) champ = "aurelion sol";
                if (champ.ToLower().Equals("cho")) champ = "cho gath";
                await Context.Channel.SendMessageAsync("[" + champ + "] http://www.probuilds.net/champions/details/" + champ.Replace("'","").Replace(" ",""));
            }
            catch(RiotSharpException e)
            {
                await Context.Channel.SendMessageAsync("Champion doesn't exist(be careful with spelling)");
            }
        }
    }
}
