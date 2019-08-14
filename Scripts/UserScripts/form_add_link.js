
//ФОРМА ДОБАВЛЕНИЯ ССЫЛОК
//при добавлении ссылки ее можно добавить у существующую категорию, или создать новую категорию
$("body").on("change", "#FormAddLink #category", function () {
    var select_category = $("#category").val();
    //если выбираем существующую категорию
    if (select_category != 0) {
        //то удаляем поле ввода новой категории что бы его не было видно
        $("#Name").attr("disabled", "disabled").addClass("form_disable").removeAttr("placeholder");
    } else {
        //иначе если в списке категорий выбрано поле по умолчанию то показываем поле ввода новой категории
        $("#Name").removeAttr("disabled").removeClass("form_disable").attr({ placeholder: "Введите новую категорию", required: "" });
    }
});

var new_category;
//ховаем или показываем выпадаючий список с существующими категориями
$("body").on("keyup", "#FormAddLink #Name", function () {
    //при каждом нажатии клавиши, тоесть при вводе текста в поле новой категории
    new_category = $("#Name").val();
    //проверяем поле на пустую строку, если в поле какой то текст ввели
    if (new_category != "") {
        //убираем выпадаючий список существующих категорий
        $("#category").attr("disabled", "disabled").addClass("form_disable");
    } else {
        //иначе показываем его если поле ввода новой категории пустое
        $("#category").removeAttr("disabled").removeClass("form_disable").attr({ required: "" });
    }
});

function OnCompleteAddLink() {

    $("#Name").removeAttr("disabled").removeClass("form_disable").attr({ placeholder: "Введите новую категорию", required: "" });
    $("#category").removeAttr("disabled").removeClass("form_disable").attr({ required: "" });

    document.getElementById('FormAddLink').reset();

    var id = $("#id_new_add_cat").val();
    var name = $("#name_new_add_cat").val();

    if (id != "") {
        $("#category option:nth-child(1)").after($('<option value="' + id + '">' + name + '</option>'));
    }

    $("#tabs_share").removeClass("active_tab");
    $("#tab_links").addClass("active_tab");

    EnterFirstCategory();
}

//ограничение на длину названия новой категории
var max_symbols_name_cat = 30;
$(document).on("keyup", ".add_link #Name", function () {
    if (this.value.length > max_symbols_name_cat) {
        this.value = this.value.substr(0, max_symbols_name_cat);
        $("#category").css({ marginBottom:"0px"});
        $(".add_link .errors").text("достигнута максимальная длина символов");
    } else {
        $(".add_link .errors").text("");
        $("#category").css({ marginBottom: "17px" });
    }
});

//ограничение на длину описания к ссылке
var max_symbols_desc = 500;
$(document).on("keyup", ".add_link #Description", function () {
    if (this.value.length > max_symbols_desc) {
        this.value = this.value.substr(0, max_symbols_desc);
        $("#Link").css({ marginBottom: "0px" });
        $(".add_link .error_description").text("достигнута максимальная длина символов");
    } else {
        $(".add_link .error_description").text("");
        $("#Link").css({ marginBottom: "17px" });
    }
});

