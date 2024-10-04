$(document).ready(function () {
    $('#itemsPerPageSelect, #sortingSelect').change(function () {
        const itemsPerPage = $('#itemsPerPageSelect').val();
        const sorting = $('#sortingSelect').val();

        const url = new URL(window.location.href);

        url.searchParams.set('ItemsPerPage', itemsPerPage);
        url.searchParams.set('ListingSorting', sorting);

        window.location.href = url.toString();
    });
});