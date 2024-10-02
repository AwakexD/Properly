document.addEventListener('DOMContentLoaded', function () {
    const forRentTab = document.getElementById('forRentTab');
    const forSaleTab = document.getElementById('forSaleTab');
    const form = document.getElementById('property-search-form');
    const searchTypeInput = document.getElementById('searchType');

    const event = new Event('input');

    forRentTab.addEventListener('click', function () {
        form.action = '/Property/Rent';
        searchTypeInput.value = 'For Rent';
        searchTypeInput.dispatchEvent(event);
    });

    forSaleTab.addEventListener('click', function () {
        form.action = '/Property/Buy';
        searchTypeInput.value = 'For Sale';
        searchTypeInput.dispatchEvent(event);
    });
});