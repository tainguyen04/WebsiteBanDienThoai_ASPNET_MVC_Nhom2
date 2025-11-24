//Chức năng xem trước ảnh khi tải lên
function previewImage(input) {
    const reader = new FileReader();
    reader.onload = function () {
        const output = document.getElementById('imagePreview');
        output.src = reader.result;
    };
    if (input.files && input.files[0]) {
        reader.readAsDataURL(input.files[0]);
    }
}

// Hàm xác nhận xóa
function confirmDelete(event,button, entityName) {
    event.preventDefault();
    Swal.fire({
        icon: 'error',
        title: 'Bạn có chắc muốn xóa?',
        html: 'Bạn có chắc muốn xóa "<b>' + entityName + '</b>" không?',
        showCancelButton: true,
        confirmButtonText: 'Xóa',
        cancelButtonText: 'Hủy'
    }).then((result) => {
        if (result.isConfirmed) {
            button.closest('form').submit();
        }
    });
}
function confirmReset(event, button) {
    event.preventDefault();
    Swal.fire({
        icon: 'warning',
        title: 'Bạn có chắc muốn Reset mật khẩu về 123456!',
        showCancelButton: true,
        confirmButtonText: 'Reset',
        cancelButtonText: 'Hủy'
    }).then((result) => {
        if (result.isConfirmed) {
            button.closest('form').submit();
        }
    });
}
// Hiển thị thông báo từ TempData
if (typeof ThongBao !== 'undefined' && ThongBao) {
    Swal.fire({
        toast: true,
        icon: 'success',
        position: "top-end",
        title: ThongBao,
        timerProgressBar: true,
        showConfirmButton: false,
        background: "#d4edda",
        timer: 1500,
    });
}
// Hiển thị thông báo từ TempData
if (typeof ThongBaoLoi !== 'undefined' && ThongBaoLoi) {
    Swal.fire({
        icon: 'error',
        title: ThongBao,
        showCancelButton: true,
        cancelButtonText: "Ok"
    });
}




let products = [];

function addProduct() {
    const select = document.getElementById('productSelect');
    const quantityInput = document.getElementById('productQuantity');

    const productId = select.value;
    const quantity = parseInt(quantityInput.value);
    const selectedOption = select.options[select.selectedIndex];

    if (!productId || quantity <= 0) {
        alert('Vui lòng chọn sản phẩm và nhập số lượng');
        return;
    }

    const productName = selectedOption.text;
    const price = parseInt(selectedOption.getAttribute('data-price'));

    // Kiểm tra sản phẩm đã có chưa
    const existing = products.find(p => p.SanPhamId == productId);
    if (existing) {
        existing.SoLuong += quantity;
        existing.ThanhTien = existing.DonGia * existing.SoLuong;
    } else {
        products.push({
            SanPhamId: productId,
            TenSanPham: productName,
            GiaBan: price,
            SoLuong: quantity,
            ThanhTien: price * quantity
        });
    }

    renderProducts();
    updateTotal();

    // Reset form
    select.value = '';
    quantityInput.value = 1;
}

function renderProducts() {
    const tbody = document.getElementById('productTableBody');
    tbody.innerHTML = '';

    products.forEach((item, index) => {
        const row = document.createElement('tr');
        row.innerHTML = `
                    <td>${item.TenSanPham}</td>
                    <td>${formatPrice(item.DonGia)}</td>
                    <td>
                        <input type="number" class="form-control"
                               value="${item.SoLuong}" min="1"
                               onchange="updateQuantity(${index}, this.value)">
                    </td>
                    <td>${formatPrice(item.ThanhTien)}</td>
                    <td>
                        <button type="button" class="btn btn-danger btn-sm"
                                onclick="removeProduct(${index})">X</button>
                    </td>
                `;
        tbody.appendChild(row);
    });
}

function updateQuantity(index, newQuantity) {
    const quantity = parseInt(newQuantity);
    if (quantity > 0) {
        products[index].SoLuong = quantity;
        products[index].ThanhTien = products[index].DonGia * quantity;
        renderProducts();
        updateTotal();
    }
}

function removeProduct(index) {
    products.splice(index, 1);
    renderProducts();
    updateTotal();
}

function updateTotal() {
    const total = products.reduce((sum, item) => sum + item.ThanhTien, 0);
    document.getElementById('totalAmount').textContent = formatPrice(total);
    document.getElementById('TongTienDisplay').value = total;
    document.getElementById('TongTien').value = total;
}

function formatPrice(amount) {
    return new Intl.NumberFormat('vi-VN').format(amount) + ' ₫';
}

function prepareSubmit() {
    if (products.length === 0) {
        alert('Vui lòng thêm ít nhất một sản phẩm');
        return false;
    }

    document.getElementById('chiTietHoaDonJson').value = JSON.stringify(products);
    return true;
}


//Load thêm sản phẩm
var skip = 10;
$("#loadMoreBtn").on("click", function () {
    $.ajax({
        url: '@Url.Action("LoadMoreProducts", "Home")',
        type: "GET",
        data: { skip: skip },
        success: function (data) {
            if (data.trim() === "") {
                $("#loadMoreBtn").hide();
            } else {
                $("#sanPhamContainer").append(data);
                skip += 10;
            }
        }
    });
});