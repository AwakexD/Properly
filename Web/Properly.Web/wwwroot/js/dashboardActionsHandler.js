import { post } from "./webApiRequester.js";

document.addEventListener('DOMContentLoaded', () => {
    const antiForgeryToken = document.querySelector("form > input[name=__RequestVerificationToken]").value;
    const deleteButtons = document.querySelectorAll('.deleteBtn');
    const soldButtons = document.querySelectorAll('.soldBtn');

    if (!antiForgeryToken) {
        console.error("CSRF token not found.");
        return;
    }

    deleteButtons.forEach(button => {
        button.addEventListener('click', async (event) => {
            const listingRow = event.target.closest('tr.file-delete');
            const routePath = listingRow.querySelector('.link').href;
            const listingId = routePath.split('/').pop();
            const data = { ListingId: listingId };

            const swalAlert = await Swal.fire({
                title: 'Are you sure you want to delete this listing?',
                text: 'If you confirm, the listing will be permanently removed.',
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                confirmButtonText: 'Yes, delete it!',
                cancelButtonColor: '#d33',
                cancelButtonText: 'Cancel',
            });

            if (swalAlert.isConfirmed) {
                listingRow.style.display = 'none';

                const undoAlert = await Swal.fire({
                    position: "top-end",
                    width: '22em',
                    title: 'Listing deleted!',
                    text: 'You can undo this action within the next 10 seconds.',
                    showConfirmButton: true,
                    confirmButtonText: 'Undo',
                    timer: 10000,
                    timerProgressBar: true,
                    showCancelButton: false,
                    backdrop: false,
                });

                if (undoAlert.isConfirmed) {
                    listingRow.style.display = '';
                } else {
                    try {
                        const response = await post('/api/PropertyInteractions/Delete', data, antiForgeryToken);

                        if (response.success) {
                            listingRow.remove();
                        }
                    } catch (error) {
                        console.error("Error sending POST request.", error);
                    }
                }
            }
        });
    });
});