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
                        Swal.fire({
                            position: "top-end",
                            width: '22em',
                            title: 'Error!',
                            icon: 'error',
                            text: `${response.message}`,
                            timer: 2000,
                            timerProgressBar: true,
                            backdrop: false,
                        });
                        console.error("Error sending POST request.", error);
                    }
                }
            }
        });
    });

    soldButtons.forEach(button => {
        button.addEventListener('click', async (event) => {
            const listingRow = event.target.closest('tr');
            const listingId = button.getAttribute('data-listing-id');
            const currentStatus = button.getAttribute('data-listing-status');
            const listingType = button.getAttribute('data-listing-type');

            console.log("Listing row: ", listingRow);
            console.log("Listing Id: ", listingId);
            console.log("Current status: ", currentStatus);
            console.log("Listing type: ", listingType);

            let newStatus;
            if (currentStatus === "Active") {
                newStatus = (listingType === "For Rent") ? "Rented" : "Sold";
                console.log("New status: ", newStatus);
            } else if (currentStatus === "Sold" || currentStatus === "Rented") {
                newStatus = "Active";
                console.log("New status: ", newStatus);
            } else {
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
                        button.setAttribute('data-listing-status', newStatus); 
                        const buttonTextNode = button.querySelector('span.button-text');
                        const icon = button.querySelector('i');

                        if (newStatus === "Rented" || newStatus === "Sold") {
                            icon.className = "fa-solid fa-angle-up";
                            buttonTextNode.innerText = "Activate";  
                        } else if (newStatus === "Active") {
                            icon.className = "fa-regular fa-handshake";
                            buttonTextNode.innerText = (listingType === "For Rent") ? "Rented" : "Sold"; 
                        }

                       
                        const statusSpan = listingRow.querySelector('td.align-middle > span'); 
                        if (statusSpan) {
                            statusSpan.innerText = newStatus;
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
                    } else {
                        throw new Error(response.message || "Failed to update status.");
                    }
                } catch (error) {
                    Swal.fire({
                        position: "top-end",
                        width: '22em',
                        title: 'Error!',
                        icon: 'error',
                        text: `An error occurred: ${error.message}`,
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