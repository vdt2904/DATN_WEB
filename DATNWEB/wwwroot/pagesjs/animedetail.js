let connection;
function detail(a) {
    $.ajax({
        url: 'https://localhost:7274/api/animedetail?id=' + a,
        method: 'GET',
        contentType: 'application/json',
        dataType: 'json',
        error: function (response) {
            console.log("error");
        },
        success: function (response) {
            let table = '';
            table += '<div class="col-lg-3">';
            table += '<div class="anime__details__pic set-bg" data-setbg="' + response.imageVUrl + '" style="background-image: url(' + response.imageVUrl + '); max-width: 262.5px; max-height: 440px;">';
            table += '<div class="comment"><i class="fa fa-comments"></i> ' + response.totalc + '</div>';
            table += '<div class="view"><i class="fa fa-eye"></i> ' + response.total + '</div>';
            table += '</div>';
            table += '</div>';
            table += '<div class="col-lg-9">';
            table += '<div class="anime__details__text">';
            table += '<div class="anime__details__title">';
            if (response.permission == 0) {
                table += '<h3>' + response.animeName + ' <img src = "../home/img/vip-pass.png" alt = "VIP Icon" style = "width: 40px; height: 40px;" ></h3>';
            } else {
                table += '<h3>' + response.animeName + '</h3>';
            }
            table += '<span>' + response.direc + '</span>';
            table += '</div>';
            table += '<div id="rate">'
            table += '</div>';
            table += '<p>' + response.information + '</p>';
            table += '<div class="anime__details__widget">';
            table += '<div class="row">';
            table += '<div class="col-lg-6 col-md-6">';
            table += '<ul>';
            table += '<li><span>Loại:</span> ' + (response.permission === 1 ? 'Vip' : 'Thường') + '</li>';
            table += '<li><span>Ngày phát sóng:</span> ' + convertdate(response.BroadcastSchedule) + '</li>';
            table += '<li><span>Trạng thái:</span> Đang tiến hành</li>';
            var theloai = response.genre;
            var ren = '';
            for (var i = 0; i < theloai.length; i++) {
                if (i == theloai.length - 1) {
                    ren += '<a href="/Home/category?genre=' + theloai[i].genreId + '">' + theloai[i].genreName + '</a>'
                } else {
                    ren += '<a href="/Home/category?genre=' + theloai[i].genreId + '">' + theloai[i].genreName + '</a>, '
                }
            }
            table += ' <li><span>Thể loại:</span>' + ren + '</li>';
            table += '</ul>';
            table += '</div>';
            table += '<div class="col-lg-6 col-md-6">';
            table += '<ul>';
            table += '<li><span>Đánh giá:</span> 7.31 / 1,515</li>';
            table += '<li><span>Thời lượng:</span> 24 min/ep</li>';
            table += '<li><span>Chất lượng:</span> HD</li>';
            table += '<li><span>Lượt xem:</span> ' + response.total + '</li>';
            table += '</ul>';
            table += '</div>';
            table += '</div>';
            table += '</div>';
            table += '<div class="anime__details__btn">';
            table += '<a href="#" class="follow-btn"><i class="fa fa-heart-o"></i> Follow</a>';
            table += '<a href="/home/Episode?id=' + response.animeId + '&ep='+ response.ep+'" class="watch-btn">';
            table += '<span>Watch Now</span> <i class="fa fa-angle-right"></i>';
            table += '</a>';
            table += '</div>';
            table += '</div>';
            table += '</div>';
            document.getElementById('detail').innerHTML = table;
            rate(response.animeId);
            mikelike(response.animeId);
            getreview(response.animeId);
            connection = new signalR.HubConnectionBuilder()
                .withUrl("/reviewhub/"+a)
                .build();
            connection.on("ReviewRequested", () => {
                // Gọi hàm để thực hiện lấy dữ liệu review
                rate(response.animeId);
                getreview(response.animeId);
            });

            connection.start().catch(err => console.error(err));
        },
        fail: function (response) {
            console.log("fail");
        }
    })
}

function convertdate(inputDateString) {
    var inputDate = new Date(inputDateString);
    var options = { year: 'numeric', month: 'long', day: 'numeric' };
    var outputDateString = inputDate.toLocaleDateString('en-US', options);
    return outputDateString;
}

function mikelike(a) {
    $.ajax({
        url: 'https://localhost:7274/api/animedetail/Mikelike?id=' + a,
        method: 'GET',
        contentType: 'application/json',
        dataType: 'json',
        error: function (response) {
            console.log("error");
        },
        success: function (response) {
            let table = '';
            table += '<div class="section-title">';
            table += '<h5>Bạn có thể thích...</h5>';
            table += '</div>';
            for (var i = 0; i < response.length; i++) {
                table += '<div class="product__sidebar__view__item set-bg" data-setbg="' + response[i].imageHUrl + '" style="background-image: url(' + response[i].imageHUrl + '); max-width: 360px; max-height: 190px;">';
                table += '<div class="ep">' + (response[i].maxep === -1 ? '??' : response[i].maxep) + '/' + (response[i].totalEpisode === null ? '??' : response[i].totalEpisode) + '</div>';
                table += '<div class="view"><i class="fa fa-eye"></i> ' + response[i].total + '</div>';
                if (response[i].permission == 0) {
                    table += '<h5><a href="/Home/AnimeDetail?id=' + response[i].animeId + '">' + response[i].animeName + '</a> <img src = "../home/img/vip-pass.png" alt = "VIP Icon" style = "width: 40px; height: 40px;" ></h5>';
                } else {
                    table += '<h5><a href="/Home/AnimeDetail?id=' + response[i].animeId + '">' + response[i].animeName + '</a></h5>';
                }
                table += '</div>';
            }
            document.getElementById('like').innerHTML = table;
        },
        fail: function (response) {
            console.log("fail");
        }
    })
}

function getreview(id,page) {
    page = page || 1;
    $.ajax({
        url: 'https://localhost:7274/api/animedetail/review?id='+id+'&page=' + page,
        method: 'GET',
        contentType: 'application/json',
        dataType: 'json',
        error: function (response) {
            console.log("error");
        },
        success: function (response) {
            const products = response.pagedReview; // Lấy dữ liệu sản phẩm từ response
            const len = products.length;
            let table = '';
            table += '<div class="section-title">';
            table += '<h5>Reviews</h5>';
            table += '</div>';
            for (var i = 0; i < len; i++) {
                var date = new Date(products[i].timestamp);
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
                if (products[i].rating === 1) {
                    str1 = '★☆☆☆☆';
                }
                if (products[i].rating === 2) {
                    str1 = '★★☆☆☆';
                }
                if (products[i].rating === 3) {
                    str1 = '★★★☆☆';
                }
                if (products[i].rating === 4) {
                    str1 = '★★★★☆';
                }
                if (products[i].rating === 5) {
                    str1 = '★★★★★';
                }                
                table += '<div class="anime__review__item">';
                table += '<div class="anime__review__item__pic"><img src="../Home/img/avatar.jfif" alt=""></div>';
                table += '<div class="anime__review__item__text">';
                table += '<h6>' + products[i].user + ' - <span>' + str1 + '</span> - <span>' + str + '</span></h6>';
                table += '<p>' + products[i].content + '</p>';
                table += '</div>';
                table += '</div>';

            }
            document.getElementById('review').innerHTML = table;
            renderPagination(response.paginationInfo, id);
        },
        fail: function (response) {
            console.log("fail");
        }
    })
}

function rate(id) {
    $.ajax({
        url: 'https://localhost:7274/api/animedetail/rate?id=' + id,
        method: 'GET',
        contentType: 'application/json',
        dataType: 'json',
        error: function (response) {
            console.log("error");
        },
        success: function (response) {
            var rateTb = response.totalRating / response.totalReviews;
            var nguyen = Math.floor(rateTb);
            var du = rateTb % 1;
            var tbr = 0;
            if (du > 0 && du < 0.3) {
                tbr = nguyen;
            } else if (du >= 0.3 && du < 0.7) {
                tbr = nguyen + 0.5;
            }
            else {
                tbr = nguyen + 1;
            }
            let html = '';
            html += '<div class="anime__details__rating">';
            html += '<div class="rating">';
            if (tbr % 1 == 0) {
                for (var i = 1; i <= 5; i++) {
                    if (i > tbr) {
                        html += '<a><i class="fa fa-star-o"></i></a>';
                    } else {
                        html += '<a><i class="fa fa-star"></i></a>';
                    }
                }
            } else {
                for (var i = 1; i <= 5; i++) {
                    if (i <= tbr) {
                        html += '<a><i class="fa fa-star"></i></a>';
                    } else if (i >= Math.floor(tbr) + 2) {
                        html += '<a><i class="fa fa-star-o"></i></a>';
                    } else {
                        html += '<a href="#"><i class="fa fa-star-half-o"></i></a>';
                    }
                }
            }
            html += '</div>';
            html += '<span>' + response.totalReviews + ' lượt đánh giá</span>';
            html += '</div>';
            document.getElementById('rate').innerHTML = html;           
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
        paginationHtml += '<li class="page-item"><a class="page-link" onclick="getreview(\'' + id + '\',' + (paginationInfo.currentPage - 1) + ')" style="cursor:pointer;"><span aria-hidden="true">&laquo;</span></a></li>';
    }

    // Add page links to pagination
    if (paginationInfo.totalPages > 1) {
        for (let i = 1; i <= paginationInfo.totalPages; i++) {
            paginationHtml += '<li class="page-item"><a class="page-link" onclick="getreview(\'' + id + '\',' + i + ')" style="cursor:pointer;">' + i + '</a></li>';
        }
    }

    // Display next page link
    if (paginationInfo.currentPage < paginationInfo.totalPages) {
        paginationHtml += '<li class="page-item"><a class="page-link" onclick="getreview(\'' + id + '\',' + (paginationInfo.currentPage + 1) + ')" style="cursor:pointer;"><span aria-hidden="true">&raquo;</span></a></li>';
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

    const item = {
        animeId: id,
        userId: localStorage.getItem("uid"),
        rating: document.getElementById('vrate').value,
        content: document.getElementById('idrv').value,
        timestamp: formattedDateTime
    }
    console.log(item);
    $.ajax({
        url: 'https://localhost:7274/api/animedetail/addreview',
        method: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({ animeId: id, userId: localStorage.getItem("uid"), rating: document.getElementById('vrate').value, content: document.getElementById('idrv').value, timestamp: formattedDateTime }),
        error: function (response) {
            console.log("error");
        },
        success: function (response) {
            connection.invoke("RequestReview").catch(err => console.error(err));
            getreview(id);
            document.getElementById("vrate").selectedIndex = 0; // Chọn lại option đầu tiên
            document.getElementById("idrv").value = ""; // Xóa nội dung của textarea
        },

        fail: function (response) {
            getreview(id);
            console.log("fail");
        }
    })
}           
                         
                        
                    
                    
                        
                        
                        
                      
        
        
            
                
                
                
                
                
            
            
        
        
        
            
                
                    
                        
                        
                        
                        
                       
                    
                
               
                    

                        
                        
                        
                    
                
            
        
        
            
            
                
            
        
    
 
        
        
    
