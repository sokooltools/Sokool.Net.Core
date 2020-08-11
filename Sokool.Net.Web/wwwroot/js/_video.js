$(document).ready(function () {
    if ($("#Lyrics").html() === "") {
        $("#ShowHide").hide();
    } else {
        InitDialog();
    }

    // Re-initialize dialog on window resize.
    window.addEventListener("resize", InitDialog);

    $("div.ui-dialog").resize(function () {
        $("#Lyrics").css("width", "100%");
    });

    $(`#videotab .nav-link`).on("click", function (e) {
        sessionStorage.setItem("videotab", $(e.target).attr("href"));
    });

    (function () {
        const thetab = sessionStorage.getItem("videotab");
        if (thetab) {
            $(`.nav-tabs a[href="${thetab}"]`).tab("show");
        }
    })();
});

function InitDialog() {
    $("#Lyrics").dialog({ autoOpen: false, zIndex: 999999, show: "slide", hide: "slide" });
    $("#Lyrics").dialog("option", "closeOnEscape", true);
    $("#Lyrics").bind("dialogopen",
        function () { $("#ShowHide").attr("value", "Hide Lyrics").html("Hide Lyrics"); });
    $("#Lyrics").bind("dialogclose",
        function () { $("#ShowHide").attr("value", "Show Lyrics").html("Show Lyrics"); });
    $("#Lyrics").dialog("option", "title", "Lyrics: "); // + $("h1").text()
    $("#Lyrics").dialog("option", "maxWidth", $(window).width() - 20);
    $("#Lyrics").dialog("option", "position", ["left", "top"]);

    $("div.ui-dialog").css("left", "5px");

    const top = $("#videotab").position().top + $("#videotab").height() + 5;
    $("div.ui-dialog").css("top", top + "px");

    const wdth = $(window).width() * 0.5;
    $("#Lyrics").dialog("option", "width", wdth + "px");

    const hght = $(document).height() - top - $("footer").height() - 5;
    $("#Lyrics").dialog("option", "height", hght);

    $("#Lyrics").css("width", "100%");
};

function OpenDialog() {
    if ($("#ShowHide").attr("value") === "Hide Lyrics") {
        $("#Lyrics").dialog("close");
    }
    else {
        $("#Lyrics").dialog("open");
    }
    return false;
};

function GoBack() {
    const path = $("#videotab a.active").attr("href") || "/Videos/Index/Across";
    setTimeout(function () { document.location.href = path; }, 250);
}

function getFileExtensionFromPath(path) {
    const filename = path.split("\\").pop().split("/").pop();
    return filename.substr((Math.max(0, filename.lastIndexOf(".")) || Infinity) + 1);
}

function getFileNameFromPath(path) {
    var slash = "/";
    if (path.match(/\\/))
        slash = "\\";
    return path.substring(path.lastIndexOf(slash) + 1, path.lastIndexOf("."));
}