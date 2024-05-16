// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(".deleteButton").click(function (event) {
    event.preventDefault();

    let editButton = $(this).closest('.buttonsDiv').find('.editButton');

    editButton.hide();

    let noButton = $(this).closest('.buttonsDiv').find('.noButton');

    let yesButton = $(this).closest('.buttonsDiv').find('.yesButton');

    noButton[0].onclick = () => {
        yesButton.hide();
        noButton.hide();
        editButton.show();
        $(this).show();
    }

    $(this).hide();

    noButton.show();
    yesButton.show();
});