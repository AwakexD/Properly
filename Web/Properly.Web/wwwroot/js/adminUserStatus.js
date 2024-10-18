const adminConnection = new signalR.HubConnectionBuilder()
    .withUrl("/userStatusHub")
    .build();

adminConnection.start().then(function () {
    adminConnection.invoke("GetOnlineUsers").then(function (onlineUsers) {
        const rows = document.querySelectorAll('#usersTable tbody tr');

        rows.forEach(row => {
            const userId = row.getAttribute('data-userId');

            if (onlineUsers.hasOwnProperty(userId)) {
                const statusBadge = row.querySelector('.user-status');
                statusBadge.textContent = 'Online';
                statusBadge.classList.remove('badge-danger');
                statusBadge.classList.add('badge-success');
            }
        });
    }).catch(function (err) {
        console.error("Error fetching online users: ", err);
    });

    adminConnection.on("UserStatusChanged", function (userId, status) {
        const row = document.querySelector(`#usersTable tbody tr[data-userId="${userId}"]`);

        if (row) {
            const statusBadge = row.querySelector('.user-status');

            if (status === 'Online') {
                statusBadge.textContent = 'Online';
                statusBadge.classList.remove('badge-danger');
                statusBadge.classList.add('badge-success');
            } else if (status === 'Offline') {
                statusBadge.textContent = 'Offline';
                statusBadge.classList.remove('badge-success');
                statusBadge.classList.add('badge-danger');
            }
        }
    });

}).catch(function (err) {
    console.error("Error connecting to the hub: ", err);
});
