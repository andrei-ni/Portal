$(document).ready(function () {
    const aoval = document.querySelector('.aoval');
    const boval = document.querySelector('.boval');

    const robtn = document.getElementById('robtn');
    const enbtn = document.getElementById('enbtn');
    const jpbtn = document.getElementById('jpbtn');

    robtn.addEventListener('click', () => {
        aoval.style.left = '0';
        boval.style.left = '0';
    });

    enbtn.addEventListener('click', () => {
        aoval.style.left = '33%';
        boval.style.left = '33%';
    });

    jpbtn.addEventListener('click', () => {
        aoval.style.left = '70%';
        boval.style.left = '70%';
    });

});

function updateCharacterCount() {
    const detailsInput = document.getElementById('detailsInput');
    const charCountElement = document.getElementById('charCount');
    const remainingChars = 100 - detailsInput.value.length;
    charCountElement.textContent = remainingChars;
}

let dataurl; // the icon base64 image
function allowDrop(ev) {
    ev.preventDefault();
}

function drop(ev) {
    ev.preventDefault();
    var data = ev.dataTransfer.items;
    for (var i = 0; i < data.length; i++) {
        if (data[i].kind === 'file') {
            var file = data[i].getAsFile();
            previewDroppedFile(file);
        }
    }
}

function previewFile() {
    var file = document.querySelector('input[type=file]').files[0];
    previewDroppedFile(file);
}

function previewDroppedFile(file) {
    var preview = document.getElementById('iconPreview');
    var reader = new FileReader();

    // Resize the image 
    // Maintain the aspect ratio (shorter img string)
    reader.onloadend = function () {
        var img = new Image();
        img.src = reader.result;
        img.onload = function () {
            var canvas = document.createElement('canvas');
            var ctx = canvas.getContext('2d');
            canvas.width = 64;
            canvas.height = 64;

            // Calculate the scale factor to maintain aspect ratio
            var scaleFactor = Math.min(canvas.width / img.width, canvas.height / img.height);

            // Calculate the top left position of the image
            var x = (canvas.width - img.width * scaleFactor) / 2;
            var y = (canvas.height - img.height * scaleFactor) / 2;

            // Draw the image on the canvas
            ctx.drawImage(img, x, y, img.width * scaleFactor, img.height * scaleFactor);

            // Convert the canvas to a data URL
            dataurl = canvas.toDataURL(file.type); // , 0.5
            preview.innerHTML = '<img src="' + dataurl + '" style="height:100%; width:100%;padding:15px;"/>';

        }
    }

    // No aspect ratio (longer img string)
    //reader.onloadend = function () {
    //    var img = new Image();
    //    img.src = reader.result;
    //    img.onload = function () {
    //        var canvas = document.createElement('canvas');
    //        var ctx = canvas.getContext('2d');
    //        canvas.width = 64;
    //        canvas.height = 64;
    //        ctx.drawImage(img, 0, 0, 64, 64);
    //        dataurl = canvas.toDataURL(file.type); // can reduce quality to 0.5 (file.type, 0.5)
    //        preview.innerHTML = '<img src="' + dataurl + '" style="height:100%; width:100%;padding:15px;"/>';
    //    }
    //}

    //reader.onloadend = function () {
    //    preview.innerHTML = '<img src="' + reader.result + '" style="height:100%; width:100%; padding:15px;"/>';
    //}

    if (file) {
        reader.readAsDataURL(file);
    } else {
        preview.value = "";
    }
}

function deleteIcon() {
    document.getElementById('iconPreview').innerHTML = "Icon preview";
    document.getElementById('fileUpload').value = "";
    dataurl = null;
}

//function uploadImage() {
//    var fileInput = $('#fileUpload')[0];
//    var file = fileInput.files[0];
//    var formData = new FormData();
//    formData.append('file', file);

//    $.ajax({
//        url: '/Category/UploadIcon',
//        type: 'POST',
//        data: formData,
//        processData: false,  // tell jQuery not to process the data
//        contentType: false   // tell jQuery not to set contentType
//    }).done(function () {
//        console.log("Image uploaded successfully!");
//    }).fail(function () {
//        console.error("Image upload failed!");
//    });
//}

// EDIT
// On initialize, remove the second list options from the first list
let addIds = [];
let removeIds = [];

$(document).ready(function () {
    $('#editFirstListSelect option').each(function () {
        var optionValue = $(this).val();
        if ($('#editSecondListSelect option[value="' + optionValue + '"]').length) {
            $(this).remove();
        }
    });
});
function EditMoveLine() {
    const firstList = document.getElementById('editFirstListSelect');
    const secondList = document.getElementById('editSecondListSelect');

    // Move all selected options from firstList to secondList
    const selectedOptions = Array.from(firstList.selectedOptions);
    selectedOptions.forEach((option) => {
        if (!secondList.contains(option)) {
            secondList.appendChild(option);
            addIds.push(option.value);
        }
    });

}

function EditRemoveLine() {
    const firstList = document.getElementById('editFirstListSelect');
    const secondList = document.getElementById('editSecondListSelect');

    // Move all selected options from secondList back to firstList
    const selectedOptions = Array.from(secondList.selectedOptions);
    selectedOptions.forEach((option) => {
        firstList.appendChild(option);
        removeIds.push(option.value);
    });

}

let categoryEntityDictionary = {
    'EN': { Name: '', Description: '', LanguageCode: '' },
    'RO': { Name: '', Description: '', LanguageCode: '' },
    'JA': { Name: '', Description: '', LanguageCode: '' }
};

// On initialize
$(document).ready(function () {
    var nameInputRO = document.getElementById('nameInput');
    var detailsInputRO = document.getElementById('detailsInput');
    var nameInputEN = document.getElementById('nameInputEN');
    var detailsInputEN = document.getElementById('detailsInputEN');
    var nameInputJA = document.getElementById('nameInputJA');
    var detailsInputJA = document.getElementById('detailsInputJA');

    categoryEntityDictionary = {
        'EN': { Name: nameInputEN.value, Description: detailsInputEN.value, LanguageCode: 'EN' },
        'RO': { Name: nameInputRO.value, Description: detailsInputRO.value, LanguageCode: 'RO' },
        'JA': { Name: nameInputJA.value, Description: detailsInputJA.value, LanguageCode: 'JA' }
    };

    var preview = document.getElementById('existingIconPreview');
    dataurl = preview.src;

});
function ChangeContent(lang) {
    var nameInput = document.getElementById('nameInput');
    var detailsInput = document.getElementById('detailsInput');

    // Save current input values before changing language
    var currentLang = document.getElementById('currentLang').value;
    categoryEntityDictionary[currentLang].Name = nameInput.value;
    categoryEntityDictionary[currentLang].Description = detailsInput.value;
    categoryEntityDictionary[currentLang].LanguageCode = currentLang;

    //switch (lang) {
    //    case 'RO':
    //        nameInput.placeholder = "Introduceți numele categoriei";
    //        detailsInput.placeholder = "Introduceți detaliile categoriei";
    //        break;
    //    case 'EN':
    //        nameInput.placeholder = "Enter category name";
    //        detailsInput.placeholder = "Enter category details";
    //        break;
    //    case 'JA':
    //        nameInput.placeholder = "カテゴリ名を入力してください";
    //        detailsInput.placeholder = "カテゴリの詳細を入力してください";
    //        break;
    //}

    nameInput.value = categoryEntityDictionary[lang].Name;
    detailsInput.value = categoryEntityDictionary[lang].Description;
    document.getElementById('currentLang').value = lang;
}


function UpdateCategory() {
    var nameInput = document.getElementById('nameInput');
    var detailsInput = document.getElementById('detailsInput');

    var catId = document.getElementById('catId');

    // Save current input values before saving
    var currentLang = document.getElementById('currentLang').value;
    categoryEntityDictionary[currentLang].Name = nameInput.value;
    categoryEntityDictionary[currentLang].Description = detailsInput.value;
    categoryEntityDictionary[currentLang].LanguageCode = currentLang;

    if (nameInput.value === "" || detailsInput.value === "" || dataurl === null) {
        $.ajax({
            url: '/Category/UpdateCategory',
            type: 'POST',
            data:
            {
                "content": null,
                "icon": null,
                "categoryId": catId.value
            },
            success: function (response) {
                //console.log(response);
            },
            error: function (error) {
                console.log(error);
            }
        });
    }
    else {
        $.ajax({
            url: '/Category/UpdateCategory',
            type: 'POST',
            data:
            {
                "content": categoryEntityDictionary,
                "icon": dataurl,
                "categoryId": catId.value,
                "addAppsIds": addIds,
                "removeAppsIds": removeIds
            },
            success: function (response) {
                if (response === "Success") {
                    document.location.href = "/";
                }
                //console.log(response);
            },
            error: function (error) {
                console.log(error);
            }
        });
    }

}
// Delete Category
$(function () {
    $("#confirmBtn").on("click", function () {
        let data = document.querySelector('[data-categoryid');
        let catId = data.getAttribute('data-categoryid');

        $.ajax({
            url: '/Category/DeleteCategory',
            type: 'POST',
            data: { "categoryId": catId },
            success: function (response) {
                if (response === "Success") {
                    document.location.href = "/";
                }
            },
            error: function (error) {
                console.log(error);
            }
        });

    });
});




