var connection = new signalR.HubConnectionBuilder().withUrl("/notificationHub").build();

connection.on("ReceiveNewEpisodeNotification", function (newEpisode) {
    showNotification(newEpisode);
});

connection.start().catch(function (err) {
    return console.error(err.toString());
});

function showNotification(newEpisode) {
    // Tạo div thông báo mới
    var notification = document.createElement("div");
    notification.className = "notification";
    notification.innerHTML = `
                <div>
                <a href="../home/Episode?id=${newEpisode.animeId}&ep=${newEpisode.episode}">
                <img class="lazy" src="${newEpisode.imageVUrl}" data-original="${newEpisode.imageVUrl}">
                <div>
                     ${newEpisode.permission === 0 ?
                    `<h4><img src="../home/img/vip-pass.png" alt="VIP Icon" style="width: 40px; height: 40px;"> ${newEpisode.animeName}</h4>` :
                    `<h4>${newEpisode.animeName}</h4>`
                    }
                    <h6>
                    <ul>
                    <li>${newEpisode.title}</li>
                    <li> Tập ${newEpisode.episode}</li>
                    </ul>
                    </h6>
                </div>
                </a>
                </div>
                <button class="close-btn">&times;</button>
            `;
    notification.style.top = '20px';
    notification.style.right = '20px';

    // Thêm sự kiện click cho nút đóng
    notification.querySelector(".close-btn").addEventListener("click", function () {
        notification.remove();
    });

    // Thêm thông báo vào notificationArea
    document.getElementById("notificationArea").appendChild(notification);

    // Tự động xóa thông báo sau 5 giây
    setTimeout(function () {
        notification.remove();
    }, 10000);
}