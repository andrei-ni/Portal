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
    const textarea = document.getElementById('detailsInput');
    const charCountElement = document.getElementById('charCount');
    const remainingChars = 256 - textarea.value.length;
    charCountElement.textContent = remainingChars;
}


// INDEX
function MoveLine() {
    const firstList = document.getElementById('firstListSelect');
    const secondList = document.getElementById('secondListSelect');

    // Move all selected options from firstList to secondList
    const selectedOptions = Array.from(firstList.selectedOptions);
    selectedOptions.forEach((option) => {
        secondList.appendChild(option);
    });
}
function RemoveLine() {
    const firstList = document.getElementById('firstListSelect');
    const secondList = document.getElementById('secondListSelect');

    // Move all selected options from secondList back to firstList
    const selectedOptions = Array.from(secondList.selectedOptions);
    selectedOptions.forEach((option) => {
        firstList.appendChild(option);
    });
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
            dataurl = canvas.toDataURL(file.type); //, 0.5 quality
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

let appEntityDictionary = {
    'EN': { Name: '', Description: '', LanguageCode: '' },
    'RO': { Name: '', Description: '', LanguageCode: '' },
    'JA': { Name: '', Description: '', LanguageCode: '' }
};

function ChangeContent(lang) {
    var nameInput = document.getElementById('nameInput');
    var detailsInput = document.getElementById('detailsInput');

    // Save current input values before changing language
    var currentLang = document.getElementById('currentLang').value;
    appEntityDictionary[currentLang].Name = nameInput.value;
    appEntityDictionary[currentLang].Description = detailsInput.value;
    appEntityDictionary[currentLang].LanguageCode = currentLang;

    //switch (lang) {
    //    case 'RO':
    //        nameInput.placeholder = "Introduceți numele aplicației";
    //        detailsInput.placeholder = "Introduceți detaliile aplicației";
    //        break;
    //    case 'EN':
    //        nameInput.placeholder = "Enter application name";
    //        detailsInput.placeholder = "Enter application details";
    //        break;
    //    case 'JA':
    //        nameInput.placeholder = "アプリケーション名を入力してください";
    //        detailsInput.placeholder = "アプリケーションの詳細を入力します";
    //        break;
    //}

    nameInput.value = appEntityDictionary[lang].Name;
    detailsInput.value = appEntityDictionary[lang].Description;
    document.getElementById('currentLang').value = lang;
}

let keywords = [];
function AddKeyword(value) {
    // add them in a column in new divs
    //var newDiv = document.createElement("div");
    //var newContent = document.createTextNode(value);
    //newDiv.appendChild(newContent);
    //keywordsContainer.appendChild(newDiv);

    // add them in spans
    value = value.replace(/\s/g, '');
    const i = document.createElement('i');
    i.className = "bi bi-x";

    var keywordsContainer = document.getElementById("keywordsContainer");
    const span = document.createElement('span');
    span.setAttribute('role', 'button');

    if (keywords.includes(value)) {
        Toastify({
            text: "Keyword exists",
            duration: 3000,
            newWindow: true,
            close: true,
            gravity: "bottom", // `top` or `bottom`
            position: "left", // `left`, `center` or `right`
            stopOnFocus: true, // Prevents dismissing of toast on hover
            style: {
                background: "linear-gradient(to right, #008A99, #55B1BB)",
            },
        }).showToast();
    } else {
        keywords.push(value); // array used to send them to the controller

        span.textContent = value;
        span.appendChild(i); // the close icon
        span.appendChild(document.createTextNode(' ')); // add a space
    }

    span.addEventListener('click', function () {
        var keyword = this.parentNode.removeChild(this);
        keywords.splice(keyword, 1);
    });


    keywordsContainer.appendChild(span);

    document.getElementById("keywordsInput").value = ""; // clear the input field
    //console.log(keywords);
}

function SaveApplication() {
    var nameInput = document.getElementById('nameInput');
    var detailsInput = document.getElementById('detailsInput');
    var devLink = document.getElementById('devLink');
    var appLink = document.getElementById('appLink');
    var manualLink = document.getElementById('manualLink');

    const secondList = document.getElementById('secondListSelect');
    const selectedValues = Array.from(secondList.selectedOptions).map(option => option.value);

    // Save current language input values before saving
    var currentLang = document.getElementById('currentLang').value;
    appEntityDictionary[currentLang].Name = nameInput.value;
    appEntityDictionary[currentLang].Description = detailsInput.value;
    appEntityDictionary[currentLang].LanguageCode = currentLang;

    if (nameInput.value === "" || detailsInput.value === "" || dataurl === null
        || devLink.value === "" || appLink.value === "" || manualLink.value === "")
    {
        $.ajax({
            url: '/Application/SaveApplication',
            type: 'POST',
            data:
            {
                "content": null,
                "icon": null,
                "linkDev": null,
                "linkApp": null,
                "linkManual": null

            },
            success: function (response) {
                console.log(response);
            },
            error: function (error) {
                console.log(error);
            }
        });
    }
    else {
        $.ajax({
            url: '/Application/SaveApplication',
            type: 'POST',
            data:
            {
                "selectedOptions": selectedValues,
                "content": appEntityDictionary,
                "icon": dataurl,
                "linkDev": devLink.value,
                "linkApp": appLink.value,
                "linkManual": manualLink.value,
                "kwords": keywords
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

