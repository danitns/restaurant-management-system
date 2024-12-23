﻿const filtersAndPagination = (nextButtonId, prevButtonId, filterButtonId, currentPageDivId, controllerName, inputsIdsAndNames, objectId, maxLimitInputIds) => {

    const setCurrrentFiltersAndPage = async () => {
        var response = await fetch(`/${controllerName}/GetFiltersAndCurrentPage`, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json'
            }
        });

        if (response.ok) {
            const jsonResponse = await response.json();
            inputsIdsAndNames.forEach((element) => {
                if (element.name in jsonResponse) {
                    const value = jsonResponse[element.name];
                    const inputElement = document.getElementById(element.inputId);

                    if (inputElement) {
                        inputElement[element.valueType] = value;
                    }
                }
            })

            if (maxLimitInputIds) {
                maxLimitInputIds.forEach((elemId) => {
                    elem = document.getElementById(elemId);
                    elem.max = jsonResponse.maxPrice;
                    elem.dispatchEvent(new Event('input'));
                })
            }

            currentPage.innerHTML = jsonResponse.currentPage;
            if (currentPage.innerText == "1") {
                prevButton.disabled = true;
            }
            else {
                prevButton.disabled = false;
            }
        }
    }

    const createQueryString = () => {
        let queryString = `/${controllerName}/Index`
        if (objectId !== undefined) {
            queryString = queryString.concat(`/${objectId}?`);
        }
        else {
            queryString = queryString.concat('?');
        }
        inputsIdsAndNames.forEach((element) => {
            const inputElement = document.getElementById(element.inputId);
            queryString = queryString.concat(`FilterModel.${element.name}=${inputElement[element.valueType]}&`);
        })
        queryString = queryString.concat(`FilterModel.CurrentPage=${pageControl.value}`);
        return queryString;
    }

    const filterButton = document.getElementById(filterButtonId);
    const nextButton = document.getElementById(nextButtonId);
    const prevButton = document.getElementById(prevButtonId);
    const currentPage = document.getElementById(currentPageDivId);
    const pageControl = document.getElementById('pageControl');

    setCurrrentFiltersAndPage();

    filterButton.onclick = () => {
        filterButton.href = createQueryString();
        filterButton.click;
    }

    nextButton.onclick = () => {
        pageControl.value = 1;
        filterButton.click();
    }

    prevButton.onclick = () => {
        pageControl.value = -1;
        filterButton.click();
    }
}
