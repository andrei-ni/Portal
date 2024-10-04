// Hide boxes while searching
$(function () {

    // Not used
    //document.getElementById('searchInput').addEventListener('keyup', function () {
    //    let searchQuery = this.value.toLowerCase();
    //    let boxes = document.querySelectorAll('.tall-box');

    //    boxes.forEach(box => {
    //        let boxText = box.querySelector('.tall-box-text p').textContent.toLowerCase();
    //        let boxDescription = box.querySelector('.tall-box-text span').textContent.toLowerCase();
    //        if (boxText.includes(searchQuery) || boxDescription.includes(searchQuery)) {
    //            box.style.display = 'block';
    //        } else {
    //            box.style.display = 'none';
    //        }
    //    });
    //});

    const searchInput = document.getElementById("searchInput");
    var categoryId = document.getElementById("categoryId");
    document.getElementById('searchInput').addEventListener('keyup', function () {
        $.ajax({
            url: '/Category/SearchKeywords/',
            data: {
                searchText: searchInput.value,
                catId: categoryId.value,
            },
            type: "POST",
            success: function (data) {
                //console.log(data);
                if (data !== "Not found") {

                    let boxes = document.querySelectorAll('.tall-box');

                    boxes.forEach(box => {
                        let boxId = box.querySelector('.tall-box b').textContent;
                        let boxText = box.querySelector('.tall-box-text p').textContent.toLowerCase();
                        let boxDescription = box.querySelector('.tall-box-text span').textContent.toLowerCase();

                        if (data.some(item => boxId.includes(item)) || data.some(item => boxText.includes(item)) || data.some(item => boxDescription.includes(item))) {
                            box.style.display = 'block';
                        } else {
                            box.style.display = 'none';
                        }
                    });
                }
            },
            error: function (response) {
                console.error(response);
            }
        });
    });

    const closeButton = document.getElementById("closeButton");
    searchInput.addEventListener("input", function (event) {
        closeButton.style = "display: block; position: absolute;right: 10px; top: 50%;transform: translateY(-50%);";
        //console.log("User typed something:", event.target.value);
    });

    $("#closeButton").on("click", function () {
        location.reload();
    });

    var categoryId = document.getElementById("categoryId");
    $("#listCloseButton").on("click", function () {
        window.location.href = location.origin + "/Category?id=" + categoryId.value;
    });
});
$(function () {
    const searchInput = document.getElementById("searchInput");
    $("#searchInput").on("keydown", function (event) {
        if (event.key === "Enter") {
            if (searchInput.value.trim() === '') {
                var categoryId = document.getElementById("categoryId");
                window.location.href = location.origin + "/Category?id=" + categoryId.value;
            }
        }
    });
});

// Not used
// SEARCH
// On click and keydown
//$(function () {

//    $("#searchInput").on("keydown", function (event) {
//        if (event.key === "Enter") {
//            PerformSearch();
//        }
//    });
//});
//$(function () {
//    $("#searchButton").on("click", function () {
//        PerformSearch();
//    });
//});
//function PerformSearch() {
//    var searchInputText = document.getElementById("searchInput");
//    var categoryId = document.getElementById("categoryId");

//    $.ajax({
//        url: '/Category/SearchKeywords/',
//        data: {
//            "searchText": searchInputText.value,
//            "catId": categoryId.value,
//        },
//        type: "POST",
//        success: function (data) {
//            if (data === "Not found") {
//                Toastify({
//                    text: "Not apps found",
//                    duration: 10000,
//                    newWindow: true,
//                    close: true,
//                    gravity: "bottom", // `top` or `bottom`
//                    position: "left", // `left`, `center` or `right`
//                    stopOnFocus: true, // Prevents dismissing of toast on hover
//                    style: {
//                        background: "linear-gradient(to right, #008A99, #55B1BB)",

//                    },
//                }).showToast();
//            }
//            else {
//                searchInputText.value = searchInputText.value.replace(/\s+/g, '');
//                window.location.href = location.origin + "/Category?id=" + categoryId.value + "&searchText=" + searchInputText.value;
//            }
//        },
//        error: function (response) {
//            $("#searchInput").val("Error: " + response);
//        }

//    });
//}

