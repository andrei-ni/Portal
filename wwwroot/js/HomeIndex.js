
// On initialized
$(function () {
    // Fade in effect for the boxes, check .box in site.css
    let boxes = document.querySelectorAll('.box');

    boxes.forEach((box, index) => {
        // Add a delay
        setTimeout(() => {
            box.style = 'transform: translateY(5%); opacity: 1;';
        }, index * 40); // 40ms delay between each box
    });

    // All fade in at the same time
    //boxes.forEach(box => {
    //    box.style = ' transform: translateY(5%);transition: 0.5s ease-in;opacity: 1; ';
    //});
});

function SearchAdvanced(lang) {
    let boxes = document.querySelectorAll('.box');
    var seachInput = document.getElementById('searchInput');
    //const closeAdvancedSearchButton = document.getElementById("closeAdvancedSearchButton");
    const closeButton = document.getElementById("closeButton");
    const advancedSearchButton = document.querySelector(".advanced-search-btn");
    var newDiv = document.createElement("div");
    newDiv.style.height = "30px";

    // Fade out the boxes. Not used anymore because of the redirect on bottom
    //boxes.forEach((box, index) => {
    //    setTimeout(() => {
    //        box.style = 'transform: translateY(-5%); transition: 0.5s ease-out; opacity: 0;';
    //        // Remove them from page
    //        setTimeout(() => {
    //            box.parentNode.removeChild(box);
    //        }, 800); // 800ms delay to not see the removal
    //    }, index * 40); // 40ms delay between each box
    //});

    closeButton.style.display = 'none';
    advancedSearchButton.replaceWith(newDiv);
    searchInput.value = '';
    //seachInput.placeholder = "Caută în toate aplicațiile";
    switch (lang) {
        case 'RO':
            seachInput.placeholder = "Caută în toate aplicațiile";
            break;
        case 'EN':
            seachInput.placeholder = "Search in all apps";
            break;
        case 'JA':
            seachInput.placeholder = "すべてのアプリを検索";
            break;
    }

    //closeAdvancedSearchButton.style = "display: block; position: absolute;right: 10px; top: 28%;transform: translateY(-50%);";
    //searchInput.focus();

    let count = boxes.length;
    let transitionFailed = false;

    boxes.forEach((box, index) => {
        setTimeout(() => {
            box.style = 'transform: translateY(-5%); transition: 0.5s ease-out; opacity: 0;';

            let transitionCheck = setTimeout(() => {
                transitionFailed = true;
                window.location.href = '/Home/SearchAdvanced';
            }, 550); // Check slightly after the transition duration

            box.addEventListener('transitionend', function handler() {
                clearTimeout(transitionCheck); // Clear the check if transition ended
                box.removeEventListener('transitionend', handler); // Remove event listener after it's fired once
                count--;
                if (count === 3 && !transitionFailed) {
                    // When all boxes have faded out, redirect to another page
                    window.location.href = '/Home/SearchAdvanced';
                }
            });
        }, index * 40); // 40ms delay between each box
    });


}


// Function to remove diacritics
function removeDiacritics(str) {
    return str.normalize('NFD').replace(/[\u0300-\u036f]/g, '');
}
// Hide boxes while searching
$(function () {
    document.getElementById('searchInput').addEventListener('keyup', function () {
        let searchQuery = removeDiacritics(this.value.toLowerCase());
        let boxes = document.querySelectorAll('.box');

        boxes.forEach(box => {
            let boxText = removeDiacritics(box.querySelector('.box-text p').textContent.toLowerCase());
            let boxDescription = removeDiacritics(box.querySelector('.box-text span').textContent.toLowerCase());
            if (boxText.includes(searchQuery) || boxDescription.includes(searchQuery)) {
                box.style.display = 'block';
            } else {
                box.style.display = 'none';
            }
        });
    });

    const searchInput = document.getElementById("searchInput");
    const closeButton = document.getElementById("closeButton");
    searchInput.addEventListener("input", function (event) {
        closeButton.style = "display: block; position: absolute;right: 10px; top: 28%;transform: translateY(-50%);";
        //console.log("User typed something:", event.target.value);
    });
    $("#closeButton").on("click", function () {
        location.reload();
    });

    $("#closeAdvancedSearchButton").on("click", function () {
        location.reload();
    });
});

$(function () {
    const searchInput = document.getElementById("searchInput");
    $("#searchInput").on("keydown", function (event) {
        if (event.key === "Enter") {
            if (searchInput.value.trim() === '') {
                location.reload();
            }
        }
    });
});
function GotoAllAppsPage() {
    window.location.href = '/Application/Index';
}
function GotoCategoryAppsPage(id) {
    window.location.href = '/Category/Index/?id=' + id;
}
