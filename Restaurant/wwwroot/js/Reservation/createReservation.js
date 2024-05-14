const fetchSelectOptions = async (date, numberOfGuests) => {
    try {
        const response = await fetch(`/Reservation/GetAvailableHours?date=${date}&numberOfGuests=${numberOfGuests}`);
        if (response.ok) {
            const data = await response.json();
            return data;
        }
    } catch {
        console.log("Error fetching Reservation/GetAvailableHours");
    }
}

const populateSelect = async () => {
    const timeSelect = document.getElementById('timeSelect');
    const numberOfGuestsInput = document.getElementById('numberOfGuestsInput');
    const dateInput = document.getElementById('dateInput');

    const numberOfGuests = numberOfGuestsInput.value;
    const date = dateInput.value;

    const timeOptions = await fetchSelectOptions(date, numberOfGuests);
    if (timeOptions !== undefined) {
        timeOptions.forEach((timeOption) => {
            const newOption = document.createElement('option');
            newOption.text = timeOption;
            newOption.value = timeOption;
            timeSelect.appendChild(newOption);
        })
    }
}

const findAvailableHours = async () => {
    const button = document.getElementById('findButton');
    button.addEventListener('click', async () => {
        await populateSelect();
    })
}

findAvailableHours();
