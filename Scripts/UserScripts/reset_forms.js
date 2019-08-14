function ResetForms() {
    document.getElementById('FormAddLink').reset();
    document.getElementById('FormShareLinks').reset();
    document.getElementById('FormSaveSharedLinks').reset();

    //ФОРМА СОХРАНЕНИЯ ПЕРЕСЛАННЫХ ССЫЛОК

    //возвращаем стандартный цвет блоку ссылок с зинего на серый
    $("#workzone #" + previus_block_links).css({ backgroundColor: "#464646" });
    //делаем выпадающий список вкладок видимым
    $("#Category_Id").removeAttr("disabled").removeClass("form_disable").attr({ required: "" });
    //удаляем текст ошибки
    $(".errors").text("");
    $(".error_description").text("");

    if ($(".share_link").is(":visible")) {
        $(".share_link").slideUp(300, function () {
            $(".add_link").slideDown(300);
        });
    } else if ($(".save_share_link_form").is(":visible")){
        $(".save_share_link_form").slideUp(300, function () {
            $(".add_link").slideDown(300);
        });
        
    } 
}

