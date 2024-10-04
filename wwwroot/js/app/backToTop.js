$(document).ready(function () {
    document.getElementById("backToTop").addEventListener("mouseover", function () {
        this.className = "bi bi-arrow-up btt-fill";
    });

    document.getElementById("backToTop").addEventListener("mouseout", function () {
        this.className = " bi bi-arrow-up btt";
    });
});

window.onscroll = function () { scrollFunction() };
function scrollFunction() {
    if (document.body.scrollTop > 20 || document.documentElement.scrollTop > 20) {
        if (document.getElementById("backToTop") != null) {
            document.getElementById("backToTop").style.display = "block";
        }
    } else {
        if (document.getElementById("backToTop") != null) {
            document.getElementById("backToTop").style.display = "none";
        }
    }
}
function topFunction() {
    document.body.scrollTop = 0; // For Safari
    document.documentElement.scrollTop = 0; // For Chrome, Firefox, IE and Opera
}