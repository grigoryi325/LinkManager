///////////обработка кнопки сохранения блока gпересланных ссылок////////////
var previus_block_links = 0;

    $(document).on("click", ".save_share_link", function (e) {

        var share_id = e.target.id;

        if (previus_block_links != 0) {
            $("#workzone #" + previus_block_links).css({ backgroundColor: "#464646" });
            $("#workzone #" + share_id).css({ backgroundColor: "#03a9f473" });
            previus_block_links = share_id;
        } else {
            $("#workzone #" + share_id).css({ backgroundColor: "#03a9f473" });
            previus_block_links = share_id;
        }

        $("#workzone #" + share_id).css({ backgroundColor: "#03a9f473" });

        previus_block_links = share_id;

        $("#ShareID").val(share_id);

        if ($(".add_link").is(":visible")) {

            $(".add_link").slideUp(300, function () {
                $(".save_share_link_form").slideDown(300);
            });

        } else if ($(".share_link").is(":visible")) {

            $(".share_link").slideUp(300, function () {
                $(".save_share_link_form").slideDown(300);
            });
        }
    });


//проверка поля ввода новой категории на совпадение с существующими названиями категорий
var stop;
$(document).on("keyup", ".save_share_link_form #NameCategory", function () {

    var name__new_cat = $(this).val();

    stop = 0;

    $("#Category_Id option").each(function () {
        
        if ($(this).text() == name__new_cat) {
            $(".errors").text("категория с таким именем уже существует");
            $("#save_share_link").attr("disabled", "disabled");
            stop = 1;
        }
        else {
            if (stop == 0) {
                $(".errors").text("");
                $("#save_share_link").removeAttr("disabled");
                stop = 1;
            }
        }
     
    });
});



/////////////обработка кнопки формы///////////////
$(document).on("click", "#save_share_link", function () {

    $("#workzone #" + previus_block_links).css({ backgroundColor: "#464646" });

    $(".save_share_link_form").slideUp(300, function () {

        //$(".add_link").slideDown(300);
        //document.getElementById('FormSaveSharedLinks').reset();
        ResetForms();
    });

});


//ФОРМА СОХРАНЕНИЯ ПЕРЕСЛАННЫХ ССЫЛОК
//при добавлении ссылки ее можно добавить у существующую категорию, или создать новую категорию
$(document).on("change", "#Category_Id", function () {
    var select_category = $("#Category_Id").val();
    //если выбираем существующую категорию
    if (select_category != 0) {
        //то удаляем поле ввода новой категории что бы его не было видно
        $("#NameCategory").attr("disabled", "disabled").addClass("form_disable").removeAttr("placeholder");
    } else {
        //иначе если в списке категорий выбрано поле по умолчанию то показываем поле ввода новой категории
        $("#NameCategory").removeAttr("disabled").removeClass("form_disable").attr({ placeholder: "Введите новую категорию", required: "" });
    }
});

//ховаем или показываем выпадаючий список с существующими категориями
$(document).on("keyup", "#NameCategory", function () {
    //при каждом нажатии клавиши, тоесть при вводе текста в поле новой категории
    var new_category = $("#NameCategory").val();
    //проверяем поле на пустую строку, если в поле какой то текст ввели
    if (new_category != "") {
        //убираем выпадаючий список существующих категорий
        $("#Category_Id").attr("disabled", "disabled").addClass("form_disable");
    } else {
        //иначе показываем его если поле ввода новой категории пустое
        $("#Category_Id").removeAttr("disabled").removeClass("form_disable").attr({ required: "" });
    }
});


//ограничение на длину названия новой категории
var max_symbols = 30;
$(document).on("keyup", "#NameCategory", function () {
    console.log("+");
    if (this.value.length > max_symbols) {
        this.value = this.value.substr(0, max_symbols);
        $(".save_share_link_form #Category_Id").css({ marginBottom: "0px" });
        $(".save_share_link_form .errors").text("достигнута максимальная длина символов").css({fontSize:"13px"});
    } else {
        $(".save_share_link_form .errors").text("");
        $(".save_share_link_form #Category_Id").css({ marginBottom: "17px" });
    }
});