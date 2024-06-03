if (localStorage.getItem('redirected') === 'true') {
    // Gọi hàm của bạn
    gettrans();

    // Xóa trạng thái đã được redirect
    localStorage.removeItem('redirected');
}
function info() {
    $.ajax({
        url: baseUrl+'api/infouser',
        method: 'GET',
        contentType: 'application/json',
        dataType: 'json',
        error: function (response) {
            console.log("error");
        },
        success: function (response) {
            let table = '';
            table += '<h5>Hồ sơ</h5>';
            table += '<div class="personal__profile">';
            table += '<div class="d-flex align-items-center">';
            table += '<span class="b-avatar img-avatar badge-secondary rounded-circle">';
            table += '<span class="b-avatar-img"><img src="https://images.fptplay.net/media/structure/default_avatar_new_2023_03_08.png" alt="avatar"></span>';
            table += '</span>';
            table += '<div class="profile-info">';
            table += '<div class="profile-info-div">';
            table += '<p class="name">' + response.username + '</p>';
            table += '<div class="d-flex align-items-center">';
            table += '<span class="label">Giới tính:</span>';
            table += '<span class="info">( Chưa cập nhật )</span>';
            table += '<span class="bi-blank line b-icon bi"></span>';
            table += '<span class="label">Sinh nhật:</span>';
            table += '<span class="info">( Chưa cập nhật )</span>';
            table += '</div>';
            table += '</div>';
            table += '<div class="button-change">Thay đổi</div>';
            table += '</div>';
            table += '</div>';
            table += '</div>';
            table += '<h5>Tài khoản và bảo mật</h5>';
            table += '<div class="personal__account-security container">';
            table += '<div class="row">';
            table += '<div class="col-12">';
            table += '<div class="item">';
            table += '<span class="label">';
            table += 'ID';
            table += '<span class="info">&nbsp;&nbsp;' + response.userId + '</span>';
            table += '</span>';
            table += '<div class="button-change"></div>';
            table += '</div>';
            table += '</div>';
            table += '<div class="col-12">';
            table += '<div class="item">';
            table += '<span class="label">';
            table += 'Email';
            table += '<span class="info">&nbsp;&nbsp;' + response.email + '</span>';
            document.getElementById("email").value = response.email;
            table += '</span>';
            table += '<div class="button-change"></div>';
            table += '</div>';
            table += '</div>';
            table += '<div class="col-12">';
            table += '<div class="item">';
            table += '<span class="label">';
            table += 'Số điện thoại';
            if (response.phone == null) {
                table += '<span class="info">&nbsp;&nbsp;( Chưa cập nhật )</span>';
            } else {
                table += '<span class="info">&nbsp;&nbsp;' + response.phone + '</span>';
                document.getElementById("phone").value = response.phone;
            }
            table += '</span>';
            table += '<div class="button-change" data-toggle="modal" data-target="#phoneModalCenter">Thay đổi</div>';
            table += '</div>';
            table += '</div>';
            table += '<div class="col-12">';
            table += '<div class="item">';
            table += '<span class="label">';
            table += 'Mật khẩu';
            table += '<span class="info" data-password="' + response.password + '">&nbsp;&nbsp;******</span>';
            table += '</span>';
            table += '<div class="button-change" data-toggle="modal" data-target="#passModalCenter">Thay đổi</div>';
            table += '</div>';
            table += '</div>';
            table += '</div>';
            table += '</div>';
            table += '';
            document.getElementById('infou').innerHTML = table;
            updateModal();
        },
        fail: function (response) {
            console.log("fail");
        }
    })
}

function logout() {
    $.ajax({
        url: baseUrl+'api/infouser/logout',
        method: 'POST',
        contentType: 'application/json',
        dataType: 'json',
        success: function (response) {
            console.log("Logout successful");

        }, error: function (response) {
            console.log("Error:", response);
            localStorage.removeItem("token");
            localStorage.removeItem("uid");
            window.location.href = "/home/login";
        }
    });
}

function updatesdt() {
    var sdt = document.getElementById("phone").value;
    $.ajax({
        url: baseUrl+'api/infouser/updatesdt',
        method: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(sdt),
        success: function (response) {
            info();
            $('#phoneModalCenter').modal('hide');
        }, error: function (response) {
            console.log("Error:", response);
        }
    });
}

function validateOTP(input) {
    // Xóa bất kỳ ký tự không phải là số nào được nhập vào
    input.value = input.value.replace(/\D/g, '');

    // Giới hạn độ dài của đầu vào là 6 ký tự
    if (input.value.length > 6) {
        input.value = input.value.slice(0, 6);
    }
}


var currentStep = 1;

function nextStep() {
    if (currentStep == 1) {
        sendmail();
    } else {
        checkotp();
    }
}

function prevStep() {
    if (currentStep > 1) {
        currentStep--;
        updateModal();
    }
}

function updateModal() {
    $("#step1, #step2, #step3").hide();
    $("#step" + currentStep).show();

    // Hiển thị hoặc ẩn nút "Quay lại" và "Tiếp theo" tùy thuộc vào bước hiện tại
    if (currentStep === 1) {
        $(".btn-secondary").hide();
        $(".btn-success").hide();
    } else {
        $(".btn-secondary").show();
        $(".btn-success").hide();
    }
    if (currentStep === 3) {
        $(".btn-primary").hide();
        $(".btn-success").show();
    } else {
        $(".btn-primary").show();
    }
}

function sendmail() {
    var currentDate = new Date();
    if (currentStep < 3) {
        currentStep++;
        updateModal();
    }
    // Lấy các thành phần của ngày
    var year = currentDate.getFullYear(); // Năm
    var month = ('0' + (currentDate.getMonth() + 1)).slice(-2); // Tháng (từ 01 đến 12)
    var day = ('0' + currentDate.getDate()).slice(-2); // Ngày (từ 01 đến 31)
    var hours = ('0' + currentDate.getHours()).slice(-2); // Giờ (từ 00 đến 23)
    var minutes = ('0' + currentDate.getMinutes()).slice(-2); // Phút (từ 00 đến 59)
    // Tạo chuỗi ngày tháng theo định dạng YYYY-MM-DD HH:mm
    var formattedDate = year + '-' + month + '-' + day + ' ' + hours + ':' + minutes;
    var mail = document.getElementById("email").value;
    var subject = 'Đổi mật khẩu tại Anime-Thai';
    var code = (Math.floor(Math.random() * 999999) + 1).toString().padStart(6, '0');
    var num = 2;
    // Định dạng nội dung email với một placeholder cho đồng hồ đếm ngược
    var body = '<p>Bạn đang muốn thay đổi mật khẩu.</p>';
    body += '<p>Mã xác nhận của bạn là: ' + code + '.</p>';
    body += '<div style="text-align: center;"><img src="https://i.mailtimer.io/lTF47IQjCs.gif?start={' + formattedDate + '}" border="0" alt="mailtimer.io" style="max-width:100%;" /></div>';
    // Gửi email qua AJAX
    $.ajax({
        url: baseUrl+'api/account/sendmail',
        method: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({ toEmail: mail, subject: subject, body: body, code: code, num: num }),
        success: function (response) {
            console.log(response);
            
        },
        error: function (xhr, status, error) {
            console.error("Lỗi khi đăng nhập:", error);
            // Xử lý lỗi ở đây nếu cần thiết
        }
    });
}

function checkotp() {
    var otp = document.getElementById("otp").value;
    var mail = document.getElementById("email").value;

    $.ajax({
        url: `http://animethai-001-site1.atempurl.com/api/infouser/checkotp?otp=${otp}&mail=${mail}`,
        method: 'GET',
        success: function (response) {
            console.log(response);
            if (currentStep < 3) {
                currentStep++;
                updateModal();
            }
        },
        error: function (xhr, status, error) {
            console.error("Lỗi khi đăng nhập:", error);
            // Xử lý lỗi ở đây nếu cần thiết
        }
    });
}

function updatepass() {
    var pass = document.getElementById("newPassword").value;
    var mail = document.getElementById("email").value;
    $.ajax({
        url: baseUrl+'api/infouser/updatepass',
        method: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({ mail: mail, pass: pass }),
        success: function (response) {
            console.log(response);
            info();
            $('#passModalCenter').modal('hide');
        }, error: function (response) {
            console.log("Error:", response);
        }
    });
}
// lịch sử giao dịch
function gettrans(page) {
    if (typeof page === 'undefined') {
        page = 1;
    }
    $.ajax({
        url: baseUrl+'api/infouser/transactionhistory?page=' + page,
        method: 'GET',
        contentType: 'application/json',
        dataType: 'json',
        error: function (response) {
            console.log("error");
        },
        success: function (response) {
            var results = response.results;
            var paginationInfo = response.paginationInfo
            const len = results.length;
            let table = '';
            table += '<h5>Lịch sử giao dịch</h5> <br />';
            table += '<table class="table table-dark">';
            table += '<thead>';
            table += '<tr>';
            table += '<th scope="col">Mã</th>';
            table += '<th scope="col">Số tiền</th>';
            table += '<th scope="col">Ngày thanh toán</th>';
            table += '<th scope="col">Nội dung</th>';
            table += '<th scope="col">Trạng thái</th>';
            table += '</tr>';
            table += '</thead>';
            table += '<tbody>';
            for (var i = 0; i < len; i++) {
                if (results[i].status == "PAID") {
                    var transactions = results[i].transactions;
                    table += '<tr>';
                    table += '<th scope="row">' + results[i].orderCode + '</th>';
                    table += '<td>' + results[i].amount + '</td>';
                    table += '<td>' + transactions[0].transactionDateTime + '</td>';
                    table += '<td>' + transactions[0].description + '</td>';
                    table += '<td>Thanh toán thành công</td>';
                    table += '</tr>';
                }
            }
            table += '</tbody>';
            table += '</table>';
            renderPagination(paginationInfo)
            document.getElementById('infou').innerHTML = table;
            updateModal();
        },
        fail: function (response) {
            console.log("fail");
        }
    })
}


function renderPagination(paginationInfo) {
    let paginationHtml = '';

    // Add page links to pagination
    if (paginationInfo.totalPages > 1) {
        for (let i = 1; i <= paginationInfo.totalPages; i++) {
            paginationHtml += '<a href="#" onclick="gettrans(' + i + ')">' + i + '</a>';
        }
    }
    // Display previous page link
    if (paginationInfo.currentPage > 1) {
        paginationHtml += '<a href="#" onclick="category( ' + (paginationInfo.currentPage - 1) + ')"><i class="fa fa-angle-double-left"></i></a>';
    }

    // Display next page link
    if (paginationInfo.currentPage < paginationInfo.totalPages) {
        paginationHtml += '<a href="#" onclick="category(' + (paginationInfo.currentPage + 1) + ')"><i class="fa fa-angle-double-right"></i></a>';
    }

    // Display pagination
    document.getElementById('pagination').innerHTML = paginationHtml;
}


// goi da mua           

function getbought(page) {
    if (typeof page === 'undefined') {
        page = 1;
    }
    $.ajax({
        url: baseUrl+'api/infouser/bought?page=' + page,
        method: 'GET',
        contentType: 'application/json',
        dataType: 'json',
        error: function (response) {
            console.log("error");
        },
        success: function (response) {
            var results = response.results;
            var paginationInfo = response.paginationInfo
            const len = results.length;
            let table = '';
            table += '<h5>Dịch vụ đã mua</h5> <br />';
            table += '<table class="table table-dark">';
            table += '<thead>';
            table += '<tr>';
            table += '<th scope="col">Tên dịch vụ</th>';
            table += '<th scope="col">Ngày mua</th>';
            table += '<th scope="col">Thời gian sử dụng</th>';
            table += '<th scope="col">Trạng thái</th>';
            table += '</tr>';
            table += '</thead>';
            table += '<tbody>';
            var currentTime = new Date().getTime();
            for (var i = 0; i < len; i++) {
                var subscriptionDate = new Date(results[i].subscriptionDate);
                var expirationDate = new Date(results[i].expirationDate);
                var timeRemaining = expirationDate.getTime() - subscriptionDate.getTime();


                // Chuyển thời gian từ milliseconds sang ngày, giờ, phút, giây
                var days = Math.floor(timeRemaining / (1000 * 60 * 60 * 24));
                var hours = Math.floor((timeRemaining % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
                var minutes = Math.floor((timeRemaining % (1000 * 60 * 60)) / (1000 * 60));
                var seconds = Math.floor((timeRemaining % (1000 * 60)) / 1000);

                // Tạo chuỗi để hiển thị
                var remainingTimeStr = days + " ngày " + hours + " giờ " + minutes + " phút " + seconds + " giây ";

                table += '<tr>';
                table += '<th scope="row">' + results[i].packageName + '</th>';
                table += '<td>' + results[i].subscriptionDate + '</td>';
                table += '<td>' + days + ' Ngày</td>';
                var dynamicId = 'hsd_' + i;
                if (expirationDate > currentTime) {
                    table += '<td id="' + dynamicId + '"></td>';
                    // Gọi hàm cập nhật thời gian còn lại với id tương ứng
                    updateRemainingTime(dynamicId,results[i].expirationDate);
                } else {
                    table += '<td>Đã hết hạn</td>';
                }
                table += '<td>Thanh toán thành công</td>';
                table += '</tr>';
            }
            table += '</tbody>';
            table += '</table>';
            renderPagination1(paginationInfo)
            document.getElementById('infou').innerHTML = table;
            updateModal();
        },
        fail: function (response) {
            console.log("fail");
        }
    })
}
function updateRemainingTime(elementId, expirationDateString) {
    var currentTime = new Date().getTime();
    var expirationDate = new Date(expirationDateString);

    // Kiểm tra tính hợp lệ của expirationDate
    if (isNaN(expirationDate.getTime())) {
        console.error("Ngày hết hạn không hợp lệ: ", expirationDateString);
        return;
    }

    // Tính thời gian còn lại
    var timeRemaining = expirationDate.getTime() - currentTime;

    // Tạo chuỗi để hiển thị và cập nhật nội dung của phần tử HTML
    var timer = setInterval(function () {
        timeRemaining--;
        var days = Math.floor(timeRemaining / (1000 * 60 * 60 * 24));
        var hours = Math.floor((timeRemaining % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
        var minutes = Math.floor((timeRemaining % (1000 * 60 * 60)) / (1000 * 60));
        var seconds = Math.floor((timeRemaining % (1000 * 60)) / 1000);
        if (days != 0) {
            document.getElementById(elementId).innerText ="Còn " +days +"ngày";
        }
        if (days == 0 && hours != 0) {
            document.getElementById(elementId).innerText = "Còn " + hours + "giờ";
        } if (days == 0 && hours == 0) {
            document.getElementById(elementId).innerText = "Còn " + minutes + "Phút";
        }

        // Kiểm tra xem thời gian còn lại có bằng 0 không
        if (timeRemaining <= 0) {
            clearInterval(timer); // Dừng đếm ngược
            document.getElementById(elementId).innerText = "Đã hết hạn";
        }
    }, 1000);
}


function renderPagination1(paginationInfo) {
    let paginationHtml = '';

    // Add page links to pagination
    if (paginationInfo.totalPages > 1) {
        for (let i = 1; i <= paginationInfo.totalPages; i++) {
            paginationHtml += '<a href="#" onclick="getbought(' + i + ')">' + i + '</a>';
        }
    }
    // Display previous page link
    if (paginationInfo.currentPage > 1) {
        paginationHtml += '<a href="#" onclick="getbought( ' + (paginationInfo.currentPage - 1) + ')"><i class="fa fa-angle-double-left"></i></a>';
    }

    // Display next page link
    if (paginationInfo.currentPage < paginationInfo.totalPages) {
        paginationHtml += '<a href="#" onclick="getbought(' + (paginationInfo.currentPage + 1) + ')"><i class="fa fa-angle-double-right"></i></a>';
    }

    // Display pagination
    document.getElementById('pagination').innerHTML = paginationHtml;
}










