using Discord.WebSocket;
using System;
using System.Threading.Tasks;

namespace HisnuBot.Events
{
	public class Ready
	{
		public static async Task OnReadyAsync(DiscordSocketClient client)
		{
			// Affiche le message que le bot est connecté
			Console.WriteLine("Le bot est connecté et prêt !");

			// Récupère les informations de l'application
			var applicationInfo = await client.GetApplicationInfoAsync();
			// Affiche le nom de l'application
			Console.WriteLine($"Nom de l'application : {applicationInfo.Name}");
		}
	}
}
