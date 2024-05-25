let connection1;
var ids = "";
function watch(a, b,c) {
    var urls = '';
    if (b == null) {
        urls = 'https://localhost:7274/api/AnimeWatching?aid=' + a;
    } else {
        urls = 'https://localhost:7274/api/AnimeWatching?aid=' + a + '&ep=' + b;
    }
    $.ajax({
        url: urls,
        method: 'GET',
        contentType: 'application/json',
        dataType: 'json',
        error: function (xhr) {
            if (xhr.status === 400) {
                var errorMessage = xhr.responseText;
                if (errorMessage === "Bạn không phải là VIP" || errorMessage === "Bạn không phải là SVIP") {
                    window.location.href = "../home/package";
                } else {
                    // Xử lý các trường hợp lỗi khác nếu cần
                    console.log(errorMessage);
                }
            } else {
                console.log("Lỗi không xác định");
            }
        },
        success: function (response) {
            let table = '';
            ids = response.episodeId;
            table += '<div class="col-lg-12">';
            table += '<div class="anime__video__player">';
            table += '<video id="player" playsinline controls poster="' + response.backgroundImageUrl + '" style="width:100%;max-width: 1140px; max-height: 654.06px; ">';
            table += '<source src="' + response.videoUrl + '" type="video/mp4" />';
            table += '<track kind="captions" label="English captions" src="#" srclang="en" default />';
            table += '</video>';
            table += '</div>';
            table += '<div class="anime__details__episodes">';
            table += '<div class="section-title">';
            table += '<h5>List Name</h5>';
            table += '</div>';
            var eps = response.e;
            if (response.permission != 0) {
                for (var i = 0; i < eps.length; i++) {
                    if (eps[i].ep == response.epside) {
                        if (new Date(eps[i].postingDate).getTime() < (new Date().getTime() + 7 * 24 * 60 * 60 * 1000) && new Date(eps[i].postingDate).getTime() > (new Date().getTime())) {
                            // Your logic here
                            table += '<a href="../Home/Episode?id=' + response.animeId + '&ep=' + eps[i].ep + '" style = "color: white; background-color:blue;">Ep ' + eps[i].ep + '<img src = "../home/img/vip-pass.png" alt = "VIP Icon" style = "width: 20px; height: 20px;" ></a>';
                        }
                        else if (new Date(eps[i].postingDate).getTime() > (new Date().getTime() + 7 * 24 * 60 * 60 * 1000)) {
                            // Your logic here
                            table += '<a href="../Home/Episode?id=' + response.animeId + '&ep=' + eps[i].ep + '" style = "color: white; background-color:blue;">Ep ' + eps[i].ep + '<img src = "../home/img/svip_icon.jpg" alt = "VIP Icon" style = "width: 20px; height: 20px;" ></a>';
                        }
                        else {
                            table += '<a href="../Home/Episode?id=' + response.animeId + '&ep=' + eps[i].ep + '" style = "color: white; background-color:blue;">Ep ' + eps[i].ep + '</a>';
                        }
                    }
                    else {
                        if (new Date(eps[i].postingDate).getTime() < (new Date().getTime() + 7 * 24 * 60 * 60 * 1000) && new Date(eps[i].postingDate).getTime() > (new Date().getTime())) {
                            // Your logic here
                            table += '<a href="../Home/Episode?id=' + response.animeId + '&ep=' + eps[i].ep + '">Ep ' + eps[i].ep + '<img src = "../home/img/vip-pass.png" alt = "VIP Icon" style = "width: 20px; height: 20px;" ></a>';
                        }
                        else if (new Date(eps[i].postingDate).getTime() > (new Date().getTime() + 7 * 24 * 60 * 60 * 1000)) {
                            // Your logic here
                            table += '<a href="../Home/Episode?id=' + response.animeId + '&ep=' + eps[i].ep + '">Ep ' + eps[i].ep + '<img src = "../home/img/svip_icon.jpg" alt = "VIP Icon" style = "width: 20px; height: 20px;" ></a>';
                        }
                        else {
                            table += '<a href="../Home/Episode?id=' + response.animeId + '&ep=' + eps[i].ep + '">Ep ' + eps[i].ep + '</a>';
                        }

                    }
                }
            } else {
                for (var i = 0; i < eps.length; i++) {
                    if (eps[i].ep == response.epside) {
                        if (new Date(eps[i].postingDate).getTime() > (new Date().getTime() + 7 * 24 * 60 * 60 * 1000)) {
                            // Your logic here
                            table += '<a href="../Home/Episode?id=' + response.animeId + '&ep=' + eps[i].ep + '" style = "color: white; background-color:blue;">Ep ' + eps[i].ep + '<img src = "../home/img/svip_icon.jpg" alt = "VIP Icon" style = "width: 20px; height: 20px;" ></a>';
                        }
                        else if (eps[i].ep == 0) {
                            table += '<a href="../Home/Episode?id=' + response.animeId + '&ep=' + eps[i].ep + '" style = "color: white; background-color:blue;">Ep ' + eps[i].ep + '</a>';
                        }
                        else {
                            table += '<a href="../Home/Episode?id=' + response.animeId + '&ep=' + eps[i].ep + '" style = "color: white; background-color:blue;">Ep ' + eps[i].ep + '<img src = "../home/img/vip-pass.png" alt = "VIP Icon" style = "width: 20px; height: 20px;" ></a>';
                        }
                    }
                    else {
                        if (new Date(eps[i].postingDate).getTime() > (new Date().getTime() + 7 * 24 * 60 * 60 * 1000)) {
                            // Your logic here
                            table += '<a href="../Home/Episode?id=' + response.animeId + '&ep=' + eps[i].ep + '">Ep ' + eps[i].ep + '<img src = "../home/img/svip_icon.jpg" alt = "VIP Icon" style = "width: 20px; height: 20px;" ></a>';
                        }
                        else if (eps[i].ep == 0) {
                            table += '<a href="../Home/Episode?id=' + response.animeId + '&ep=' + eps[i].ep + '">Ep ' + eps[i].ep + '</a>';
                        }
                        else {
                            table += '<a href="../Home/Episode?id=' + response.animeId + '&ep=' + eps[i].ep + '">Ep ' + eps[i].ep + '<img src = "../home/img/vip-pass.png" alt = "VIP Icon" style = "width: 20px; height: 20px;" ></a>';
                        }

                    }
                }
            }
            console.log(response.epside)
            table += '</div>';
            table += '</div>';
            document.getElementById('video').innerHTML = table;
            connection1 = new signalR.HubConnectionBuilder()
                .withUrl("/commenthub/" + response.episodeId)
                .build();
            connection1.on("ReviewRequested", () => {
                // Gọi hàm để thực hiện lấy dữ liệu review
                getcmt(response.episodeId);
            });

            connection1.start().catch(err => console.error(err));
            var videoElement = document.getElementById('player');
            videoElement.addEventListener('loadedmetadata', function () {
                videoElement.currentTime = response.view;
              //  videoElement.play(); // Bắt đầu phát video sau khi đã đặt thời gian
            }, false);
        },
        fail: function (response) {
            console.log("fail");
        }
    })
}

function getcmt(id, page) {
    page = page || 1;
    $.ajax({
        url: 'https://localhost:7274/api/animewatching/getcmt?eid=' + id + '&page=' + page,
        method: 'GET',
        contentType: 'application/json',
        dataType: 'json',
        error: function (response) {
            console.log("error");
        },
        success: function (response) {
            const products = response.pagedCMT; // Lấy dữ liệu sản phẩm từ response
            const len = products.length;
            let table = '';
            table += '<div class="section-title">';
            table += '<h5>Bình Luận</h5>';
            table += '</div>';
            for (var i = 0; i < len; i++) {
                var date = new Date(products[i].commentDate);
                var now = new Date();
                var date1 = date.toISOString().slice(0, 10);
                var date2 = now.toISOString().slice(0, 10);
                var diff = now.getTime() - date.getTime();
                var diffM = Math.floor(diff / (1000 * 60));
                var diffD = Math.floor(diff / (1000 * 60 * 60 * 24));
                var str = '';
                var str1 = '';
                if (diffM < 60) {
                    if (diffM === 0) {
                        str = 'Bây giờ';
                    } else {
                        str = diffM + ' phút trước'
                    }
                } else if (date1 == date2) {
                    var hour = Math.floor(diff / (1000 * 60 * 60));
                    str = hour + ' giờ trước';
                } else if (diffD <= 7) {
                    str = diffD + ' ngày trước';
                } else {
                    str = date1;
                }
                table += '<div class="anime__review__item">';
                table += '<div class="anime__review__item__pic"><img src="../Home/img/avatar.jfif" alt=""></div>';
                table += '<div class="anime__review__item__text">';
                table += '<h6>' + products[i].user + ' - <span>' + str + '</span></h6>';
                table += '<p>' + products[i].comment1 + '</p>';
                table += '</div>';
                table += '</div>';

            }
            document.getElementById('cmt').innerHTML = table;
            renderPagination(response.paginationInfo, id);
        },
        fail: function (response) {
            console.log("fail");
        }
    })
}

function renderPagination(paginationInfo, id) {
    let paginationHtml = '<nav aria-label="Page navigation"><ul class="pagination">';

    // Display previous page link
    if (paginationInfo.currentPage > 1) {
        paginationHtml += '<li class="page-item"><a class="page-link" onclick="getcmt(\'' + id + '\',' + (paginationInfo.currentPage - 1) + ')" style="cursor:pointer;"><span aria-hidden="true">&laquo;</span></a></li>';
    }

    // Add page links to pagination
    if (paginationInfo.totalPages > 1) {
        for (let i = 1; i <= paginationInfo.totalPages; i++) {
            paginationHtml += '<li class="page-item"><a class="page-link" onclick="getcmt(\'' + id + '\',' + i + ')" style="cursor:pointer;">' + i + '</a></li>';
        }
    }

    // Display next page link
    if (paginationInfo.currentPage < paginationInfo.totalPages) {
        paginationHtml += '<li class="page-item"><a class="page-link" onclick="getcmt(\'' + id + '\',' + (paginationInfo.currentPage + 1) + ')" style="cursor:pointer;"><span aria-hidden="true">&raquo;</span></a></li>';
    }

    paginationHtml += '</ul></nav>';

    // Display pagination
    document.getElementById('pagination').innerHTML = paginationHtml;
}

function addreview(id) {
    // Tạo một đối tượng Date hiện tại
    var currentDate = new Date();
    var year = currentDate.getFullYear();
    var month = ('0' + (currentDate.getMonth() + 1)).slice(-2); // Lấy tháng, thêm 0 nếu cần thiết
    var day = ('0' + currentDate.getDate()).slice(-2); // Lấy ngày, thêm 0 nếu cần thiết
    var hours = ('0' + currentDate.getHours()).slice(-2); // Lấy giờ, thêm 0 nếu cần thiết
    var minutes = ('0' + currentDate.getMinutes()).slice(-2); // Lấy phút, thêm 0 nếu cần thiết
    var seconds = ('0' + currentDate.getSeconds()).slice(-2); // Lấy giây, thêm 0 nếu cần thiết

    // Tạo chuỗi định dạng "yyyy-mm-ddThh:mm:ss"
    var formattedDateTime = year + '-' + month + '-' + day + 'T' + hours + ':' + minutes + ':' + seconds;

    $.ajax({
        url: 'https://localhost:7274/api/animewatching/addcmt',
        method: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({ episodeId: id, userId: localStorage.getItem("uid"), commentDate: formattedDateTime, Comment1: document.getElementById('idrv').value }),
        error: function (response) {
            console.log("error");
        },
        success: function (response) {
            connection1.invoke("RequestReview").catch(err => console.error(err));
            document.getElementById("idrv").value = ""; // Xóa nội dung của textarea
        },

        fail: function (response) {
            getreview(id);
            console.log("fail");
        }
    })
}

var requestSent = false;

window.addEventListener('beforeunload', function (event) {
    // Đảm bảo rằng yêu cầu chỉ được gửi một lần khi người dùng rời khỏi trang
    if (!requestSent) {
        // Lấy phần tử video bằng ID
        var player = document.getElementById("player");

        // Kiểm tra nếu player tồn tại
        if (player) {
            var currentTime = player.currentTime;
            var duration = player.duration;

            // Gửi yêu cầu HTTP POST đến API để ghi nhận thời gian đang xem
            sendApiRequest(currentTime, ids, duration);
        }

        // Đặt requestSent thành true để đánh dấu đã gửi yêu cầu
        requestSent = true;
    }

    // Chrome cần một giá trị cho event.returnValue
});


function sendApiRequest(time, id, times) {
    var bool = 0;
    if (time > times / 2) {
        bool = 1;
    }
    // Tạo một đối tượng Date hiện tại
    var currentDate = new Date();
    var year = currentDate.getFullYear();
    var month = ('0' + (currentDate.getMonth() + 1)).slice(-2); // Lấy tháng, thêm 0 nếu cần thiết
    var day = ('0' + currentDate.getDate()).slice(-2); // Lấy ngày, thêm 0 nếu cần thiết
    var hours = ('0' + currentDate.getHours()).slice(-2); // Lấy giờ, thêm 0 nếu cần thiết
    var minutes = ('0' + currentDate.getMinutes()).slice(-2); // Lấy phút, thêm 0 nếu cần thiết
    var seconds = ('0' + currentDate.getSeconds()).slice(-2); // Lấy giây, thêm 0 nếu cần thiết

    // Tạo chuỗi định dạng "yyyy-mm-ddThh:mm:ss"
    var formattedDateTime = year + '-' + month + '-' + day + 'T' + hours + ':' + minutes + ':' + seconds;
    // Gửi yêu cầu AJAX POST đến API
/*    $.ajax({
        url: 'https://localhost:7274/api/animewatching/addview',
        method: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({ episodeId: id, userId: localStorage.getItem("uid"), viewDate: formattedDateTime, duration: formatTime(time), isView: bool }),
        success: function (response) {
            console.log(response);
        },
        error: function (response) {
            console.log(response);
        }
    });*/
    var data = JSON.stringify({
        episodeId: id,
        userId: localStorage.getItem("uid"),
        viewDate: formattedDateTime,
        duration: formatTime(time),
        isView: bool
    });

    // Sử dụng Navigator.sendBeacon để gửi yêu cầu không đồng bộ
    var url = 'https://localhost:7274/api/animewatching/addview';
    var blob = new Blob([data], { type: 'application/json' });
    navigator.sendBeacon(url, blob);
}


function formatTime(seconds) {
    var hours = Math.floor(seconds / 3600);
    var minutes = Math.floor((seconds % 3600) / 60);
    var remainingSeconds = Math.floor(seconds % 60);

    // Chuyển đổi các giá trị thành chuỗi, nếu cần thiết thêm số 0 ở trước
    var formattedTime = (hours < 10 ? '0' : '') + hours + ':' +
        (minutes < 10 ? '0' : '') + minutes + ':' +
        (remainingSeconds < 10 ? '0' : '') + remainingSeconds;

    return formattedTime;
}









