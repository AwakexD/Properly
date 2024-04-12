import { post } from "./webApiRequester.js";

document.addEventListener("DOMContentLoaded", async () => {
    const favouriteButtons = document.querySelectorAll("i.fa-bookmark");
    const antiForgeryToken = document.querySelector("#antiForgeryForm input[name=__RequestVerificationToken]").value;

    const addToFavouriteUrl = "/api/PropertyInteractions/Favourite"

    favouriteButtons.forEach(element => {
        element.addEventListener("click", (event) => {
            event.preventDefault();
            const listingId = event.target.closest(".body-main").querySelector("#listing-id").textContent;
            AddToFavourites(listingId);
            UpdateButtonColor(event);
        })
    })

    async function AddToFavourites(listingId) {
        await post(addToFavouriteUrl, { ListingId: listingId }, antiForgeryToken);
    }


    function UpdateButtonColor(event) {
        const button = event.target;
        button.style.transition = "color 0.3s ease";
        const color = getComputedStyle(button)["color"];
        const newColor = color === "rgb(255, 212, 59)" ? "rgb(116, 192, 252)" : "rgb(255, 212, 59)";
        button.style.color = newColor;
    }
})