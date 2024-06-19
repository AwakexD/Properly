import { post } from "./webApiRequester.js";

document.addEventListener('DOMContentLoaded', () => {
    const deleteButtons = document.querySelectorAll('#deleteBtn');
    const antiForgeryTokenElement = document.querySelector("form > input[name=__RequestVerificationToken]");
    const antiForgeryToken = antiForgeryTokenElement ? antiForgeryTokenElement.value : null;

    deleteButtons.forEach(button => {
        button.addEventListener('click', async function(event) {
            console.log("Clicked");
            const listingRow = event.target.closest('tr.file-delete');

            if (listingRow && antiForgeryToken) {
                const routePath = listingRow.querySelector('.link').href;
                const listingId = routePath.split('/').pop();

                console.log(antiForgeryToken);

                const data = { ListingId: listingId };

                try {
                    const response = await post('/api/PropertyInteractions/Delete', data, antiForgeryToken);
                    console.log("Post response:", response);
                } catch (error) {
                    console.error("Error sending POST request.", error);
                }
            }
        });
    });
});