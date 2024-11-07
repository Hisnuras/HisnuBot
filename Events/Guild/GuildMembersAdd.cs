using System.Threading.Tasks;
using Discord.WebSocket;

namespace HisnuBot.Events.Guild
{
	public class GuildMemberAdd
	{
		public static async Task OnGuildMemberAdded(SocketGuildUser user)
		{
			// Logique à exécuter lorsque quelqu'un rejoint le serveur
			Console.WriteLine($"{user.Username} a rejoint le serveur !");
		}
	}
}
