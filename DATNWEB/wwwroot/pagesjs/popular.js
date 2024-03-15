function populars(page) {
    page = page || 1;
    $.ajax({
        url: 'https://localhost:7274/api/category/popular?page=' + page,
        method: 'GET',
        contentType: 'application/json',
        dataType: 'json',
        error: function (response) {
            console.log("error");
        },
        success: function (response) {
            const products = response.pagedAnimes; // Lấy dữ liệu sản phẩm từ response
            const len = products.length;
            let table = '';
            for (var i = 0; i < len; i++) {
                const genre = products[i].genres;
                table += '<div class="col-lg-4 col-md-6 col-sm-6">';
                table += '<div class="product__item">';
                table += '<div class="product__item__pic set-bg" style="background-image: url(' + products[i].imageVUrl + '); background-size: cover; width: 230px; height: 325px;">';
                table += '<div class="ep">' + products[i].maxep + ' /' + (products[i].totalEpisode == null ? '??' : products[i].totalEpisode) + '</div>';
                table += '<div class="comment"><i class="fa fa-comments"></i> ' + products[i].totalc + '</div>';
                table += '<div class="view"><i class="fa fa-eye"></i> ' + products[i].total + '</div>';
                table += '</div>';
                table += '<div class="product__item__text">';
                table += '<ul>';
                for (var j = 0; j < genre.length; j++) {
                    table += '<li>' + genre[j].genreName + '</li> ';
                }
                table += '</ul>';
                table += '<h5><a href="/Home/AnimeDetail?id=' + products[i].animeId + '">' + products[i].animeName + '</a></h5>';
                table += '</div>';
                table += '</div>';
                table += '</div>';
            }
            document.getElementById('category_product').innerHTML = table;
            renderPagination(response.paginationInfo);
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
            paginationHtml += '<a href="#" onclick="recentlys(' + i + ')">' + i + '</a>';
        }
    }

    // Display previous page link
    if (paginationInfo.currentPage > 1) {
        paginationHtml += '<a href="#" onclick="trendings(' + (paginationInfo.currentPage - 1) + ')"><i class="fa fa-angle-double-left"></i></a>';
    }

    // Display next page link
    if (paginationInfo.currentPage < paginationInfo.totalPages) {
        paginationHtml += '<a href="#" onclick="trendings(' + (paginationInfo.currentPage + 1) + ')"><i class="fa fa-angle-double-right"></i></a>';
    }

    // Display pagination
    document.getElementById('pagination').innerHTML = paginationHtml;
}