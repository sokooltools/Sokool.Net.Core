$(document).ready(function () {
    $(`#videotab .nav-link`).on("click", function (e) {
        sessionStorage.setItem("videotab", $(e.target).attr("href"));
    });

    (function () {
        const current = window.location.pathname.toLowerCase();
        $("#videotab a").each(function () {
            const $this = $(this);
            if ($this.attr("href").toLowerCase() === current) {
                $this.addClass("active");
                $this.tab("show");
            } else {
                $this.removeClass("active");
            }
        });
    })();
});
