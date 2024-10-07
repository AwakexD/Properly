import { post } from './webApiRequester.js';

document.addEventListener('DOMContentLoaded', () => {
    const antiForgeryToken = document.querySelector("form > input[name=__RequestVerificationToken]")?.value;

    if (!antiForgeryToken) {
        console.error("CSRF token not found.");
        return;
    }

    const sendMessageButton = document.getElementById('sendMessageButton');

    sendMessageButton.addEventListener('click', async (event) => {
        event.preventDefault();
        const messageData = {
            FullName: document.getElementById('fullname').value,
            PhoneNumber: document.getElementById('phone').value,
            Email: document.getElementById('email').value,
            MessageContent: document.getElementById('message').value,
            ListingId: document.getElementById('listingId').value
        };

        const swalAlert = await Swal.fire({
            title: 'Send Message',
            text: 'Are you sure you want to send this message?',
            icon: 'info',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            confirmButtonText: 'Yes, send it!',
            cancelButtonColor: '#d33',
            cancelButtonText: 'Cancel',
        });

        if (swalAlert.isConfirmed) {
            try {
                const response = await post('/api/messages/sendmessage', messageData, antiForgeryToken);

                if (response.success) {
                    document.getElementById('messageForm').reset();
                    Swal.fire({
                        position: "top-end",
                        width: '22em',
                        title: 'Success!',
                        text: `Your message has been sent successfully.`,
                        timer: 2000,
                        timerProgressBar: true,
                        backdrop: false,
                    });
                } else {
                    throw new Error(response.message);
                }
            } catch (error) {
                Swal.fire({
                    title: 'Error!',
                    text: error.message,
                    icon: 'error',
                    confirmButtonText: 'OK'
                });
            }
        }
    });
});