$(document).ready(function () {
    //var dropdownButton = document.querySelector("#dropdown-button");
    //var dropdownContent = document.querySelector("#dropdown-content");

    // language menu
    var languageDropdownButton = document.getElementById('language-dropdown-button');
    var languageDropdownContent = document.getElementById('language-dropdown-content');

    // panel menu
    var panelDropdownButton = document.getElementById('dropdown-button');
    var panelDropdownContent = document.getElementById('dropdown-content');

    // Close the dropdown if the user clicks outside of it
    // language menu
    window.onclick = function (event) {
        if (!languageDropdownButton.contains(event.target)) {
            languageDropdownContent.style.opacity = 0;
            languageDropdownContent.style.visibility = 'hidden';
            languageDropdownContent.style.width = '0';
            languageDropdownContent.style.height = '0';
        }

        // panel menu
        if (!panelDropdownButton.contains(event.target)) {
            panelDropdownContent.style.opacity = 0;
            panelDropdownContent.style.visibility = 'hidden';
            panelDropdownContent.style.width = '0';
            panelDropdownContent.style.height = '0';
        }
    }

});

function togglePanelDropdown() {
    var dropdownContent = document.getElementById('dropdown-content');

    if (dropdownContent.style.opacity == 0) {
        dropdownContent.style.opacity = 1;
        dropdownContent.style.visibility = 'visible';
        dropdownContent.style.width = '150px';
        dropdownContent.style.height = '130px';
    } else {
        dropdownContent.style.opacity = 0;
        dropdownContent.style.visibility = 'hidden';
        dropdownContent.style.width = '0';
        dropdownContent.style.height = '0';
    }
}
function toggleLanguageDropdown() {
    var dropdownContent = document.getElementById('language-dropdown-content');

    if (dropdownContent.style.opacity == 0) {
        dropdownContent.style.opacity = 1;
        dropdownContent.style.visibility = 'visible';
        dropdownContent.style.width = '110px';
        dropdownContent.style.height = '90px';
    } else {
        dropdownContent.style.opacity = 0;
        dropdownContent.style.visibility = 'hidden';
        dropdownContent.style.width = '0';
        dropdownContent.style.height = '0';
    }
}