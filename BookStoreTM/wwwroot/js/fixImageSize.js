document.addEventListener("DOMContentLoaded", function () {
    //// Lấy tất cả các hình ảnh có class 'img-full'
    //var images = document.querySelectorAll('.img-full');

    //// Thiết lập kích thước cố định cho mỗi hình ảnh
    //images.forEach(function (image) {
    //    image.style.width = '100%'; // Đặt chiều rộng
    //    image.style.height = '300px'; // Đặt chiều cao cố định
    //    image.style.objectFit = 'cover'; // Đảm bảo hình ảnh bao phủ toàn bộ khung mà không bị méo
    //});
    var oldPrices = document.querySelectorAll('.old-price');

    // Thiết lập margin-left cho mỗi phần tử
    oldPrices.forEach(function (price) {
        price.style.marginLeft = '0px';
    });
});

