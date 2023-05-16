function Promeni(e) {
    var cout = 0;
    $(".trr").each(function () {
        if (cout == 0) {
            cout = 1;
        } else {
            if (e == "-2") {
                $(".trr").show();
                return;
            }
            if ($(this).find("#admin").data("admin") == e) {
                $(this).show();
            } else {
                $(this).hide();
            }
        }
    })
}
function FiltrirajPoDatumu() {
    var cout = 0;
    var od = new Date($("#od").val());
    var doss = new Date($("#do").val());
    $(".trr").each(function () {
        if (cout == 0) {
            cout = 1;
        } else {
            var ticketTime = new Date($(this).find("#time").data("date"));
            if (ticketTime >= od && ticketTime <= doss) {
                $(this).show();
            } else {
                $(this).hide();
            }
        }
    })
}