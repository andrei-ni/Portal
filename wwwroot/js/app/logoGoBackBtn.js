$(document).ready(function () {
    //console.log(window.location.origin === window.location.href.slice(0, -1));
    if (window.location.origin !== window.location.href.slice(0, -1)) { // if it's not the home page read the id
        $("#logo").hover(
            function () {
                document.getElementById("home-btn").style = "transition: 0.3s; opacity: 1;";
                /*$(".home-btn").show();*/
            }, function () {
                document.getElementById("home-btn").style = "transition: 0.3s; opacity: 0;";
                /*$(".home-btn").hide();*/
            }
        );
    }
});