function CheckNullSharedLinks() {
    if (filter_mass_length == 0) {

        mass_shared_link.splice(0, mass_shared_link.length);

        $(".share_link").slideUp(300, function () {
            $(".add_link").slideDown(300);
            document.getElementById('FormShareLinks').reset();
        });
    }
}