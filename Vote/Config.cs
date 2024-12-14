using Newtonsoft.Json;
using TShockAPI;


namespace Vote
{
	public class Config
	{
		[JsonProperty("API Key")]
		public string apiKey { get; set; } = "XXX";
		[JsonProperty("Commands")]
		public List<string> Commands = new List<string>();
		[JsonProperty("Reward Message")]
		public string rewardMessage { get; set; } = "[Vote Reward] %PLAYER% has voted for us and receieved a reward. Use /vote to get the same reward!";
		[JsonProperty("Already claim message")]
		public string alreadyClaimedMessage { get; set; } = "[Vote Reward] You have already claimed your reward for today!";
		[JsonProperty("Haven't voted message")]
		public string haventVotedMessage { get; set; } = "[Vote Reward] You haven't voted today! Head to terraria-servers.com and vote for our server page!";

		public static Config Read()
		{
			string filepath = Path.Combine(TShock.SavePath, "Vote.json");

			try
			{
				Config config = new Config().defaultConfig();

				if (!File.Exists(filepath))
				{
					File.WriteAllText(filepath, JsonConvert.SerializeObject(config, Formatting.Indented));
				}
				config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(filepath));

				return config;
			}
			catch (Exception ex)
			{
				TShock.Log.ConsoleError(ex.ToString());
				return new Config();
			}
		}

		private Config defaultConfig()
		{
			Config config = new Config();
			config.Commands = new List<string>
			{
				"/give 74 %PLAYER% 10"
			};

			return config;
		}
	}
}
