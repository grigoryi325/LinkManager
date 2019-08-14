var mass_shared_link = [];
var filter_mass = [];
var filter_mass_length = 0;
var c = 0;

function SahredLinks() {

    $(document).on("click", "#workzone input[type=checkbox]", function (e) {

        if ($(e.target).is(":checked") == true) {

            var checkmass = 0;

            if ($(e.target).is("input[type=checkbox]")) {

                $("#workzone input[type=checkbox]").each(function () {

                    if ($(this).is(":checked") == true) {

                        for (var i = 0; i < mass_shared_link.length; i++) {

                            if ($(e.target).attr("id") == mass_shared_link[i]) {

                                checkmass = 1;
                            }
                        }
                        if (checkmass == 0) {
                            mass_shared_link[c] = $(e.target).attr("id");
                            c++;
                        }
                    }
                });

                //подсвечиваем выбранные ссылки
                for (var i = 0; i < mass_shared_link.length; i++) {
                    $("#workzone #" + mass_shared_link[i] + " .link").addClass("active_shared_link");
                    $("#workzone #" + mass_shared_link[i] + " .description").addClass("active_shared_link");
                }

                 if (filter_mass.Length != 0) {
                    if ($(".add_link").is(":visible")) {

                        $(".add_link").slideUp(300, function () {

                            $(".share_link").slideDown(300);

                            document.getElementById('FormAddLink').reset();
                        });
                    }
                 }
            }

        }
        else
        {
            for (var i = 0; i < mass_shared_link.length; i++)
            {
                if ($(e.target).attr("id") == mass_shared_link[i])
                {
                    mass_shared_link.splice(i, 1);

                    $("#workzone #" + $(e.target).attr("id") + " .link").removeClass("active_shared_link");
                    $("#workzone #" + $(e.target).attr("id") + " .description").removeClass("active_shared_link");

                }
            }
        }

        //удаляем пустые элементы с масива
        filter_mass = mass_shared_link.filter(function (e) {
            return e != "";
        });

        filter_mass_length = filter_mass.length;

        //если масив пустой то показываем убираем форму розшаривания ссылок и показываем форму добавление ссылок
        CheckNullSharedLinks();

        $("#shared_links").val(filter_mass);
    });

        /////////////////////////////SHARE ERROR DESTANARION///////////////////////////////////

        //if (Session["error_sare"] != 0) {
        //    $("#Recipient").val(Session["error_sare"]);
        //    $("#Recipient").css({ borderColor: "red", color: "red" });
        //}
   
}

///////////////////////////Обработка кнопки формы при розшаривании ссылок//////////////////////////////////

$(document).on("click", "#share_links", function () {
    //очищаем масив з id ссылок
    filter_mass.length = 0;
    CheckNullSharedLinks();
    $("#tab_links").removeClass("active_tab");
    $("#tabs_share").addClass("active_tab");
});

function RemoveShareLink() {
    $(document).on("click", ".remove_share_link", function (e) {
        $("#workzone #" + e.target.id).fadeOut(300, function () {
            $(this).remove();
        });

    });

}

RemoveShareLink();

function NewSharedMessages() {
    $(".row").each(function () {
        if ($(this).attr("newlinks") == "new") {
            $(this).addClass("light");
            console.log($(this).attr("id"));
            console.log("сработало");
        }
    });
}