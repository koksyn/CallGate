export default {
    activateSidebarRoute: function () {
        $(".sidebar .menu .list li.active").removeClass("active");

        $(".sidebar .menu .list li > a.active").each(function (id, link) {
            $(link).parent().addClass("active");
        });
    }
}