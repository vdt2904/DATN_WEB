function blogs(a) {
    a = (typeof a !== 'undefined') ? a : null;
    var urls = '';
    if (a == null) {
        urls = baseUrl+'api/blog';
    } else {
        urls = baseUrl+'api/blog?id=' + a;
    }
    $.ajax({
        url: urls,
        method: 'GET',
        contentType: 'application/json',
        dataType: 'json',
        error: function (response) {
            console.log("error");
        },
        success: function (response) {
            const season = response.sea;
            var inputDateString = season.postingDate;
            var inputDate = new Date(inputDateString);
            var options = { year: 'numeric', month: 'long', day: 'numeric' };
            var outputDateString = inputDate.toLocaleDateString('en-US', options);
            const animes = response.animes;
            let table = '';
            table += '<div class="col-lg-8">';
            table += '<div class="blog__details__title">';
            table += '<h6><span>- ' + outputDateString + '</span></h6>';
            table += '<h2>Anime for Beginners: ' + season.seasonName + '</h2>';
            table += '<div class="blog__details__social">';
            table += '<a class="facebook" onclick="shareOnFacebook()"><i class="fa fa-facebook-square"></i> Facebook</a>';
            table += '<a href="#" class="pinterest"><i class="fa fa-pinterest"></i> Pinterest</a>';
            table += '<a href="#" class="linkedin"><i class="fa fa-linkedin-square"></i> Linkedin</a>';
            table += '<a href="#" class="twitter"><i class="fa fa-twitter-square"></i> Twitter</a>';
            table += '</div>';
            table += '</div>';
            table += '</div>';
            table += '<div class="col-lg-12">';
            table += '<div class="blog__details__pic">';
            table += '<img src="' + season.imageUrl + '" alt="">';
            table += '</div>';
            table += '</div>';
            table += '<div class="col-lg-8">';
            table += '<div class="blog__details__content">';
            table += '<div class="blog__details__text">';
            table += '<p>' + season.information + '</p>';
            table += '</div>';
            for (var i = 0; i < animes.length; i++) {
                table += '<div class="blog__details__item__text">';
                table += '<h4>' + (i+1) + '.' + animes[i].animeName + '</h4>';
                table += '<img src="' + animes[i].imageHUrl + '" alt="">';
                table += '<p>' + animes[i].information + '</p>';
                table += '</div>';
            }
            table += '<div class="blog__details__tags">';
            table += '<a href="/home/blogdetail?season=' + season.seasonId + '">Chi tiết</a>';
            table += '</div>';
            table += '<div class="blog__details__btns" id="btn">';
            table += '</div>';
            table += '</div>';
            document.getElementById('blogs').innerHTML = table;
            renderpages(response.sea1, response.sea2);
        },
        fail: function (response) {
            console.log("fail");
        }
    });
}

function renderpages(idt, ids) {
    var table = ''; 
    table += '<div class="row">';
    table += '<div class="col-lg-6">';
    table += '<div class="blog__details__btns__item">';
    if (idt === null) {
        table += '<h5 style="color: grey;"><span class="arrow_left"></span> Trước</h5>';
    } else {
        table += '<h5 style="color: white;"><a onclick="blogs(\'' + idt + '\')"><span class="arrow_left"></span> Trước</a></h5>';
    }
    table += '</div>';
    table += '</div>';
    table += '<div class="col-lg-6">';
    table += '<div class="blog__details__btns__item next__btn">';
    if (ids === null) {
        table += '<h5 style="color: grey;">Sau <span class="arrow_right"></span></h5>';
    } else {
        table += '<h5 style="color: white;"><a onclick="blogs(\'' + ids + '\')">Sau <span class="arrow_right"></span></a></h5>';
    }
    table += '</div>';
    table += '</div>';
    table += '</div>';
    document.getElementById('btn').innerHTML = table;
}



function shareOnFacebook() {
    var currentPageUrl = window.location.href;
    console.log(currentPageUrl);
    FB.ui({
        method: 'share',
        href: currentPageUrl,
    }, function (response) {
        if (response && !response.error_message) {
            alert('Chia sẻ trang hiện tại thành công');
        } else {
            alert('Chia sẻ trang hiện tại thất bại');
        }
    });
}







































