using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Discord.WebSocket;

namespace HisnuBot.Loader
{
	public static class LoadEvents
	{
		public static void RegisterEvents(DiscordSocketClient client)
		{
			// Enregistre les événements avec des méthodes spécifiques
			client.Ready += () => Events.Ready.OnReadyAsync(client);
			client.MessageReceived += OnMessageReceivedAsync;

			// Charge dynamiquement les événements à partir des fichiers DLL dans le dossier Events
			LoadDynamicEvents(client);
		}

		private static void LoadDynamicEvents(DiscordSocketClient client)
		{
			// Obtient le chemin du dossier Events
			string eventsPath = Path.Combine(AppContext.BaseDirectory, "Events");

			// Affiche les fichiers dans le dossier Events
			Console.WriteLine("Éléments chargés dans Events :");

			// Cherche tous les fichiers dans le dossier Events
			var files = Directory.GetFiles(eventsPath, "*.*", SearchOption.TopDirectoryOnly);
			foreach(var file in files)
			{
				Console.WriteLine($"Fichier : {Path.GetFileName(file)}"); // Affiche seulement le nom du fichier
			}

			// Cherche tous les sous-dossiers dans le dossier Events
			var directories = Directory.GetDirectories(eventsPath, "*", SearchOption.TopDirectoryOnly);
			foreach(var dir in directories)
			{
				Console.WriteLine($"Dossier : {Path.GetFileName(dir)}"); // Affiche seulement le nom du dossier

				// Charge les fichiers dans chaque sous-dossier
				var subFiles = Directory.GetFiles(dir, "*.*", SearchOption.TopDirectoryOnly);
				foreach(var subFile in subFiles)
				{
					Console.WriteLine($"  Fichier : {Path.GetFileName(subFile)}"); // Affiche le nom des fichiers dans le sous-dossier
				}
			}

			// Cherche tous les fichiers .dll dans le dossier Events et ses sous-dossiers
			var dllFiles = Directory.GetFiles(eventsPath, "*.dll", SearchOption.AllDirectories);

			foreach(var file in dllFiles)
			{
				// Tente de charger l'assembly à partir du fichier DLL
				try
				{
					var assembly = Assembly.LoadFrom(file);

					// Recherche toutes les classes qui implémentent des méthodes d'événements
					var eventTypes = assembly.GetTypes()
						.Where(t => t.IsClass && !t.IsAbstract && t.Namespace == "HisnuBot.Events");

					foreach(var type in eventTypes)
					{
						// Cherche une méthode qui commence par "On" pour l'enregistrement des événements
						var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Static)
										  .Where(m => m.Name.StartsWith("On"));

						foreach(var method in methods)
						{
							// Trouve l'événement approprié dans le client Discord
							var eventName = method.Name.Substring(2); // Enlève "On" du début
							var eventInfo = client.GetType().GetEvent(eventName);

							if(eventInfo != null)
							{
								// Crée un délégué pour le gestionnaire d'événements et l'attache
								var handler = Delegate.CreateDelegate(eventInfo.EventHandlerType, method);
								eventInfo.AddEventHandler(client, handler);
							}
						}
					}
				} catch(Exception ex)
				{
					Console.WriteLine($"Erreur lors du chargement de {file}: {ex.Message}");
				}
			}
		}

		// Gestion de l'événement MessageReceived
		private static async Task OnMessageReceivedAsync(SocketMessage message)
		{
			if(message.Author.IsBot) return;

			// Exemple de réponse simple
			if(message.Content == "!ping")
			{
				await message.Channel.SendMessageAsync("Pong!");
			}
		}
	}
}
