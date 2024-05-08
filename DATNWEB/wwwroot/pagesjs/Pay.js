function getprice(id,a) {
    $.ajax({
        url: 'https://localhost:7274/api/pay?id='+id,
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
                var price = response[i].price.toLocaleString('vi-VN', { style: 'currency', currency: 'VND' });
                table += '<div class="PayPlan p-3 relative cursor-pointer ">';
                if (i == 0) {
                    table += '<input type="radio" id="checkbox" name="checkbox" onclick="getpayinfo(' + response[i].id + ','+a+')" value="'+response[i].id+'" checked />';
                    getpayinfo(response[i].id,a);
                } else {
                    table += '<input type="radio" id="checkbox" name="checkbox" onclick="getpayinfo(' + response[i].id + ',' + a + ')" value="' + response[i].id +'" />';
                }               
                table += '<div class="PayPlan__Name font-weight-semibold text-sm">' + response[i].usedTime + ' Tháng</div>';
                table += '<div class="PayPlan__Price">';
                table += '<span class="PayPlan__Amount">' + price + '</span>';
                table += '</div>';
                table += '</div>';
            }
            document.getElementById('price').innerHTML = table;
        },
        fail: function (response) {
            console.log("fail");
        }
    })
}
function getpayinfo(id,a) {
    $.ajax({
        url: 'https://localhost:7274/api/pay/infopay?id=' + id,
        method: 'GET',
        contentType: 'application/json',
        dataType: 'json',
        error: function (response) {
            console.log("error");
        },
        success: function (response) {
            var info = response.info;
            var pay = response.pay;
            var pack = response.package;
            let table = '';
            table += '<h4 class="font-weight-bold mb-2 text-lg">Thông tin thanh toán</h4>';
            // tài khoản
            table += '<div class="PayTable">';
            table += '<table>';
            table += '<tbody>';
            table += '<tr>';
            table += '<td>Tài khoản</td>';
            table += '<td>' + info.email + '</td>';
            table += '</tr>';
            table += '</tbody>';
            table += '</table>';
            table += '</div>';
            table += '<div class="PaySeperateLine"></div>';
            //thông tin gói
            table += '<div class="PayTable">';
            table += '<table>';
            table += '<tbody>';
            table += '<tr>';
            table += '<td>Tên gói</td>';
            table += '<td>' + response.packageName + '</td>';
            table += '</tr>';
            table += '<tr>';
            table += '<td>Thời hạn gói</td>';
            table += '<td>' + pay.usedTime + ' tháng</td>';
            table += '</tr>';
            table += '</tbody>';
            table += '</table>';
            table += '</div>';
            table += '<div class="PaySeperateLine"></div>';
            //giá
            price = pay.price.toLocaleString('vi-VN', { style: 'currency', currency: 'VND' });
            table += '<div class="PayTable">';
            table += '<table>';
            table += '<tbody>';
            table += '<tr>';
            table += '<td>Giá gói</td>';
            table += '<td>' + price + '</td>';
            table += '</tr>';
            table += '</tbody>';
            table += '</table>';
            table += '</div>';
            table += '<div class="PaySeperateLine"></div>';
            // tổng
            table += '<div class="PayTable">';
            table += '<table>';
            table += '<tbody>';
            table += '<tr>';
            table += '<td>Tổng thanh toán:</td>';
            table += '<td><span id="amount_price" class="total">' + price + '</span></td>';
            table += '</tr>';
            table += '</tbody>';
            table += '</table>';
            table += '</div>';
            table += '<div class="PaySeperateLine"></div>';
            //button
            table += '<div class="PayTable">';
            table += '<div class="text-center mt-4">';
            table += '<a id="btn_payment_submit" href="/home/payment?a='+a+'&b=' + pay.id + '" type="button" class="btn font-weight-semibold rounded-btn px-5 py-2 btn-orange btn-primary">Thanh toán</a>';
            table += '</div>';
            document.getElementById('inf').innerHTML = table;
        },
        fail: function (response) {
            console.log("fail");
        }
    })
}
// Lắng nghe sự kiện onclick cho tất cả các checkbox có name là "checkboxp"
function check() {
    var checkboxes = document.querySelectorAll('input[type="radio"][name="checkboxp"]');
    // Lắng nghe sự kiện onclick cho tất cả các checkbox có name là "checkbox"
    var checkboxe = document.querySelectorAll('input[type="radio"][name="checkbox"]');
    var selectedCheckbox = getCheckboxValue(checkboxes);
    var selectedCheckbox1 = getCheckboxValue1(checkboxe);
    getpayinfo(selectedCheckbox1, selectedCheckbox);
}
function getCheckboxValue(checkboxes) {
    var selectedCheckboxValue = ""; // Biến để lưu trữ giá trị của checkbox được chọn
    // Duyệt qua tất cả các checkbox
    checkboxes.forEach(function (checkbox) {
        // Kiểm tra xem checkbox đó có được chọn không
        if (checkbox.checked) {
            // Nếu checkbox được chọn, gán giá trị của nó cho biến selectedCheckboxValue
            selectedCheckboxValue = checkbox.value;
        }
    });
    // Trả về giá trị của checkbox được chọn
    return selectedCheckboxValue;
}

function getCheckboxValue1(checkboxe) {
    var selectedCheckboxValue = ""; // Biến để lưu trữ giá trị của checkbox được chọn
    // Duyệt qua tất cả các checkbox
    checkboxe.forEach(function (checkbox) {
        // Kiểm tra xem checkbox đó có được chọn không
        if (checkbox.checked) {
            // Nếu checkbox được chọn, gán giá trị của nó cho biến selectedCheckboxValue
            selectedCheckboxValue = checkbox.value;
        }
    });
    // Trả về giá trị của checkbox được chọn
    return selectedCheckboxValue;
}



        

   
        
            
        
         
                
                
           
 
    

        
    
    
        
            
        
    