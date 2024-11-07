using System.Threading.Tasks;
using Discord.WebSocket;

namespace HisnuBot.Events.Guild
{
	public class GuildMemberRemove
	{
		public static async Task OnGuildMemberRemoved(SocketGuildUser user)
		{
			// Logique à exécuter lorsque quelqu'un quitte le serveur
			Console.WriteLine($"{user.Username} a quitté le serveur !");
		}
	}
}
