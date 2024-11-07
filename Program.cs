using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using System.IO;
using HisnuBot.Loader; // Importer le namespace du loader

namespace HisnuBot
{
	class Program
	{
		private DiscordSocketClient _client;
		private IConfiguration _config;

		static void Main(string[] args) => new Program().RunBotAsync().GetAwaiter().GetResult();

		public async Task RunBotAsync()
		{
			// Charger le fichier de configuration
			_config = new ConfigurationBuilder()
				.SetBasePath(AppContext.BaseDirectory)
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
				.Build();

			// Initialiser le client Discord avec les intents nécessaires
			var config = new DiscordSocketConfig
			{
				GatewayIntents = GatewayIntents.Guilds | GatewayIntents.GuildMessages | GatewayIntents.MessageContent
			};
			_client = new DiscordSocketClient(config);
			_client.Log += Log;

			// Charger les événements
			LoadEvents.RegisterEvents(_client);

			// Lire le token depuis le fichier de configuration
			var token = _config["DiscordToken"];
			await _client.LoginAsync(TokenType.Bot, token);
			await _client.StartAsync();

			// Garder le bot en ligne
			await Task.Delay(-1);
		}

		private Task Log(LogMessage msg)
		{
			Console.WriteLine(msg.ToString());
			return Task.CompletedTask;
		}
	}
}
