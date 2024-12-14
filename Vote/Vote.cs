using Microsoft.Xna.Framework;
using Terraria;
using TerrariaApi.Server;
using TShockAPI;
using TShockAPI.Hooks;

namespace Vote
{
    [ApiVersion(2, 1)]
    public class Vote : TerrariaPlugin
    {
        public override string Name => "Vote Reward";
        public override Version Version => new Version(1, 0, 0);
        public override string Author => "Keyou";
        public override string Description => "A simple, and light-weight Vote Rewards plugin.";
        public static string apiKey;
        public static Config config;

        public Vote(Main game) : base(game)
        {

        }

        public override void Initialize()
        {
            GeneralHooks.ReloadEvent += Reload;
            ServerApi.Hooks.GameInitialize.Register(this, GameInit);
            ServerApi.Hooks.GamePostInitialize.Register(this, PostInit);
        }

        public void GameInit(EventArgs args)
        {
            Commands.ChatCommands.Add(new Command("vote.reward", VoteCmd, "vote", "reward"));
            config = Config.Read();
            apiKey = config.apiKey;
        }

        public void PostInit(EventArgs args)
        {
            if (apiKey == "XXX")
            {
                TShock.Log.ConsoleError("[Vote Reward] API Key not found. Plugin will not work.");
            }
        }

        public void Reload(ReloadEventArgs args)
        {
            args.Player.SendMessage("[Vote Reward] API Key reloaded!", Color.LightGreen);
            config = Config.Read();
            apiKey = config.apiKey;

            if (apiKey == "XXX")
            {
                args.Player.SendErrorMessage("[Vote Reward] API Key not found. Plugin will not work.");
            }
        }

        public void VoteCmd(CommandArgs args)
        {
            if (!args.Player.IsLoggedIn)
            {
                args.Player.SendErrorMessage("You must be logged in to use this command!");
                return;
            }

            var isBeingUsedForTesting = false;
            if(args.Parameters.Count == 1)
            {
                if(args.Parameters[0] == "-t")
                {
                    if (args.Player.HasPermission("vote.admin"))
                    {
                        isBeingUsedForTesting = true;
                        args.Player.SendInfoMessage("-t");
                    }
                }
            }

            TSPlayer Player = args.Player;

            if (checkifPlayerVoted(Player).Result == true || isBeingUsedForTesting == true)
            {
                if(rewardClaimed(Player).Result == true || isBeingUsedForTesting == true)
                {
                    string rewardMsg = config.rewardMessage;
                    rewardMsg = rewardMsg.Replace("%PLAYER%", Player.Name);
                    TSPlayer.All.SendMessage(rewardMsg, Microsoft.Xna.Framework.Color.LightGreen);

                    foreach (string cmd in config.Commands)
                    {
                        if (string.IsNullOrEmpty(cmd))
                        {
                            return;
                        }
                        string newCmd = cmd.Replace("%PLAYER%", '"' + Player.Name + '"');
                        Commands.HandleCommand(TSPlayer.Server, newCmd);
                    }
                    return;
                }
                else
                {
                    Player.SendErrorMessage(config.alreadyClaimedMessage);
                    return;
                }

            }

            Player.SendMessage(config.haventVotedMessage, Color.LightGreen);
            return;
        }

        public static async Task<bool> rewardClaimed(TSPlayer player)
        {
            bool hasVoted = false;

            string voteUrl = "http://terraria-servers.com/api/?action=post&object=votes&element=claim&key=" + apiKey + "&username=" + player.Name;

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    using (HttpResponseMessage res = await client.GetAsync(voteUrl))
                    {
                        using (HttpContent content = res.Content)
                        {
                            var data = await content.ReadAsStringAsync();

                            if (data != null)
                            {
                                if (data == "1")
                                {
                                    hasVoted = true;
                                    return hasVoted;
                                }
                                else
                                {
                                    hasVoted = false;
                                    return hasVoted;
                                }
                            }
                            else
                            {
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex);
                return hasVoted;
            }

            return hasVoted;

        }

        public static async Task<bool> checkifPlayerVoted(TSPlayer player)
        {
            bool hasVoted = false;

            string voteUrl = ($"http://terraria-servers.com/api/?object=votes&element=claim&key={apiKey}&username={player.Name}");

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    using (HttpResponseMessage res = await client.GetAsync(voteUrl))
                    {
                        using (HttpContent content = res.Content)
                        {
                            var data = await content.ReadAsStringAsync();

                            if (data != null)
                            {
                                if (data == "1" || data == "2")
                                {
                                    hasVoted = true;
                                    return hasVoted;
                                }
                                else
                                {
                                    hasVoted = false;
                                    return hasVoted;
                                }
                            }
                            else
                            {
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex);
                return hasVoted;
            }

            return hasVoted;

        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                GeneralHooks.ReloadEvent -= Reload;
                ServerApi.Hooks.GameInitialize.Deregister(this, GameInit);
                ServerApi.Hooks.GamePostInitialize.Deregister(this, PostInit);
            }
            base.Dispose(disposing);
        }
    }
}
