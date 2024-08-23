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

        const imageUrl = event.target.closest('.card').querySelector('.card-img-top').src;

        const data = {
            ListingId: listingId,
            ImageUrl: imageUrl,
        };

        try {
            const response = await post('/api/PropertyInteractions/DeleteImage', data, antiForgeryToken);
            // Todo : Remove the image card from the DOM
        } catch (error) {
            console.error("Error sending POST request.", error);
        }
    }

    deleteButtons.forEach(button => {
        button.addEventListener('click', handleButtonClick);
    });
});