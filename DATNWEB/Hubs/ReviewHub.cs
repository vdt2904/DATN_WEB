using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
namespace DATNWEB.Hubs
{
    public class ReviewHub :Hub
    {
        public async Task RequestReview()
        {
            // Gửi một sự kiện tới tất cả các máy khách
            await Clients.All.SendAsync("ReviewRequested");
        }
    }
}
