
// On initialized
$(function () {
    const closeAdvancedSearchButton = document.getElementById("closeAdvancedSearchButton");
    closeAdvancedSearchButton.style = "display: block; position: absolute;right: 10px; top: 50%;transform: translateY(-50%);";

    var seachInput = document.getElementById('searchInput');
    searchInput.focus();

});

$(function () {
    const searchInput = document.getElementById("searchInput");
    document.getElementById('searchInput').addEventListener('keyup', function () {
        document.getElementById("closeAdvancedSearchButton").style.display = 'none';
        $.ajax({
            url: '/Home/SearchApps/',
            data: {
                searchText: searchInput.value
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
                        }
                        else {
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

    $("#closeAdvancedSearchButton").on("click", function () {
        window.location.href = '/Home/Index';
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
