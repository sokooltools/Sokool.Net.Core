function goBack() {
    setTimeout(function () { window.history.go(-1); }, 250);
};

$(function () {
    $("#back").on("click", function () {
        goBack();
        return false;
    });
});
