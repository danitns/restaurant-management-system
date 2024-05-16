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