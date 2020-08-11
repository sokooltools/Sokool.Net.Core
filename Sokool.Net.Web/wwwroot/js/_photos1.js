function display(img) {
    var modal = $("#divimage");
    modal.css("display", "block");
    $("#modalimg").attr("src", img.src);
    $("#caption").html(img.alt);
    modal.click(function () {
        modal.css("display", "none");
    });
}

$(document).ready(function() {
    $(`#phototab .nav-link`).on("click", function (e) {
        sessionStorage.setItem("phototab1", $(e.target).attr("href"));
    });

    (function() {
        var current = window.location.pathname.toLowerCase();
        $("#phototab a").each(function () {
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
