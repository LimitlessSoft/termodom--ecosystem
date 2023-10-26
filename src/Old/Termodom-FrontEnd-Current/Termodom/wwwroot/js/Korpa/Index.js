var korisnikMagacin = 12;
var trenutnoTP = 1;
var trenutniRobaID = -1;

window.onload = Podesavanja;

function Podesavanja() {
    $(".adresa-isporuke").hide();
    $(".grad").hide();
    $(".mesto-preuzimanja select").val(korisnikMagacin);
}
function MestoIsporuke(e) {
    var nacin = $(e).val();
    if (nacin == -5) {
        $(".adresa-isporuke").show();
        $(".grad").show();
        $(".nacin-placanja select").val("3");
        $(".napomena-isporuke").show();
    } else {
        var nacinPlacanja = $(".nacin-placanja select").val();
        $(".adresa-isporuke").hide();
        $(".grad").hide();
        $(".napomena-isporuke").hide();

        if (nacinPlacanja == 3) {
            $(".nacin-placanja select").val("0");
        }

    }
}
function NacinUplate(e) {
    var nacin = $(e).val();
    if (nacin == 3) {
        $(".mesto-preuzimanja select").val("-5");
        $(".adresa-isporuke").show();
        $(".grad").show();
        $(".napomena-isporuke").show();


    } else {
        if ($(".mesto-preuzimanja select").val() == -5) {
            $(".mesto-preuzimanja select").val(korisnikMagacin);
        }
        $(".adresa-isporuke").hide();
        $(".grad").hide();
        $(".napomena-isporuke").hide();
    }
}
function Disable() {
    $("input").each(function (e) {
        $(this).attr('disabled', 'disabled');
    })
    $("select").each(function (e) {
        $(this).attr('disabled', 'disabled');
    })
}
function Enable() {
    $("input").each(function (e) {
        $(this).removeAttr("disabled")
    })
    $("select").each(function (e) {
        $(this).removeAttr("disabled")
    })
}

function Zakljuci() {
    $(".zakljuci-porudzbinu-btn").hide();
    $(".zakljuci-porudzbinu-animation").show();
    var magacin = $(".mesto-preuzimanja select").val();
    var nacinUplate = $(".nacin-placanja select").val();
    var komentar = $(".napomena input").val();
    var imePrezime = $(".kupac-info input").val();
    var mobilni = $(".mobilni-telefon input").val();
    var adresa = $(".adresa-isporuke input").val();
    var grad = $(".grad input").val();
    adresa = grad + ", " + adresa
    if (nacinUplate == 3 || magacin == -5) {
        if (adresa == null || adresa.length < 5) {
            $(".Error span").html("morate da popunite adresu isporuke")
            $(".zakljuci-porudzbinu-btn").show();
            $(".zakljuci-porudzbinu-animation").hide();
            return;
        } if (grad == null || grad.length < 2) {
            $(".Error span").html("morate da popunite grad")
            $(".zakljuci-porudzbinu-btn").show();
            $(".zakljuci-porudzbinu-animation").hide();
            return;
        }

    }
    $.ajax({
        method: "POST",
        url: "/api/Korpa/ZakljuciPorudzbinu",
        beforeSend: Disable(),
        data: {
            "komentar": komentar,
            "magacin": magacin,
            "nacinUplate": nacinUplate,
            "imePrezime": imePrezime,
            "mobilni": mobilni,
            "adresa": adresa,
        }, error: function (e) {
            $(".Error span").html(e.responseText)
            $(".zakljuci-porudzbinu-btn").show();
            $(".zakljuci-porudzbinu-animation").hide();
            Enable();
        },
        success: function (e) {
            window.location.href = "/Porudzbina/" + e;
        }
    })

}
function Ukloni(e) {
    $(e).hide();
    $(e).parent().find("img").show();
    var id = $(e).attr("data-roba");
    if (id == undefined || id == null)
        return;
    $.ajax({
        method: "POST",
        url: "/api/Korpa/Ukloni",
        data: { id: id },
        success: function (e) {
            window.location.reload();
        }, error: function (e) {
            $(".Error span").html(e.responseText)
        },
    })
}

function IzmeniKolicinu(kolicina, jm, tpKolicina, tpJM, robaID) {
    trenutnoTP = tpKolicina;
    trenutniRobaID = robaID;
    $('.izmena-kolicine-tooltip-wrapper').fadeIn();

    $(".izmena-kolicine-tooltip-wrapper-front .kolicina-normalno-pakovanje .jedinica-mere").html(jm);
    $(".izmena-kolicine-tooltip-wrapper-front .kolicina-normalno-pakovanje .jedinica-mere").html(jm);

    if (trenutnoTP == 0 || trenutnoTP == 1) {
        $(".izmena-kolicine-tooltip-wrapper-front .kolicina-transportno-pakovanje .jedinica-mere").html("empty");
        $(".izmena-kolicine-tooltip-wrapper-front .kolicina-transportno-pakovanje").css("display", "none");
        $(".izmena-kolicine-tooltip-wrapper-front .kolicina-normalno-pakovanje input").val(kolicina.toFixed(2));
        $(".izmena-kolicine-tooltip-wrapper-front .kolicina-transportno-pakovanje input").val(kolicina.toFixed(2));
        $(".izmena-kolicine-tooltip-wrapper-front .kolicina-normalno-pakovanje").css("width", "98%");
    } else {
        $(".izmena-kolicine-tooltip-wrapper-front .kolicina-transportno-pakovanje .jedinica-mere").html(tpJM);
        $(".izmena-kolicine-tooltip-wrapper-front .kolicina-transportno-pakovanje").css("display", "block");
        $(".izmena-kolicine-tooltip-wrapper-front .kolicina-normalno-pakovanje input").val(kolicina.toFixed(2));
        $(".izmena-kolicine-tooltip-wrapper-front .kolicina-transportno-pakovanje input").val((kolicina / tpKolicina).toFixed(2));
        $(".izmena-kolicine-tooltip-wrapper-front .kolicina-normalno-pakovanje").css("width", "48%");
    }
}

function Uvecaj() {
    var trenutnaTPKolicina = +$(".izmena-kolicine-tooltip-wrapper-front .kolicina-transportno-pakovanje input").val();
    if (trenutnaTPKolicina % 1 > 0)
        trenutnaTPKolicina = trenutnaTPKolicina - (trenutnaTPKolicina % 1);
    var buducaTPKolicina = trenutnaTPKolicina + 1;

    $(".izmena-kolicine-tooltip-wrapper-front .kolicina-normalno-pakovanje input").val((buducaTPKolicina * trenutnoTP).toFixed(2));
    $(".izmena-kolicine-tooltip-wrapper-front .kolicina-transportno-pakovanje input").val(buducaTPKolicina.toFixed(2));
}

function Smanji() {
    var trenutnaTPKolicina = +$(".izmena-kolicine-tooltip-wrapper-front .kolicina-transportno-pakovanje input").val();
    if (trenutnaTPKolicina % 1 > 0)
        trenutnaTPKolicina = trenutnaTPKolicina - (trenutnaTPKolicina % 1);
    var buducaTPKolicina = trenutnaTPKolicina - 1;

    $(".izmena-kolicine-tooltip-wrapper-front .kolicina-normalno-pakovanje input").val((buducaTPKolicina * trenutnoTP).toFixed(2));
    $(".izmena-kolicine-tooltip-wrapper-front .kolicina-transportno-pakovanje input").val(buducaTPKolicina.toFixed(2));
}

function RacunajJM() {
    var pak = $(".izmena-kolicine-tooltip-wrapper-front .kolicina-normalno-pakovanje input").val();
    $(".izmena-kolicine-tooltip-wrapper-front .kolicina-transportno-pakovanje input").val((pak / trenutnoTP).toFixed(2));
}

function RacunajTransportno() {
    var kol = $(".izmena-kolicine-tooltip-wrapper-front .kolicina-transportno-pakovanje input").val();
    $(".izmena-kolicine-tooltip-wrapper-front .kolicina-normalno-pakovanje input").val((kol * trenutnoTP).toFixed(2));
}

function SacuvajKolicinu() {
    $(".izmena-kolicine-btns .row").css("display", "none");
    $(".korpa-wrapper .work-animation-wrapper").css("display", "block");
    $.ajax({
        url: "/api/Korpa/Kolicina/Set",
        type: "POST",
        data: {
            "robaID": trenutniRobaID,
            "kolicina": parseFloat(($(".izmena-kolicine-tooltip-wrapper-front .kolicina-normalno-pakovanje input").val() * 1).toFixed(2))
        },
        complete: function (a, s, d) {
            if (a.status == 200) {
                window.location.reload();
            } else if (a.status == 400) {
                    $(".izmena-kolicine-btns .row").css("display", "");
                    $(".korpa-wrapper .work-animation-wrapper").css("display", "none");
                    $(".korpa-wrapper .izmena-kolicine-status-label").html("Neispravna kolicina!");
                    $(".korpa-wrapper .izmena-kolicine-status-label").css("color", "red");
                    $(".korpa-wrapper .izmena-kolicine-status-label").show();
            } else {
                alert("Doslo je do greske! Kontaktirajte administratora!");
            }
        }
    })
}