import { get } from './webApiRequester.js';

$(document).ready(function () {
    // Reorder ListingSorting options with the current selected
    function getQueryParam(param) {
        const urlParams = new URLSearchParams(window.location.search);
        return urlParams.get(param);
    }

    const listingSorting = getQueryParam('queryModel.ListingSorting');
    if (listingSorting !== null) {
        $('#sortingSelect').val(listingSorting);
    }

    // Handle change event and relaod
    $('.list-sort').change(async function () {
        var selectedOption = $(this).val();
        var baseUrl = window.location.origin + window.location.pathname;
        var endPoints = `?id=1&queryModel.ListingSorting=${selectedOption}`
        var newUrl = baseUrl + endPoints

        try {
            await get(endPoints);
            window.location.href = newUrl;
        } catch (error) {
            console.error('Error:', error.message);
        }
    });
});