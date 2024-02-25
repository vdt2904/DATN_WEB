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
            table += '<h3>' + response.animeName + '</h3>';
            table += '<span>' + response.direc + '</span>';
            table += '</div>';
            table += '<div class="anime__details__rating">';
            table += '<div class="rating">';
            table += '<a href="#"><i class="fa fa-star"></i></a>';
            table += '<a href="#"><i class="fa fa-star"></i></a>';
            table += '<a href="#"><i class="fa fa-star"></i></a>';
            table += '<a href="#"><i class="fa fa-star"></i></a>';
            table += '<a href="#"><i class="fa fa-star-half-o"></i></a>';
            table += '</div>';
            table += '<span>1.029 Votes</span>';
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
            table += '<a href="/home/Watch?id=' + response.animeId + '" class="watch-btn">';
            table += '<span>Watch Now</span> <i class="fa fa-angle-right"></i>';
            table += '</a>';
            table += '</div>';
            table += '</div>';
            table += '</div>';
            document.getElementById('detail').innerHTML = table;
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
                table += '<h5><a href="/Home/AnimeDetail?id=' + response[i].animeId + '">' + response[i].animeName + '</a></h5>';
                table += '</div>';
            }
            document.getElementById('like').innerHTML = table;
        },
        fail: function (response) {
            console.log("fail");
        }
    })
}
    
        
            
                         
                        
                    
                    
                        
                        
                        
                      
        
        
            
                
                
                
                
                
            
            
        
        
        
            
                
                    
                        
                        
                        
                        
                       
                    
                
               
                    

                        
                        
                        
                    
                
            
        
        
            
            
                
            
        
    
 
        
        
    
