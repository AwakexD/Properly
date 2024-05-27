import { get } from "./webApiRequester.js";

$(document).ready(function () {
    function getQueryParam(param) {
        const urlParams = new URLSearchParams(window.location.search);
        return urlParams.get(param);
    }

    const listingSorting = getQueryParam('queryModel.ListingsPerPage');
    if (listingSorting !== null) {
        $('#listingsPerPage').val(listingSorting);
    }

    $("#listingsPerPage").change(async function () {
        var selectedOption = $(this).val();
        var baseUrl = window.location.origin + window.location.pathname;
        var endPoints = `?id=1&queryModel.ListingsPerPage=${selectedOption}`
        var newUrl = baseUrl + endPoints

        try {
            await get(endPoints);
            window.location.href = newUrl;
        } catch (error) {
            console.error('Error:', error.message);
        }
    })
})