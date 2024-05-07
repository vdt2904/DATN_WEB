function package() {
    $.ajax({
        url: 'https://localhost:7274/api/package',
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

                table += '<div class="col-12 col-sm-6 col-md-4 col-lg-3">';
                table += '<div class="card border-success mb-3 h-100">';
                table += '<div class="card-header bg-transparent border-success">' + response[i].packageName + '</div>';
                table += '<div class="card-body text-success">';
                table += '<h5 class="card-title">Mô tả:</h5>';
                table += '<p class="card-text">' + response[i].description + '</p >';
                table += '</div>';
                table += '<div class="card-footer bg-transparent border-success"><a href="/home/pay?id=' + response[i].packageId + '">Mua gói</a></div>';
                table += '</div>';
                table += '</div>';

            }
            document.getElementById('ps').innerHTML = table;
        },
        fail: function (response) {
            console.log("fail");
        }
    })}
    
        
        
            
            
        
        
    