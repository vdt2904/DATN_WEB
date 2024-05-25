using DATNWEB.Models.ViewModel;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
namespace DATNWEB.Hubs
{
    public class NotificationHub:Hub
    {
        public async Task SendNewEpisodeNotification(NotificationView newEpisode)
        {
            await Clients.All.SendAsync("ReceiveNewEpisodeNotification", newEpisode);
        }
    }
}
