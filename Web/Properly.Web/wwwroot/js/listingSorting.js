import { get } from './webApiRequester.js';

$(document).ready(function () {
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