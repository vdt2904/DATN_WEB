const searchInput = document.getElementById('search-input');
function clearSearchInput() {
    // Lấy tham chiếu đến ô nhập liệu
    // Đặt giá trị của ô nhập liệu thành chuỗi rỗng
    searchInput.value = '';
    let table = '';
    document.getElementById('suggestsearch').innerHTML = table;
    $('.h-25').removeClass('h-25').addClass('h-100');
}
// Lắng nghe sự kiện nhập liệu trên input
searchInput.addEventListener('input', function () {
    // Lấy giá trị nhập liệu từ input
    const inputValue = searchInput.value;

    // Gọi đến API để tìm kiếm dựa trên giá trị nhập liệu
    suggest(inputValue);
});
function suggest(a) {
    $.ajax({
        url: baseUrl+'api/Search?keyword=' + a,
        method: 'GET',
        contentType: 'application/json',
        dataType: 'json',
        error: function (response) {
            console.log("error");

            // Kiểm tra nếu lỗi là "Tối thiểu 3 ký tự"
            if (response.responseText === "Tối thiểu 3 ký tự") {
                // Hiển thị thông báo lỗi về độ dài tối thiểu
                let errorMessage = '<span style="color: white; display: block; text-align: center;">Tối thiểu 3 ký tự</span>';
                document.getElementById('suggestsearch').innerHTML = errorMessage;
            } else {
                // Nếu không, hiển thị thông báo "Không tìm thấy kết quả"
                let noResultMessage = '<span style="color: white; display: block; text-align: center;">Không tìm thấy kết quả</span>';
                document.getElementById('suggestsearch').innerHTML = noResultMessage;
            }
        },
        success: function (response) {
            const len = response.length;
            let table = '';
            table += '<ul>';
            for (var i = 0; i < len; i++) {
                table += '<li>';
                table += '<a href="../home/AnimeDetail?id=' + response[i].animeId +'">';
                table += '<img class="lazy" src="' + response[i].imageVUrl + '" data-original="' + response[i].imageVUrl +'">';
                table += '<div>';
                if (response[i].permission == 0) {
                    table += '<h4><img src = "../home/img/vip-pass.png" alt = "VIP Icon" style = "width: 40px; height: 40px;"> ' + response[i].animeName + '</h4>';
                } else {
                    table += '<h4>' + response[i].animeName + '</h4>';
                }
                table += '<h6>';
                table += '<ul>';
                table += '<li><i>Tập ' + (response[i].maxEpisode == -1 ? '--' : response[i].maxEpisode) + '</i></li>';
                var genre = response[i].genres;
                var genres = "";
                for (var j = 0; j < genre.length; j++) {
                    if (j == 0) {
                        genres = genre[j].genreName;
                    }else{
                        genres = genres + "," + genre[j].genreName;
                    }
                }
                table += '<li><i>'+genres+'</i></li>';
                table += '</ul>';
                table += '</h6>';
                table += '</div>';
                table += '</a>';
                table += '</li>';
            }
            table += '</ul>';
            document.getElementById('suggestsearch').innerHTML = table;
        },
        fail: function (response) {

            console.log("fail");
        }
    })
}
function handleKeyPress(event) {
    // Kiểm tra xem phím người dùng nhấn có phải là phím Enter không (mã phím 13)
    if (event.key === "Enter") {
        event.preventDefault();
        // Lấy giá trị nhập liệu từ ô tìm kiếm
        const keyword = document.getElementById('search-input').value;

        // Kiểm tra nếu từ khóa có đủ điều kiện (ví dụ: ít nhất 3 ký tự)
        if (keyword.length >= 3) {
            // Chuyển hướng đến trang tìm kiếm với từ khóa được nhập
            window.location.href = baseUrl + "home/search?keyword=" + keyword;
        } else {
            // Nếu từ khóa không đạt yêu cầu, có thể hiển thị thông báo hoặc thực hiện hành động khác
            console.log('Please enter at least 3 characters for search');
        }
    }
}
