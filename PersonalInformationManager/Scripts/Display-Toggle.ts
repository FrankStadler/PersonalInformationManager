/// <reference path="typings/jquery/jquery.d.ts" />
// Display-Toggle.js
// ---------------------------------------
// applies a pattern for hiding a div via a toggle button.
// button and target div must have matching naming conventions.
// text on the button and visibility of the target div toggle back and forth.
// ----------------------------------------


function showBox(boxName: string, displayName: string) {
    var btnId = "#" + boxName + "ToggleButton";
    var box = $("#" + boxName);
    $(btnId).addClass("active");
    $(btnId).text("Hide " + displayName + " \xBB");
    box.removeClass("hidden");
    box.show();
}

function hideBox(boxName: string, displayName: string) {
    var btnId = "#" + boxName + "ToggleButton";
    var box = $("#" + boxName);
    $(btnId).removeClass("active");
    $(btnId).text("Show " + displayName + " \xBB");
    box.hide();
}

function DHS_ToggleBox(boxName: string, displayName: string) {
    if ($("#" + boxName + "ToggleButton").hasClass("active") === false) {
        showBox(boxName, displayName);
    } else {
        hideBox(boxName, displayName);
    }
}