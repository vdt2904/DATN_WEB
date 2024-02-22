function blogdetail(a) {
    $.ajax({
        url: 'https://localhost:7274/api/blogdetail?id=' + a,
        method: 'GET',
        contentType: 'application/json',
        dataType: 'json',
        error: function (response) {
            console.log("error");
        },
        success: function (response) {
            let table = '';
            var a = response.length;
            var chunkSize = Math.ceil(a / 2);
            var firstPart = response.slice(0, chunkSize);
            var secondPart = response.slice(chunkSize);
            table += '<div class="col-lg-6">';
            table += '<div class="row">';
            for (var i = 0; i < firstPart.length; i++) {
                if (i % 3 == 0) {
                    table += '<div class="col-lg-12">';
                    table += '<div class="blog__item set-bg" data-setbg="' + firstPart[i].imageHUrl + '" style="background-image: url(' + firstPart[i].imageHUrl + '); max-width: 575px; max-height: 580px;">';
                    table += '<div class="blog__item__text">';
                    table += '<p><span class="icon_calendar"></span> ' + convertdate(firstPart[i].broadcastSchedule) + '</p>';
                    table += '<h4><a href="#">' + firstPart[i].animeName + '</a></h4>';
                    table += '</div>';
                    table += '</div>';
                    table += '</div>';
                } else {
                    table += '<div class="col-lg-6 col-md-6 col-sm-6">';
                    table += '<div class="blog__item set-bg" data-setbg="' + firstPart[i].imageHUrl + '" style="background-image: url(' + firstPart[i].imageHUrl + '); max-width: 292.5px; max-height: 295px;">';
                    table += '<div class="blog__item__text">';
                    table += '<p><span class="icon_calendar"></span> ' + convertdate(firstPart[i].broadcastSchedule) + '</p>';
                    table += '<h4><a href="#">' + firstPart[i].animeName + '</a></h4>';
                    table += '</div>';
                    table += '</div>';
                    table += '</div>';
                }
            }
            table += '</div>';
            table += '</div>'; 
            table += '<div class="col-lg-6">';
            table += '<div class="row">';
            for (var i = 0; i < secondPart.length; i++) {
                if ((i+1) % 3 == 0) {
                    table += '<div class="col-lg-12">';
                    table += '<div class="blog__item set-bg" data-setbg="' + secondPart[i].imageHUrl + '" style="background-image: url(' + secondPart[i].imageHUrl + '); max-width: 575px; max-height: 580px;">';
                    table += '<div class="blog__item__text">';
                    table += '<p><span class="icon_calendar"></span> ' + convertdate(secondPart[i].broadcastSchedule) + '</p>';
                    table += '<h4><a href="#">' + secondPart[i].animeName + '</a></h4>';
                    table += '</div>';
                    table += '</div>';
                    table += '</div>';
                } else {
                    table += '<div class="col-lg-6 col-md-6 col-sm-6">';
                    table += '<div class="blog__item set-bg" data-setbg="' + secondPart[i].imageHUrl + '" style="background-image: url(' + secondPart[i].imageHUrl + '); max-width: 292.5px; max-height: 295px;">';
                    table += '<div class="blog__item__text">';
                    table += '<p><span class="icon_calendar"></span> ' + convertdate(secondPart[i].broadcastSchedule) + '</p>';
                    table += '<h4><a href="#">' + secondPart[i].animeName + '</a></h4>';
                    table += '</div>';
                    table += '</div>';
                    table += '</div>';
                }
            }
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


    
        
            
            
        
