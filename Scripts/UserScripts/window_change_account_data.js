
$(document).on("click", "#show_pass", function () {
    
    var type = $("#Password").attr("type");
    if (type == "password") {
        $("#Password").attr("type", "text");
    } else {
        $("#Password").attr("type", "password");
    }
});



var check_window = 0;
$(document).on("click", ".alert", function () {
    if (check_window == 0) {
        console.log();
        $(".change").css({ display: "block" }).addClass("Animation");
        $("#window").addClass("blur");
        check_window = 1;
    } else {
        $(".change").css({ display: "none" }).removeClass("Animation");
        $("#window").removeClass("blur");
        check_window = 0;
    }
});

function CheckFirstLogining() {

    var check_first_logining = $("#first_logining").val();
   
    if (check_first_logining == "Yes") {
        $(".change").css({ display: "block" }).addClass("Animation");
        $("#window").addClass("blur");
    }
}

var check = $("#error_change_account_data").val();

$(document).on("click", "#close", function () {
    $(".change").css({ display: "none" }).removeClass("Animation");
    $("#window").removeClass("blur");
    check_window = 0;
    $("#error_change_account_data").val("");
    $(".errors").text("");
    check = "";
});

if (check == "этот логин занят") {
   
    $(".change").css({
        display: "block",
        opacity: "1"
    });
    $(".errors").text(check);
}