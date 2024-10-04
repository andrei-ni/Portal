// Hide rows while searching
$(function () {
    var searchInput = document.getElementById("searchInput");
    searchInput.addEventListener("input", FilterLines);

    const closeButton = document.getElementById("closeButton");
    searchInput.addEventListener("input", function (event) {
        closeButton.style = "display: block; position: absolute;right: 10px; top: 50%;transform: translateY(-50%);";
        //console.log("User typed something:", event.target.value);
    });

    $("#closeButton").on("click", function () {
        location.reload();
    });
});
function FilterLines() {
    var input = document.getElementById("searchInput");
    var filter = input.value.toUpperCase();
    var table = document.getElementById("linesTableBody");
    var rows = table.getElementsByTagName("tr");

    for (var i = 0; i < rows.length; i++) {
        var empNameColumn = rows[i].getElementsByTagName("td")[0];
        var idColumn = rows[i].getElementsByTagName("td")[1];
        var roleColumn = rows[i].getElementsByTagName("td")[2];


        var empName = empNameColumn.textContent || empNameColumn.innerText;
        var id = idColumn.textContent || idColumn.innerText;
        var role = roleColumn.textContent || roleColumn.innerText;

        if (empName.toUpperCase().indexOf(filter) > -1
            || id.indexOf(filter) > -1
            || role.toUpperCase().indexOf(filter) > -1
        ) {
            rows[i].style.display = "";
        } else {
            rows[i].style.display = "none";
        }
    }
}

// Autocomplete

$(function () {

    $("#searchUserInput").autocomplete({
        source: function (request, response) { // response is the server response, request is the search term
            document.getElementById("spinner").style.display = "block"; // start the spinner here
            $.ajax({
                url: '/AppUser/AutocompleteSearchUsers/',
                data: { "searchText": request.term },
                type: "POST",
                success: function (data) {
                    document.getElementById("spinner").style.display = "none";
                    response($.map(data, function (item) {
                        return item;
                    }));

                    $(window).resize(function () {
                        $(".ui-autocomplete").css('display', 'none');
                    });

                },
                error: function (response) {
                    $("#searchUserInput").val("Error: " + response);
                },
                //failure: function (response) { // use error or failure
                //    $("#searchInput").val("Failure: " + response);
                //}
            });
        },
        select: function (e, i) {
            $("#userId").val(i.item.val).trigger('change');
            //$(this).autocomplete("close");
        },

    });


    // Add the user details into the fields (keep the code inside document ready)
    $('#userId').change(function () {
        var userId = $("#userId");
        document.getElementById("spinner").style.display = "block";

        $.ajax({
            url: '/AppUser/FindUser/',
            data: { "searchText": userId.val() },
            type: "POST",
            success: function (data) {
                document.getElementById("spinner").style.display = "none";
                var result = JSON.parse(data);

                var name = document.getElementById('Name');
                name.value = result.FullName;
                var empId = document.getElementById('EmployeeID');
                empId.value = result.EmployeeID;
                var adUsername = document.getElementById('AdUsername');
                adUsername.value = result.Username;

                var email = document.getElementById('Email');
                email.value = result.Email;

                var formDiv = document.getElementById('formDiv');
                formDiv.style.display = 'inline';
            },
            error: function (response) {
            },
            failure: function (response) {
            }
        });
    });
});




// DELETE
function AppUserDeleteFunction(clicked_id) {
    const button = clicked_id;
    var userId = button.getAttribute("user-Id");
    var userName = button.getAttribute("user-Name");
    var userBadge = button.getAttribute("user-BadgeNo");

    var modalUserId = document.getElementById("delModalUserId");
    modalUserId.value = userId;
    var modalUserName = document.getElementById("delModalName");
    modalUserName.value = userName;
    var modalBadgeNo = document.getElementById("delModalBadgeNo");
    modalBadgeNo.value = userBadge;

    $('#deleteModal').modal('show');
}

$("body").on("click", "#deleteBtn", function () {
    var modalUserId = $("#delModalUserId");

    $.ajax({
        type: "POST",
        url: "/AppUser/DeleteUser",
        data: {
            'id': modalUserId.val(),
        },
        /*dataType: 'json',*/
        success: function (result) {
            //console.log(result);
            if (result === "Success") {
                //$('#deleteModal').modal('hide');
                location.reload(); // don't reload if you want notification
                // window.location.href = location.origin + "/AppUsers"; 
            }
        },
        error: function (error) {
            console.log(error);
        }
    });
});