function uploadPictureFromUser(pictureInputId, containerId) {
    let pictureInput = document.getElementById(pictureInputId);
    let container = document.getElementById(containerId);

    updatePictureFunction(pictureInput, container);

    pictureInput.oninput = function () {
        updatePictureFunction(pictureInput, container);
    }
}

let updatePictureFunction = (pictureInput, pictureContainer) => {
    const [file] = pictureInput.files
    if (file) {
        var link = URL.createObjectURL(file);
        pictureContainer.style.background = `url(${link})`;
        pictureContainer.style.backgroundPosition = 'center';
        pictureContainer.style.backgroundSize = 'contain';
        pictureContainer.style.backgroundRepeat = 'no-repeat';
    }
}

export default uploadPictureFromUser;
