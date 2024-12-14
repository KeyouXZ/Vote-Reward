# Vote Reward Plugin for Terraria

A simple, lightweight plugin for Terraria that provides rewards to players who vote for your server on [Terraria-Servers.com](http://terraria-servers.com).  

## Features
- Provides customizable rewards for players who vote for the server.
- Uses API integration with Terraria-Servers.com for vote validation.
- Lightweight and easy to configure.
- Commands for both players and administrators.

---

## Installation

1. **Download the Plugin**  
   Download the plugin's `.dll` file from the [Releases](#) page.

2. **Place the File**  
   Place the `.dll` file into the `ServerPlugins` directory of your Terraria server.

3. **Restart the Server**  
   Restart your server to load the plugin.

4. **Configure the Plugin**  
   - Locate the generated `Vote.json` configuration file in the TShock directory.
   - Edit the file to include your **API Key** and other settings.

---

## Configuration

The `Vote.json` file contains the following settings:

```json
{
  "API Key": "XXX",
  "Commands": ["/give 74 %PLAYER% 10"],
  "Reward Message": "[Vote Reward] %PLAYER% has voted for us and received a reward. Use /vote to get the same reward!",
  "Already claim message": "[Vote Reward] You have already claimed your reward for today!",
  "Haven't voted message": "[Vote Reward] You haven't voted today! Head to terraria-servers.com and vote for our server page!"
}
```

- **API Key**: Replace `"XXX"` with your Terraria-Servers.com API key.
- **Commands**: List the commands to be executed as a reward (e.g., giving items, permissions, etc.).
- **Reward Message**: The message broadcasted when a player claims their reward.
- **Already Claim Message**: The message displayed when a player tries to re-claim a reward.
- **Haven't Voted Message**: The message shown when a player has not voted.

---

## Commands

| Command         | Permission         | Description                             |
|------------------|--------------------|-----------------------------------------|
| `/vote`          | `vote.reward`      | Check and claim your voting reward.     |
| `/vote -t`       | `vote.admin`       | Test the vote reward system.            |

---

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

---

## Acknowledgments

Special thanks to the TShock community and Terraria-Servers.com for their API support.
