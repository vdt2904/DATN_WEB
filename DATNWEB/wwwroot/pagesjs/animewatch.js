function watch(a, b) {
    var urls = '';
    if (b == null) {
        urls = 'https://localhost:7274/api/AnimeWatching?aid=' + a;
    } else {
        urls = 'https://localhost:7274/api/AnimeWatching?aid='+a+'&ep='+b;
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
            let table = '';
            table += '<div class="col-lg-12">';
            table += '<div class="anime__video__player">';
            table += '<video id="player" playsinline controls poster="' + response.backgroundImageUrl + '" style="width:100%;max-width: 1140px; max-height: 654.06px; ">';
            table += '<source src="' + response.videoUrl + '" type="video/mp4" />';
            table += '<track kind="captions" label="English captions" src="#" srclang="en" default />';
            table += '</video>';
            table += '</div>';
            table += '<div class="anime__details__episodes">';
            table += '<div class="section-title">';
            table += '<h5>List Name</h5>';
            table += '</div>';
            var eps = response.e;
            for (var i = 0; i < eps.length; i++) {
                if (eps[i] == response.epside) {
                   
                    table += '<a onclick="watch(\'' + response.animeId + '\', \'' + eps[i] + '\')" style = "color: blue;">Ep ' + eps[i] + '</a>';
                } else {
                    table += '<a onclick="watch(\'' + response.animeId + '\', \'' + eps[i] + '\')">Ep ' + eps[i] + '</a>';
                }
            }
            console.log(response.epside)
            table += '</div>';
            table += '</div>';
            document.getElementById('video').innerHTML = table;
        },
        fail: function (response) {
            console.log("fail");
        }
    })
}





    
        

            
        
    
    
        
            
        
        
    
