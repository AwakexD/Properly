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

    soldButtons.forEach(button => {
        button.addEventListener('click', async (event) => {
            const listingRow = event.target.closest('tr.file-delete');
            const routePath = listingRow.querySelector('.link').href;
            const listingId = routePath.split('/').pop();
            const statusSpan = listingRow.querySelector('td.align-middle > span');
            const currentStatus = statusSpan.innerText.trim();
            let newStatus;

            if (currentStatus === "Active") {
                newStatus = "Sold";
            } else if (currentStatus === "Sold") {
                newStatus = "Active";
            } else if (["Pending", "Rented", "Coming Soon", "Off Market"].includes(currentStatus)) {
                event.preventDefault();
                return;
            }

            const data = {
                ListingId: listingId,
                ListingStatus: newStatus
            };

            const swalAlert = await Swal.fire({
                title: `Are you sure you want to mark this listing as ${newStatus}?`,
                text: `If you confirm, the listing will be marked as ${newStatus}.`,
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                confirmButtonText: `Yes, mark as ${newStatus}!`,
                cancelButtonColor: '#d33',
                cancelButtonText: 'Cancel',
            });

            if (swalAlert.isConfirmed) {
                try {
                    const response = await post('/api/PropertyInteractions/UpdateStatus', data, antiForgeryToken);

                    if (response.success) {
                        statusSpan.innerText = newStatus;

                        const icon = event.target.querySelector('i');
                        const buttonTextNode = event.target.querySelector('span.button-text');

                        // Fix button text change
                        if (newStatus === "Active") {
                            icon.className = "fa-solid fa-angle-up";
                            buttonTextNode.textContent = " Activate"; 
                        } else if (newStatus === "Sold") {
                            icon.className = "fa-regular fa-handshake";
                            buttonTextNode.textContent = " Sold";
                        } else {
                            event.target.disabled = true;
                        }

                        Swal.fire({
                            position: "top-end",
                            width: '22em',
                            title: 'Success!',
                            text: `Listing status updated to ${newStatus}.`,
                            timer: 2500,
                            timerProgressBar: true,
                            backdrop: false,
                        });
                    }
                } catch (error) {
                    Swal.fire({
                        title: 'Error!',
                        text: `Failed to update listing status.`,
                        icon: 'error',
                        confirmButtonText: 'OK'
                    });
                }
            }
        });
    });

});