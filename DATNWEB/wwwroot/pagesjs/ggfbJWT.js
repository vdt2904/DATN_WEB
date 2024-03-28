﻿function loginJWT(a,b) {
    $.ajax({
        url: 'https://localhost:7274/api/account',
        method: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({ username: a, pass: b }),
        success: function (response) {
            var token = response.token;
            // Lưu token vào localStorage hoặc session để sử dụng trong các yêu cầu sau này
            localStorage.setItem("token", token);
            localStorage.setItem("uid", response.userId);
            window.close();
            window.opener.postMessage('reloadLogin', 'https://localhost:7274/Home/Login');
        },
        error: function (xhr, status, error) {
            console.error("Lỗi khi đăng nhập:", error);
            // Xử lý lỗi ở đây nếu cần thiết
        }
    });
}