
function loginJWT(a, b) {
    $.ajax({
        url: 'https://animethai-001-site1.atempurl.com/api/account',
        method: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({ mail: a, pass: b }),
        success: function (response) {
            var token = response.token;
            // Lưu token vào localStorage hoặc session để sử dụng trong các yêu cầu sau này
            localStorage.setItem("token", token);
            localStorage.setItem("uid", response.userId);
         //   window.opener.postMessage('reloadLogin', 'https://animethai-001-site1.atempurl.com/Home/Login');
            window.close();
        },
        error: function (xhr, status, error) {
            console.error("Lỗi khi đăng nhập:", error);
            // Xử lý lỗi ở đây nếu cần thiết
        }
    });
}