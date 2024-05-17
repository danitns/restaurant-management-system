const createToast = (titleText, bodyText, containerId, id) => {
    let container = document.getElementById(containerId);
    let toastDiv = document.createElement('div');
    toastDiv.className = 'toast';
    if (id != null) {
        toastDiv.id = id;
    }
    toastDiv.role = "alert";
    toastDiv.ariaLive = "assertive";
    toastDiv.ariaAtomic = "true";

    let toastHeader = document.createElement('div');
    toastHeader.className = "toast-header";

    let title = document.createElement('strong');
    title.className = "mr-auto";
    title.textContent = titleText;

    let toastBody = document.createElement('div');
    toastBody.textContent = bodyText;
    toastBody.className = 'toast-body';

    toastHeader.appendChild(title);

    toastDiv.appendChild(toastHeader);
    toastDiv.appendChild(toastBody);
    container.appendChild(toastDiv);
}