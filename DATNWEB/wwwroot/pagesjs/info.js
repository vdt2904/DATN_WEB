function info() {
    $.ajax({
        url: 'https://localhost:7274/api/infouser',
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
            }
            table += '</span>';
            table += '<div class="button-change">Thay đổi</div>';
            table += '</div>';
            table += '</div>';
            table += '<div class="col-12">';
            table += '<div class="item">';
            table += '<span class="label">';
            table += 'Mật khẩu';
            table += '<span class="info" data-password="' + response.password + '">&nbsp;&nbsp;******</span>';
            table += '</span>';
            table += '<div class="button-change">Thay đổi</div>';
            table += '</div>';
            table += '</div>';
            table += '</div>';
            table += '</div>';
            table += '';
            document.getElementById('infou').innerHTML = table;
        },
        fail: function (response) {
            console.log("fail");
        }
    })
}

function logout() {
    $.ajax({
        url: 'https://localhost:7274/api/infouser/logout',
        method: 'POST',
        contentType: 'application/json',
        dataType: 'json',        
        success: function (response) {
            console.log("Logout successful");
            
        },error: function (response) {
            console.log("Error:", response);
            localStorage.removeItem("token");
            localStorage.removeItem("uid");
            window.location.href = "/home/login";
        }
    });
}

