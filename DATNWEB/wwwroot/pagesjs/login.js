function loginUser() {
    var username = document.getElementById("username").value;
    var password = document.getElementById("password").value;

    $.ajax({
        url: 'https://localhost:7274/api/account',
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
}
window.addEventListener('message', function (event) {
    // Kiểm tra xem thông điệp có phải là yêu cầu reload trang không
    if (event.data === 'reloadLogin') {
        // Reload lại trang
        window.location.href = 'https://localhost:7274/Home/Login';
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
    googleLoginWindow.addEventListener('beforeunload', function () {
        // Gửi thông điệp reload khi cửa sổ pop-up đóng lại
        window.opener.postMessage('reloadLogin', 'https://localhost:7274/Home/Login');
    });
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
    googleLoginWindow.addEventListener('beforeunload', function () {
        // Gửi thông điệp reload khi cửa sổ pop-up đóng lại
        window.opener.postMessage('reloadLogin', 'https://localhost:7274/Home/Login');
    });
}

function register() {
    var username = document.getElementById("nameid").value;
    var password = document.getElementById("passid").value;
    var mail = document.getElementById("mailid").value;
    $.ajax({
        url: 'https://localhost:7274/api/account/Register',
        method: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({ username: username, pass: password ,mail : mail}),
        success: function (response) {
            $.ajax({
                url: 'https://localhost:7274/api/account',
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
