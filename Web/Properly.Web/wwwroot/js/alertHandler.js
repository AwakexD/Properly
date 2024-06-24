document.addEventListener('DOMContentLoaded', function () {
    var notification = '@TempData["Notification"]';
    var notificationType = '@TempData["NotificationType"]';

    if (notification) {
        showAlert(notification, notificationType);
    }
});

function showAlert(message, type = 'success') {
    Swal.fire({
        icon: type,
        title: message,
        showConfirmButton: false,
        timer: 2000
    });
}