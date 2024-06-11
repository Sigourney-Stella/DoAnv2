$(document).ready(function () {
    //giảm
    $('.cart-plus-minus .fa-minus').click(function () {
        let qty = parseInt($(this).parent().siblings('.cart-plus-minus-box').val()); // lấy giá trị ô input
        let price = parseFloat($(this).closest('td').prev('td').find('.new-price').attr("data-price"));
        qty -= 1;
        if (qty < 1) return;
        let money = qty * price; // Tính thành tiền
        //hiển thị dữ liệu
        $(this).parent().find('.cart-plus-minus-box').val(qty); //hiển thị lại số lượng
        //cập nhập lại số lượng trong sesion
        let id = $(this).parent().parent().parent().siblings('.btn-action').children('.update').attr("data-id");
        let href = "/shopcart/Update/?id=" + id + "&quantity=" + qty; // link cập nhật sản phẩm trong giỏ hàng
        $(this).parent().parent().parent().siblings('.btn-action').children('.update').attr("href", href);
        $(this).parent().parent().parent().siblings('.product-subtotal').children('.total-money').attr("data-money", money);
        let total_money = formatMoney(money, 0, ",", ".");

        $(this).parent().parent().parent().siblings('td').children().children("span#money").text(total_money);
        let totalMoney = formatMoney(total(), 0, ",", ".");
        $("#totalMoney").text(totalMoney); // hiển thị tổng tiền
        //$("#totalMoneyPay").text(totalMoney);// hiển thị tổng tiền
    })

    //Tăng
    $('.cart-plus-minus .fa-plus').click(function () {
        //console.log("ok");
        let qty = parseInt($(this).parent().siblings('.cart-plus-minus-box').val()); // lấy giá trị ô input
        console.log(qty);

        let price = parseFloat($(this).closest('td').prev('td').find('.new-price').attr("data-price"));
        // let price = parseFloat($(this).parent().parent().siblings().children("span.new-price").attr("data-price"));
        //giới hạn số lượng tăng
        let maxQuantity = parseInt($(this).parent().siblings('.cart-plus-minus-box').data('max-quantity'));
        //var maxVal = parseInt($input.data("max-quantity"));
        if (qty < maxQuantity) {
            qty += 1;
        }
        
        //qty += 1;
        let money = qty * price; // Tính thành tiền
        //hiển thị dữ liệu
        // $(this).parent().siblings('.cart-plus-minus-box').val(qty); //hiển thị lại số lượng
        console.log("ok", qty, price, money);
        //cập nhập lại số lượng trong sesion
        let id = $(this).parent().parent().parent().siblings('.btn-action').children('.update').attr("data-id");
        let href = "/shopcart/Update/?id=" + id + "&quantity=" + qty; // link cập nhật sản phẩm trong giỏ hàng
        $(this).parent().parent().parent().siblings('.btn-action').children('.update').attr("href", href);
        $(this).parent().parent().parent().siblings('.product-subtotal').children('.total-money').attr("data-money", money);
        let total_money = formatMoney(money, 0, ",", ".");
        console.log("ok", qty, price, money, total_money);
        $(this).parent().parent().parent().siblings('.product-subtotal').children('.total-money').children("#money").text(total_money);
        let totalMoney = formatMoney(total(), 0, ",", ".");
        $("#totalMoney").text(totalMoney); // hiển thị tổng tiền
        //$("#totalMoneyPay").text(totalMoney);// hiển thị tổng tiền
    })

    function total() {
        let totalMoney = 0;
        $("p.total-money").each(function () {
            console.log($(this));
            console.log($(this).attr("data-money"));
            let money = parseFloat($(this).attr("data-money"));
            console.log("total-money", money);
            totalMoney += money;
        });
        return totalMoney;
    }
});

function formatMoney(amount, decimalCount = 2, decimal = ".", thousands = ",") {
    try {
        decimalCount = Math.abs(decimalCount);
        decimalCount = isNaN(decimalCount) ? 2 : decimalCount;
        const negativeSign = amount < 0 ? "-" : "";
        let i = parseInt(amount = Math.abs(Number(amount) ||
            0).toFixed(decimalCount)).toString();
        let j = (i.length > 3) ? i.length % 3 : 0;
        return negativeSign +
            (j ? i.substr(0, j) + thousands : '') +
            i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + thousands) +
            (decimalCount ? decimal + Math.abs(amount -
                i).toFixed(decimalCount).slice(2) : "");
    } catch (e) {
        console.log(e)
    }
};