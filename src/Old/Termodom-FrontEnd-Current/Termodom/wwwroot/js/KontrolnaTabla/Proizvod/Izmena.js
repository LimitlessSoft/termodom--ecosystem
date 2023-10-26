
function UpdateTDHelp() {
    $(".td-help button").hide();
    $(".td-help img").css("display", "block");
    $.ajax({
        method: "POST",
        url: "/Help/Update",
        data: {
            "name": "uredi_korisnika",
            "content": $("#td-help-content").val()
        },
        success: function (data, textStatus, jqXHR) {
            if (jqXHR.status == 200) {
                $(".td-help label").html("Uspesno sacuvane izmene!");
                $(".td-help label").css("color", "green");
                $(".td-help").css("background-color", "green");
            } else {
                $(".td-help label").html("Doslo je do greske prilikom cuvanja helpa!");
                $(".td-help label").css("color", "red");
                $(".td-help").css("background-color", "red");
            }
            $(".td-help label").fadeIn(1000);
            $(".td-help button").show();
            $(".td-help img").css("display", "none");
            setTimeout(function () {
                $(".td-help label").fadeOut(1000);
            }, 5000);
            setTimeout(function () {
                $(".td-help").css("background-color", "white");
            }, 300)
        }
    })
}
function ProkaziGaleriju() {
    $("#Gallery").show();
}
function GalleryImageClicked(ff) {
    $("#blah").attr("src", ff);
    $("#Thumbnail").val(ff);
    $("#Gallery").hide();
}