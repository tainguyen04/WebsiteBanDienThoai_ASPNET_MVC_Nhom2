// Hiển thị thông báo từ TempData
if (typeof ThongBao !== 'undefined' && ThongBao) {
    Swal.fire({
        icon: 'success',
        title: 'Thành công',
        text: ThongBao,
        confirmButtonText: 'OK',
        timer: 3000
    });
}

// Hàm xác nhận xóa
function confirmDelete(button, entityName) {
    Swal.fire({
        icon: 'error',
        title: 'Bạn có chắc muốn xóa?',
        html: 'Bạn có chắc muốn xóa "<b>' + entityName + '</b>" khỏi giỏ hàng không?',
        showCancelButton: true,
        confirmButtonText: 'Xóa',
        cancelButtonText: 'Hủy'
    }).then((result) => {
        if (result.isConfirmed) {
            button.closest('form').submit();
        }
    });
}

// Tính tổng tiền
function calculateTotal() {
    let total = 0;
    let count = 0;
    document.querySelectorAll('.product-checkbox:checked').forEach(checkbox => {
        total += parseInt(checkbox.getAttribute('data-total'));
        count++;
    });

    document.getElementById('selectedTotal').textContent = total.toLocaleString('vi-VN') + ' ₫';
    document.getElementById('selectedCount').textContent = count;
}

// Khởi tạo khi DOM ready
document.addEventListener('DOMContentLoaded', function () {
    const selectAllCheckbox = document.getElementById('select-all');
    const productCheckboxes = document.querySelectorAll('.product-checkbox');

    // Chọn tất cả
    if (selectAllCheckbox) {
        selectAllCheckbox.addEventListener('change', function () {
            productCheckboxes.forEach(function (checkbox) {
                checkbox.checked = selectAllCheckbox.checked;
            });
            calculateTotal();
        });
    }

    // Thêm event listener cho từng checkbox
    productCheckboxes.forEach(function (checkbox) {
        checkbox.addEventListener('change', calculateTotal);
    });

    // Xử lý thanh toán
    const checkoutForm = document.getElementById('checkoutForm');
    if (checkoutForm) {
        checkoutForm.addEventListener('submit', function (e) {
            const selectedIds = [];
            document.querySelectorAll('.product-checkbox:checked').forEach(checkbox => {
                selectedIds.push(checkbox.getAttribute('data-id'));
            });
            if (selectedIds.length === 0) {
                e.preventDefault();
                Swal.fire('Thông báo', 'Vui lòng chọn ít nhất 1 sản phẩm để thanh toán', 'warning');
                return;
            }
            document.getElementById('selectedCartItems').value = selectedIds.join(',');
        });
    }
});