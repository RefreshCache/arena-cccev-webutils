$(document).ready(function() {
    initCustomForms();
});

function initCustomForms() {
    replaceCheckBoxes();
}

function replaceCheckBoxes() {
    $(".check-box input[id$='cbRemember'], .checkboxArea input[id$='cbRemember']").not(".replaced").each(function()
    {
        $(this).addClass("replaced");
        var newCheck = $("<div></div>");
        newCheck
            .addClass(this.checked ? "checkboxAreaChecked" : "checkboxArea")
            .attr("id", newCheck.attr("id") + "Custom")
            .insertBefore(this)
            .data("oldCheck", this)
            .click(toggleCheckBox).keydown(toggleCheckBox);
        $("label[for='" + $(this).attr("id") + "']").click(toggleCheckBox);
    });
}

function toggleCheckBox() {
    var oldCheck = $($(this).data("oldCheck"));
    if (oldCheck.attr("checked")) {
        $(this).removeClass("checkboxAreaChecked").addClass("checkboxArea")
        oldCheck.attr("checked", false);
    }
    else {
        $(this).removeClass("checkboxArea").addClass("checkboxAreaChecked")
        oldCheck.attr("checked", true);
    }
}