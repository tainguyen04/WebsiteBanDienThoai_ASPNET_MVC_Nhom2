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