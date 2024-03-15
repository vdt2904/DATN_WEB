function loginUser() {
    var username = document.getElementById("username").value;
    var password = document.getElementById("password").value;

    $.ajax({
        url: 'https://localhost:7274/api/account',
        method: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({ username: username, pass: password }),
        success: function (response) {
            var token = response.token;
            // Lưu token vào localStorage hoặc session để sử dụng trong các yêu cầu sau này
            localStorage.setItem("token", token);
            console.log("Đăng nhập thành công!");
            // Thực hiện hành động sau khi đăng nhập thành công, ví dụ: chuyển hướng đến trang khác
            window.location.href = "/";
        },
        error: function (xhr, status, error) {
            console.error("Lỗi khi đăng nhập:", error);
            // Xử lý lỗi ở đây nếu cần thiết
        }
    });
}

