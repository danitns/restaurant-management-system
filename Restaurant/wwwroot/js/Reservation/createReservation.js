const fetchSelectOptions = async (date, numberOfGuests, restaurantId) => {
    try {
        const response = await fetch(`/Reservation/GetAvailableHours?date=${date}&numberOfGuests=${numberOfGuests}&restaurantId=${restaurantId}`);
        if (response.ok) {
            const data = await response.json();
            return data;
        }
    } catch {
        console.log("Error fetching Reservation/GetAvailableHours");
    }
}

const populateSelect = async (date, numberOfGuests) => {
    const timeSelect = document.getElementById('timeSelect');
    const restaurantIdInput = document.getElementById('restaurantIdInput');
    const restaurantId = restaurantIdInput.value;

    const timeOptions = await fetchSelectOptions(date, numberOfGuests, restaurantId);
    if (timeOptions !== undefined) {
        timeSelect.innerHTML = '';
        timeOptions.forEach((timeOption) => {
            const newOption = document.createElement('option');
            newOption.text = timeOption;
            newOption.value = timeOption;
            timeSelect.appendChild(newOption);
        })
    }
}

const findAvailableHours = async (dateInput, numberOfGuestsInput) => {
    populateSelect(dateInput.value, numberOfGuestsInput.value);
    dateInput.addEventListener('change', async() => {
        await populateSelect(dateInput.value, numberOfGuestsInput.value);
    })
    numberOfGuestsInput.addEventListener('change', async () => {
        await populateSelect(dateInput.value, numberOfGuestsInput.value);
    })
}
const reservationDateInput = document.getElementById('dateInput');
const reservationNumberOfGuestsInput = document.getElementById('numberOfGuestsInput');

findAvailableHours(reservationDateInput, reservationNumberOfGuestsInput);
