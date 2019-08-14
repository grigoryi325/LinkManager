var last_category;
var last_cat_id;
var check_menu_html = 0;
var id_click_cat;
var check = 0;
var edit_id_cat;


//при первой загрузки страныци активным выбираем первую вкладку ()
function EnterFirstCategory() {
    $(".ul_category li:nth-child(2) a").css({
        backgroundColor: "red",
        color: "white"
    });
}


$(".ul_category li:nth-child(2) a").css({
    backgroundColor: "red",
    color: "white"
});

last_cat_id = $(".ul_category li:nth-child(2)").attr("id");

//подсвечивание категории на которой мы находимся
$(document).on("click", ".ul_category li a", function (e) {

    //при переходе на новую категорию обнуляем масив ссылок для разшаривания,
    //тоесть за раз можно розшарить ссылки только с одной категории
    mass_shared_link.length = 0;

    //вызываем функцию проверки какой блок представления открыть разшаривания или добавление ссылок
    CheckNullSharedLinks();
    //при переключении обнуляем поля всех форм
    ResetForms();

    $(".ul_category li:nth-child(2) a").removeAttr("style");

    $(last_category).removeAttr("style");

    $(e.target).css({
        backgroundColor: "red",
        color: "white"
    });

    last_category = $(e.target);
    if (edit_id_cat != id_click_cat){
        $(".ul_category #" + id_click_cat + " ul").css({ display: "block" }).addClass("SlideUp").removeClass("SlideDown");
}
    check_menu_html = 0;

    setTimeout(Delay, 500); 

    setTimeout(function () {
        id_click_cat = 0;
    }, 500);
});


$(document).on("click", ".ul_category img", function (e) {

    if ((check_menu_html == 0)) {
     
        $(".ul_category #" + e.target.id + " ul").addClass("SlideDown").removeClass("SlideUp");

        id_click_cat = e.target.id;

        check_menu_html = 1;

    } else if ((check_menu_html == 1) && (id_click_cat != e.target.id)){

        $(".ul_category #" + e.target.id + " ul").css({ display: "block" }).addClass("SlideDown").removeClass("SlideUp");
        $(".ul_category #" + id_click_cat + " ul").css({ display: "block" }).addClass("SlideUp").removeClass("SlideDown");

        setTimeout(Delay, 500);

        check_menu_html = 1;

        setTimeout(function () {
            id_click_cat = e.target.id;
        }, 500);
        
         
    } else {
        $(".ul_category #" + id_click_cat + " ul").css({ display: "block" }).addClass("SlideUp").removeClass("SlideDown");
        check_menu_html = 0;
        setTimeout(Delay, 500);      
    }

    
});

function Delay() {
    $(".ul_category #" + id_click_cat + " ul").removeAttr("style");
}

//$(document).on("click", "#last_menu li img", function (e) {
//    if (check === 0) {
//        $("#" + e.target.id + " .last_menu").addClass("SlideDownMenu").removeClass("SlideUpMenu");
//        check = 1;
//    } else {
//        $("#" + e.target.id + " .last_menu").addClass("SlideUpMenu").removeClass("SlideDownMenu");
//        check = 0;
//    }
//});


$(document).on('click', '.edit', function (e) {

    var name_category = $(".ul_category ." + e.target.id + " a").text();

    $(".ul_category #" + e.target.id).html("<form data-ajax-success='OnCompleteEditCat' id='ed' action='/User/EditCategory/" + e.target.id + "' data-ajax='true' data-ajax-mode='replace' data-ajax-update='#" + e.target.id + "' method='post'><input type='text' name='Name' id='Name' value='" + name_category + "' required='' /><input class='edit_category' type='submit' value='ok'></form>");
    $("#" + e.target.id + " .last_menu").addClass("SlideUpMenu").removeClass("SlideDownMenu").removeAttr("style");
    //check = 0;
    edit_id_cat = e.target.id
    check_menu_html = 0;
});

///////если редактируем выбраную категорию то оставляем ее выделеной красным иначе белой
function OnCompleteEditCat() {
    if (Number(last_cat_id) == id_click_cat) {
        $(".ul_category #"+id_click_cat+" a").css({
            backgroundColor: "red",
            color: "white"
        }); 
        last_category = $(".ul_category #" + id_click_cat + " a");
    }
}

//ограничение на длину названия вкладки
var max_symbols = 30;
$(document).on("keyup", "#ed #Name", function () {
    console.log("+");
    if (this.value.length > max_symbols) {
        this.value = this.value.substr(0, max_symbols);

    }
});

///////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////окно подтверждения удаления категории ссылок/////////////////////////////

$(document).on('click', '#delete', function () {
    $(".window_del_cat").css({ display: "block" });
    $("#window").removeClass("cancel_blur").addClass("blur");
    $("#name_del_cat").text($(this).attr("name"));
    $(".window_del_cat").append('<p align="center"><a href="/User/DelCategory/' + id_click_cat + '">Удалить</a> <a href="#" onclick="CancelDelCategory(this)" id="#cancel">Отмена</a></p>');
});


/////////////////////////////кнопка отмены окна подтверждения удаления категории ссылок/////////////////////////////
function CancelDelCategory() {
    $(".window_del_cat").css({ display: "none" });
    $(".window_del_cat p:last").remove();
    $("#window").removeClass("blur").addClass("cancel_blur");
}

/////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////окно подтверждения удаления чата ссылок/////////////////////////////

$("body").on('click', '.btn_del_chat img', function (e) {
    $(".window_del_chat").css({ display: "block" });
    $("#window").removeClass("cancel_blur").addClass("blur");
    $("#name_del_chat").text(e.target.id);
    $(".window_del_chat").append('<p align="center"><a href="/User/DelChat/?Sender=' + id_click_cat + '">Удалить</a> <a href="#" onclick="CancelDelChat(this)" id="#cancel_del_chat">Отмена</a></p>');
});


/////////////////////////////кнопка отмены окна подтверждения удаления чата ссылок/////////////////////////////
function CancelDelChat() {
    $(".window_del_chat").css({ display: "none" });
    $(".window_del_chat p:last").remove();
    $("#window").removeClass("blur").addClass("cancel_blur");
}
