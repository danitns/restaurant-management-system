
const updateRole = async () => {
    var response = await fetch("/User/BecomeManager", {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        }
    });

    if (response.ok) {
        const jsonResponse = await response.json();
        if (jsonResponse.success) {
            createToast("Request sent", "Your request has been sent. You will be contacted soon by an admin.", "toast-notification", "organizerRequestToast");
            $('.toast').toast('show');
            var becomeManagerDiv = document.getElementById('become-manager-div');
            becomeManagerDiv.style.display = 'none';
        }
    }
}
