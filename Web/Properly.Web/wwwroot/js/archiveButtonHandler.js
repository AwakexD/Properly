import { post } from "./webApiRequester.js";

document.addEventListener('DOMContentLoaded', () => {
    const antiForgeryToken = document.querySelector("form > input[name=__RequestVerificationToken]").value;
    const archiveButtons = document.querySelectorAll('.btn-archive');

    if (!antiForgeryToken) {
        console.error("CSRF token not found.");
        return;
    }

    archiveButtons.forEach(button => {
        button.addEventListener('click', async (event) => {
            const messageRow = event.target.closest('tr');
            const messageId = messageRow.querySelector('input[name="messageId"]').value;
            const data = messageId;

            const swalAlert = await Swal.fire({
                title: 'Are you sure you want to archive this message?',
                text: 'This action will move the message to the Archived Messages.',
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                confirmButtonText: 'Yes, archive it!',
                cancelButtonColor: '#d33',
                cancelButtonText: 'Cancel',
            });

            if (swalAlert.isConfirmed) {
                messageRow.style.display = 'none';

                try {
                    const response = await post('/api/Messages/ArchiveMessage', data, antiForgeryToken);

                    if (response.success) {
                        const archivedMessagesTable = document.querySelector('table#archivedMessages tbody');

                        if (archivedMessagesTable) {
                            const actionCell = messageRow.lastElementChild;
                            messageRow.removeChild(actionCell);

                            archivedMessagesTable.appendChild(messageRow);
                            messageRow.style.display = '';
                        } else {
                            console.error("Archived messages table not found.");
                        }

                        Swal.fire({
                            position: "top-end",
                            width: '22em',
                            title: 'Message archived!',
                            icon: 'success',
                            text: 'The message has been successfully archived.',
                            timer: 2000,
                            timerProgressBar: true,
                            showConfirmButton: false,
                            backdrop: false,
                        });
                    } else {
                        messageRow.style.display = '';
                        Swal.fire({
                            position: "top-end",
                            width: '22em',
                            title: 'Error!',
                            icon: 'error',
                            text: response.message,
                            timer: 2000,
                            timerProgressBar: true,
                            backdrop: false,
                        });
                        console.error("Error archiving message: ", response.message);
                    }
                } catch (error) {
                    messageRow.style.display = '';
                    Swal.fire({
                        position: "top-end",
                        width: '22em',
                        title: 'Error!',
                        icon: 'error',
                        text: 'There was a problem processing your request.',
                        timer: 2000,
                        timerProgressBar: true,
                        backdrop: false,
                    });
                    console.error("Error sending POST request.", error);
                }
            }
        });
    });
});
