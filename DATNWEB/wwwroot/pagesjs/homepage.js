﻿function advertisement() {
    $.ajax({
        url: 'https://localhost:7274/api/homepage',
        method: 'GET',
        contentType: 'json',
        dataType: 'json',
        error: function (response) {
            console.log("error");
        },
        success: function (response) {
     
            const len = response.length;
            let table = '<div class="hero__slider owl-carousel">';
            for (var i = len - 1; i >= 0; --i) {
                table = table + '<div class="hero__items set-bg" style="background-image: url(' + response[i].img + '); background-size: cover; width: 1140px; height: 579px;">';
                table = table + '<div class="row">';
                table = table + '<div class="col-lg-6">';
                table = table + '<div class="hero__text">';
                const genres = response[i].genreid;
                for (var j = 0; j < genres.length; j++) {
                    table += '<div class="label">' + genres[j].genreName + '</div> ';
                }            
                table = table + '<h2>' + response[i].animeName + '</h2>';
                table = table + '<p>' + response[i].info + '</p>';
                table = table + '<a href="#"><span>Watch Now</span> <i class="fa fa-angle-right"></i></a>';
                table = table + '</div>';
                table = table + '</div>';
                table = table + '</div>';
                table = table + '</div>';
            }
            table = table + '</div>'
            document.getElementById('advertis').innerHTML = table;
            $('.owl-carousel').owlCarousel({
                loop: true,
                margin: 10,
                nav: true,
                items: 1,
                autoplay: true, // Tự động chạy
                autoplayTimeout: 5000, // Thời gian giữa các lần chuyển động (miliseconds)
            });

        },
        fail: function (response) {
            console.log("fail");
        }
    })
}
function topday() {
    $.ajax({
        url: 'https://localhost:7274/api/homepage/topday',
        method: 'GET',
        contentType: 'application/json',
        dataType: 'json',
        error: function (response) {
            console.log("error");
        },
        success: function (response) {
            const len = response.length;
            let table = '';
            for (var i = 0; i < len; i++) {
                table += '<div class="product__sidebar__view__item set-bg" style="background-image: url(' + response[i].imageHUrl + '); background-size: cover; width: 360px; height: 190px;">';
                table += '<div class="ep">' + response[i].maxep + ' /' + (response[i].totalEpisode == null ? '??' : response[i].totalEpisode) + '</div>';
                table += '<div class="view"><i class="fa fa-eye"></i>' + response[i].total + '</div>';
                table += '<h5><a href="#">' + response[i].animeName + '</a></h5>';
                table += '</div>';
            }
            document.getElementById('topview').innerHTML = table;
        },
        fail: function (response) {
            console.log("fail");
        }
    })
}
function topweek() {
    $.ajax({
        url: 'https://localhost:7274/api/homepage/topweek',
        method: 'GET',
        contentType: 'application/json',
        dataType: 'json',
        error: function (response) {
            console.log("error");
        },
        success: function (response) {
            const len = response.length;
            let table = '';
            for (var i = 0; i < len; i++) {
                table += '<div class="product__sidebar__view__item set-bg" style="background-image: url(' + response[i].imageHUrl + '); background-size: cover; width: 360px; height: 190px;">';
                table += '<div class="ep">' + response[i].maxep + ' /' + (response[i].totalEpisode == null ? '??' : response[i].totalEpisode) + '</div>';
                table += '<div class="view"><i class="fa fa-eye"></i>' + response[i].total + '</div>';
                table += '<h5><a href="#">' + response[i].animeName + '</a></h5>';
                table += '</div>';
            }
            document.getElementById('topview').innerHTML = table;
        },
        fail: function (response) {
            console.log("fail");
        }
    })
}
function topyear() {
    $.ajax({
        url: 'https://localhost:7274/api/homepage/topyear',
        method: 'GET',
        contentType: 'application/json',
        dataType: 'json',
        error: function (response) {
            console.log("error");
        },
        success: function (response) {
            const len = response.length;
            let table = '';
            for (var i = 0; i < len; i++) {
                table += '<div class="product__sidebar__view__item set-bg" style="background-image: url(' + response[i].imageHUrl + '); background-size: cover; width: 360px; height: 190px;">';
                table += '<div class="ep">' + response[i].maxep + ' /' + (response[i].totalEpisode == null ? '??' : response[i].totalEpisode) + '</div>';
                table += '<div class="view"><i class="fa fa-eye"></i>' + response[i].total + '</div>';
                table += '<h5><a href="#">' + response[i].animeName + '</a></h5>';
                table += '</div>';
            }
            document.getElementById('topview').innerHTML = table;
        },
        fail: function (response) {
            console.log("fail");
        }
    })
}
function topmonth() {
    $.ajax({
        url: 'https://localhost:7274/api/homepage/topmonth',
        method: 'GET',
        contentType: 'application/json',
        dataType: 'json',
        error: function (response) {
            console.log("error");
        },
        success: function (response) {
            const len = response.length;
            let table = '';
            for (var i = 0; i < len; i++) {
                table += '<div class="product__sidebar__view__item set-bg" style="background-image: url(' + response[i].imageHUrl + '); background-size: cover; width: 360px; height: 190px;">';
                table += '<div class="ep">' + response[i].maxep + ' /' + (response[i].totalEpisode == null ? '??' : response[i].totalEpisode) + '</div>';
                table += '<div class="view"><i class="fa fa-eye"></i>' + response[i].total + '</div>';
                table += '<h5><a href="#">' + response[i].animeName + '</a></h5>';
                table += '</div>';
            }
            document.getElementById('topview').innerHTML = table;
        },
        fail: function (response) {
            console.log("fail");
        }
    })
}
function newcomment() {
    $.ajax({
        url: 'https://localhost:7274/api/homepage/newcomment',
        method: 'GET',
        contentType: 'application/json',
        dataType: 'json',
        error: function (response) {
            console.log("error");
        },
        success: function (response) {
            const len = response.length;
            let table = '';
            for (var i = 0; i < len; i++) {
                const genre = response[i].genres;
                
                table += '<div class="product__sidebar__comment__item">';
                table += '<div class="product__sidebar__comment__item__pic">';
                table += '<img src="' + response[i].imageVUrl + '" alt="" style="max-width: 90px; max-height: 130px;">';
                table += '</div>';
                table += '<div class="product__sidebar__comment__item__text">';
                table += '<ul>';
                for (var j = 0; j < genre.length; j++) {
                    table += '<li>' + genre[j].genreName + '</li> ';
                }
                table += '</ul>';
                table += '<h5><a href="#">' + response[i].animeName + '</a></h5>';
                table += '<span><i class="fa fa-eye"></i> ' + response[i].totalViews + ' Viewes</span>';
                table += '</div>';
                table += '</div>';
            }
            document.getElementById('newcmt').innerHTML = table;
        },
        fail: function (response) {
            console.log("fail");
        }
    })
}
function trending() {
    $.ajax({
        url: 'https://localhost:7274/api/homepage/trending',
        method: 'GET',
        contentType: 'application/json',
        dataType: 'json',
        error: function (response) {
            console.log("error");
        },
        success: function (response) {
            const len = response.length;
            let table = '';
            for (var i = 0; i < len; i++) {
                const genre = response[i].genres;
                table += '<div class="col-lg-4 col-md-6 col-sm-6">';
                table += '<div class="product__item">';
                table += '<div class="product__item__pic set-bg" style="background-image: url(' + response[i].imageVUrl + '); background-size: cover; width: 230px; height: 325px;">';
                table += '<div class="ep">' + response[i].maxep + ' /' + (response[i].totalEpisode == null ? '??' : response[i].totalEpisode) + '</div>';
                table += '<div class="comment"><i class="fa fa-comments"></i> ' + response[i].totalc + '</div>';
                table += '<div class="view"><i class="fa fa-eye"></i> ' + response[i].total + '</div>';
                table += '</div>';
                table += '<div class="product__item__text">';
                table += '<ul>';
                for (var j = 0; j < genre.length; j++) {
                    table += '<li>' + genre[j].genreName + '</li> ';
                }
                table += '</ul>';
                table += '<h5><a href="#">' + response[i].animeName + '</a></h5>';
                table += '</div>';
                table += '</div>';
                table += '</div>';
            }
            document.getElementById('trending').innerHTML = table;
        },
        fail: function (response) {
            console.log("fail");
        }
    })
}
function popular() {
    $.ajax({
        url: 'https://localhost:7274/api/homepage/popular',
        method: 'GET',
        contentType: 'application/json',
        dataType: 'json',
        error: function (response) {
            console.log("error");
        },
        success: function (response) {
            const len = response.length;
            let table = '';
            for (var i = 0; i < len; i++) {
                const genre = response[i].genres;
                table += '<div class="col-lg-4 col-md-6 col-sm-6">';
                table += '<div class="product__item">';
                table += '<div class="product__item__pic set-bg" style="background-image: url(' + response[i].imageVUrl + '); background-size: cover; width: 230px; height: 325px;">';
                table += '<div class="ep">' + response[i].maxep + ' /' + (response[i].totalEpisode == null ? '??' : response[i].totalEpisode) + '</div>';
                table += '<div class="comment"><i class="fa fa-comments"></i> ' + response[i].totalc + '</div>';
                table += '<div class="view"><i class="fa fa-eye"></i> ' + response[i].total + '</div>';
                table += '</div>';
                table += '<div class="product__item__text">';
                table += '<ul>';
                for (var j = 0; j < genre.length; j++) {
                    table += '<li>' + genre[j].genreName + '</li> ';
                }
                table += '</ul>';
                table += '<h5><a href="#">' + response[i].animeName + '</a></h5>';
                table += '</div>';
                table += '</div>';
                table += '</div>';
            }
            document.getElementById('popular').innerHTML = table;
        },
        fail: function (response) {
            console.log("fail");
        }
    })
}
function recently() {
    $.ajax({
        url: 'https://localhost:7274/api/homepage/recently',
        method: 'GET',
        contentType: 'application/json',
        dataType: 'json',
        error: function (response) {
            console.log("error");
        },
        success: function (response) {
            const len = response.length;
            let table = '';
            for (var i = 0; i < len; i++) {
                const genre = response[i].genres;
                table += '<div class="col-lg-4 col-md-6 col-sm-6">';
                table += '<div class="product__item">';
                table += '<div class="product__item__pic set-bg" style="background-image: url(' + response[i].imageVUrl + '); background-size: cover; width: 230px; height: 325px;">';
                table += '<div class="ep">' + response[i].maxep + ' /' + (response[i].totalEpisode == null ? '??' : response[i].totalEpisode) + '</div>';
                table += '<div class="comment"><i class="fa fa-comments"></i> ' + response[i].totalc + '</div>';
                table += '<div class="view"><i class="fa fa-eye"></i> ' + response[i].total + '</div>';
                table += '</div>';
                table += '<div class="product__item__text">';
                table += '<ul>';
                for (var j = 0; j < genre.length; j++) {
                    table += '<li>' + genre[j].genreName + '</li> ';
                }
                table += '</ul>';
                table += '<h5><a href="#">' + response[i].animeName + '</a></h5>';
                table += '</div>';
                table += '</div>';
                table += '</div>';
            }
            document.getElementById('recently').innerHTML = table;
        },
        fail: function (response) {
            console.log("fail");
        }
    })
}





    
        

            
            

        
        
            




        
    
    
        

        
        


    
        
            
                
                
             