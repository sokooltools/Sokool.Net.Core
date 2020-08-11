// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.
$(document).ready(function () {
    $(`#navtab .nav-link`).on("click", function (e) {
        sessionStorage.setItem("navtab", $(e.target).attr("id"));
    });

    (function () {
        var current = getInitPath(window.location.pathname.toLowerCase());
        $(".navbar-nav a").each(function () {
            const $this = $(this);
            const next = getInitPath($this.attr("href")).toLowerCase();
            if (next === current) {
                $this.addClass("active");
                $this.tab("show");
            } else {
                $this.removeClass("active");
            }
        });

        function getInitPath(path) {
            // Special handling for "videos/show/xxxx"
            const idx = path.indexOf("/videos"); 
            if (idx > -1)
               return "/videos/index";
            const arr = path.split("/");
            if (arr.length > 2)
                return `/${arr[1]}/${arr[2]}`;
            else
                return "/";
        }
    })();
});
