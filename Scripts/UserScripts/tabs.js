
$("body").on("click", ".tabs", function (e) {

    switch (e.target.id) {
        case "tab_links":
            $("#tabs_share").removeClass("active_tab");
            $("#tab_links").addClass("active_tab");
            break;
        case "tabs_share":
            $("#tab_links").removeClass("active_tab");
            $("#tabs_share").addClass("active_tab");
    }

    if ($(".save_share_link_form").is(":visible")) {

        $(".save_share_link_form").slideUp(300, function () {
            $(".add_link").slideDown(300);
        });

    } else if ($(".share_link").is(":visible")) {

        $(".share_link").slideUp(300, function () {
            $(".add_link").slideDown(300);
        });
    }

    EnterFirstCategory();

    //обнуляем поля форм
    ResetForms();
});