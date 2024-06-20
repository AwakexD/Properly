import { post } from "./webApiRequester.js";

document.addEventListener('DOMContentLoaded', () => {
    const antiForgeryTokenElement = document.querySelector("form > input[name=__RequestVerificationToken]");
    const antiForgeryToken = antiForgeryTokenElement ? antiForgeryTokenElement.value : null;

    if (!antiForgeryToken) {
        console.error("CSRF token not found.");
        return;
    }

    const handleButtonClick = async (event, apiEndpoint) => {
        console.log("Clicked");
        const listingRow = event.target.closest('tr.file-delete');

        if (listingRow) {
            const routePath = listingRow.querySelector('.link').href;
            const listingId = routePath.split('/').pop();
            const data = { ListingId: listingId };

            try {
                const response = await post(apiEndpoint, data, antiForgeryToken);
                console.log("Post response:", response);
            } catch (error) {
                console.error("Error sending POST request.", error);
            }
        }
    };

    const addButtonEventListeners = (selector, apiEndpoint) => {
        document.querySelectorAll(selector).forEach(button => {
            button.addEventListener('click', event => handleButtonClick(event, apiEndpoint));
        });
    };

    addButtonEventListeners('#deleteBtn', '/api/PropertyInteractions/Delete');
    addButtonEventListeners('#soldBtn', '/api/PropertyInteractions/Sold');
    addButtonEventListeners('#editBtn', '/api/PropertyInteractions/Edit')
});