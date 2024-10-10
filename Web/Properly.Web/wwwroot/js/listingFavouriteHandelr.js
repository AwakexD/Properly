import { get, post } from './webApiRequester.js';

document.addEventListener('DOMContentLoaded', () => {
    const antiForgeryToken = document.querySelector("form > input[name=__RequestVerificationToken]")?.value;

    const favoriteIcons = document.querySelectorAll('.favorite-icon');

    favoriteIcons.forEach(async (icon) => {
        const listingId = icon.getAttribute('data-listing-id');

        if (!listingId) {
            console.error('Listing ID is missing.');
            return;
        }

        try {
            const response = await get(`/api/PropertyInteractions/IsFavorite?listingId=${listingId}`);

            if (response.isFavorite) {
                icon.classList.remove('fa-regular');
                icon.classList.add('fa-solid');
            }
        } catch (error) {
            console.error('Error checking favorite status:', error);
        }

        icon.addEventListener('click', async function (event) {
            event.stopPropagation();
            event.preventDefault();

            const isFavorited = icon.classList.contains('fa-solid');
            const favoriteData = { ListingId: listingId };

            try {
                let response;
                if (isFavorited) {
                    response = await post('/api/PropertyInteractions/RemoveFromFavorites', favoriteData, antiForgeryToken);
                } else {
                    response = await post('/api/PropertyInteractions/AddToFavorites', favoriteData, antiForgeryToken);
                }

                if (response.success) {
                    icon.classList.toggle('fa-solid');
                    icon.classList.toggle('fa-regular');

                    Swal.fire({
                        title: isFavorited ? 'Removed!' : 'Added!',
                        text: `This listing has been ${isFavorited ? 'removed from' : 'added to'} your favorites.`,
                        icon: 'success',
                        timer: 1500,
                        showConfirmButton: false
                    });
                } else {
                    throw new Error(response.message || 'Failed to update favorites.');
                }
            } catch (error) {
                Swal.fire({
                    title: 'Error!',
                    text: error.message,
                    icon: 'error',
                    confirmButtonText: 'OK'
                });
            }
        });
    });
});
