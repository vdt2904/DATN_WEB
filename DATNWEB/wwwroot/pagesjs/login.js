    function loginUser() {
    var username = document.getElementById("username").value;
    var password = document.getElementById("password").value;

    $.ajax({
        url: baseUrl+'api/account',
        method: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({ mail: username, pass: password }),
        success: function (response) {
            var token = response.token;
            // Lưu token vào localStorage hoặc session để sử dụng trong các yêu cầu sau này
            localStorage.setItem("token", token);
            localStorage.setItem("uid", response.userId);
            // Thực hiện hành động sau khi đăng nhập thành công, ví dụ: chuyển hướng đến trang khác
            window.location.href = "/";
        },
        error: function (xhr, status, error) {
            console.error("Lỗi khi đăng nhập:", error);
            var errorMessage = "";
            errorMessage = xhr.responseText;
            var errorHTML = '<div class="alert alert-danger" role="alert">' + errorMessage + '</div>';
            document.getElementById('error_login').innerHTML = errorHTML;
            // Xử lý lỗi ở đây nếu cần thiết
        }
    });
}
window.addEventListener('message', function (event) {
    console.log("Received message:", event.data);
    // Kiểm tra xem thông điệp có phải là yêu cầu reload trang không
    if (event.data === 'reloadLogin') {
        // Reload lại trang
        window.location.href = baseUrl+'Home/login';
    }
});
function loginWithGoogle() {
    // Lấy kích thước của cửa sổ trình duyệt
    var screenWidth = window.screen.width;
    var screenHeight = window.screen.height;

    // Tính toán vị trí giữa của cửa sổ trình duyệt
    var popupWidth = 600;
    var popupHeight = 600;
    var leftPosition = (screenWidth - popupWidth) / 2;
    var topPosition = (screenHeight - popupHeight) / 2;

    // Mở cửa sổ pop-up ở giữa trang
    var googleLoginWindow = window.open("/loginGGFB/logingg", "_blank", "width=600,height=600,left=" + leftPosition + ",top=" + topPosition);
    if (googleLoginWindow) {
        var timer = setInterval(function () {
            if (googleLoginWindow.closed) {
                clearInterval(timer);
                console.log("Pop-up closed. Sending message to reload login...");
                // Gửi thông điệp reload khi cửa sổ pop-up đóng lại
                window.postMessage('reloadLogin', '*');
            }
        }, 500);
    } else {
        console.error("Failed to open the pop-up window.");
    }
/*    googleLoginWindow.addEventListener('beforeunload', function () {
        // Gửi thông điệp reload khi cửa sổ pop-up đóng lại
        window.postMessage('reloadLogin', '*');
    });*/
}

function loginWithFace() {
    var screenWidth = window.screen.width;
    var screenHeight = window.screen.height;

    // Tính toán vị trí giữa của cửa sổ trình duyệt
    var popupWidth = 600;
    var popupHeight = 600;
    var leftPosition = (screenWidth - popupWidth) / 2;
    var topPosition = (screenHeight - popupHeight) / 2;

    // Mở cửa sổ pop-up ở giữa trang
    var googleLoginWindow = window.open("/loginGGFB/loginfb", "_blank", "width=600,height=600,left=" + leftPosition + ",top=" + topPosition);
    if (googleLoginWindow) {
        var timer = setInterval(function () {
            if (googleLoginWindow.closed) {
                clearInterval(timer);
                console.log("Pop-up closed. Sending message to reload login...");
                // Gửi thông điệp reload khi cửa sổ pop-up đóng lại
                window.postMessage('reloadLogin', '*');
            }
        }, 500);
    } else {
        console.error("Failed to open the pop-up window.");
    }
/*    googleLoginWindow.addEventListener('beforeunload', function () {
        // Gửi thông điệp reload khi cửa sổ pop-up đóng lại
        window.opener.postMessage('reloadLogin', baseUrl+'Home/Login');
    });*/
}

function register() {
    var username = document.getElementById("nameid").value;
    var password = document.getElementById("passid").value;
    var mail = document.getElementById("mailid").value;
    var code = document.getElementById("codeid").value;
    $.ajax({
        url: baseUrl+'api/account/Register',
        method: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({ username: username, pass: password, mail: mail, code: code }),
        success: function (response) {
            $.ajax({
                url: baseUrl+'api/account',
                method: 'POST',
                contentType: 'application/json',
                data: JSON.stringify({ mail: username, pass: password }),
                success: function (response) {
                    var token = response.token;
                    // Lưu token vào localStorage hoặc session để sử dụng trong các yêu cầu sau này
                    localStorage.setItem("token", token);
                    localStorage.setItem("uid", response.userId);
                    // Thực hiện hành động sau khi đăng nhập thành công, ví dụ: chuyển hướng đến trang khác
                    window.location.href = "/";
                },
                error: function (xhr, status, error) {
                    console.error("Lỗi khi đăng nhập:", error);
                    // Xử lý lỗi ở đây nếu cần thiết
                }
            });
        },
        error: function (xhr, status, error) {
            console.error("Lỗi khi đăng nhập:", error);
            // Xử lý lỗi ở đây nếu cần thiết
        }
    });
}

function sendmail() {
    var currentDate = new Date();

    // Lấy các thành phần của ngày
    var year = currentDate.getFullYear(); // Năm
    var month = ('0' + (currentDate.getMonth() + 1)).slice(-2); // Tháng (từ 01 đến 12)
    var day = ('0' + currentDate.getDate()).slice(-2); // Ngày (từ 01 đến 31)
    var hours = ('0' + currentDate.getHours()).slice(-2); // Giờ (từ 00 đến 23)
    var minutes = ('0' + currentDate.getMinutes()).slice(-2); // Phút (từ 00 đến 59)

    // Tạo chuỗi ngày tháng theo định dạng YYYY-MM-DD HH:mm
    var formattedDate = year + '-' + month + '-' + day + ' ' + hours + ':' + minutes;
    var mail = document.getElementById("mailid").value;
    var subject = 'Đăng ký tài khoản tại Anime-Thai';
    var code = (Math.floor(Math.random() * 999999) + 1).toString().padStart(6, '0');
    var num = 1;
    // Định dạng nội dung email với một placeholder cho đồng hồ đếm ngược
    var body = '';
    body += '<p>Mã xác nhận của bạn là: ' + code + '.</p>';
    body += '<div style="text-align: center;"><img src="https://i.mailtimer.io/lTF47IQjCs.gif?start={' + formattedDate + '}" border="0" alt="mailtimer.io" style="max-width:100%;" /></div>';


    // Gửi email qua AJAX
    $.ajax({
        url: baseUrl+'api/account/sendmail',
        method: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({ toEmail: mail, subject: subject, body: body, code: code, num:num}),
        success: function (response) {
            console.log(response);
;
        },
        error: function (xhr, status, error) {
            console.error("Lỗi khi đăng nhập:", error);
            // Xử lý lỗi ở đây nếu cần thiết
        }
    });
}


function sendlat60() {
    var button = document.getElementById("sendmail");

    if (button.disabled) return; // Nếu nút đã được vô hiệu hóa, không làm gì cả

    // Thực hiện công việc gửi mail ở đây
    sendmail();

    // Vô hiệu hóa nút gửi
    button.disabled = true;

    // Bắt đầu đếm ngược
    startTimer();
}
function startTimer() {
    var timeLeft = 60; // Thời gian còn lại đếm ngược, ở đây là 60s

    // Bắt đầu đếm ngược
    timer = setInterval(function () {
        timeLeft--;

        // Cập nhật nút gửi
        document.getElementById("sendmail").innerText = "Gửi mã (" + timeLeft + "s)";

        // Kiểm tra xem thời gian còn lại có bằng 0 không
        if (timeLeft <= 0) {
            clearInterval(timer); // Dừng đếm ngược
            document.getElementById("sendmail").innerText = "Gửi mã";
            document.getElementById("sendmail").disabled = false; // Kích hoạt nút gửi lại
        }
    }, 1000); // Cứ sau mỗi giây, đếm ngược giảm đi 1 giây
}


var currentStep = 1;

function nextStep() {
    if (currentStep == 1) {
        sendmailf();
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

function sendmailf() {
    var currentDate = new Date();
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
            $('#passModalCenter').modal('hide');
        }, error: function (response) {
            console.log("Error:", response);
        }
    });
}