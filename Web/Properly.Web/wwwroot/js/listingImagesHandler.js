import { post } from "./webApiRequester.js";

document.addEventListener('DOMContentLoaded', () => {
    console.log("Document loaded.")
    const deleteButtons = document.querySelectorAll('.delete-button i.fa-trash');
    const listingId = document.querySelector('.listingId').value;
    const antiForgeryToken = document.querySelector("form > input[name=__RequestVerificationToken]").value;


    if (!antiForgeryToken) {
        console.error("CSRF token not found.");
        return;
    }

    if (!listingId) {
        console.error("Listing ID not found.");
        return;
    }

    const handleButtonClick = async (event) => {
        event.preventDefault();

        const cardElement = event.target.closest('.card');
        const imageUrl = event.target.closest('.card').querySelector('.card-img-top').src;

        const swalAlert = await Swal.fire({
            title: 'Are you sure you want to delete this photo?',
            text: 'If you confirm, the photo will be permanently deleted.',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            confirmButtonText: 'Yes, delete it!',
            cancelButtonColor: '#d33',
            cancelButtonText: 'Cancel'
        });

        if (swalAlert.isConfirmed) {
            const data = {
                ListingId: listingId,
                ImageUrl: imageUrl,
            };
    
            try {
                const response = await post('/api/PropertyInteractions/DeleteImage', data, antiForgeryToken);

                if (response.success) {
                    cardElement.style.display = 'none';
                }
            } catch (error) {
                console.error("Error sending POST request.", error);
            }
        }

    }

    deleteButtons.forEach(button => {
        button.addEventListener('click', handleButtonClick);
    });
});