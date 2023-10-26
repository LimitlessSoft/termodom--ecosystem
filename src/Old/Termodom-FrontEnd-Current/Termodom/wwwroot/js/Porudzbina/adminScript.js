
function UkloniStavku(porudzbinaID, stavkaID, successCallback, errorCallback) {
    if (confirm("Da li sigurno zelite da uklonite stavku?")) {
        $.ajax({
            method: "POST",
            url: "/api/Porudzbina/Stavka/Ukloni",
            data: { porudzbina: porudzbinaID, stavka: stavkaID },
            complete: function (response) {
                if (response.status == 200) {
                    if (successCallback != null)
                        successCallback();
                } else {
                    if (errorCallback != null)
                        errorCallback();
                }
            }
        })
    }
}

function PromenaMestaPreuzimanja(e) {
    if (confirm("Da li zelite da promenite mesto preuzimanja?")) {
        var id = $(e).attr("data-id");
        var mesto = $(e).val();
        $.ajax({
            method: "POST",
            url: "/api/Porudzbina/MestoPreuzimanja/Set",
            data: { id: id, mesto: mesto },
            success: function (a, s, d) {
                if (d.status == 200) {
                    alert("Uspesno sacuvana izmena");
                } else {
                    alert(d.statusText);
                }
            }, complete: function () {
            }
        })
    }

}
function PromeniNacinPlacanja(e) {
    var porudzbina = $(e).attr("data-id");
    var nacinPlacanja = $(e).val();
    $.ajax({
        method: "POST",
        url: "/api/Porudzbina/NacinPlacanja/Set",
        data: { porudzbina: porudzbina, nacinPlacanja: nacinPlacanja },
        success: function (a, s, d) {
            if (d.status == 200) {
                alert("Uspesno sacuvana promena");
            } else {
                alert(d.responseText)
            }
        }, complete: function () {
        }
    })
}
function PromeniStatus(e) {
    var porudzbina = $(e).attr("data-id");
    var status = $(e).val();
    $.ajax({
        method: "POST",
        url: "/api/Porudzbina/Status/Set",
        data: { porudzbina: porudzbina, status: status },
        success: function (a, s, d) {
            if (d.status == 200) {
                window.location.reload();
            } else {
                alert(d.responseText)
            }
        }, complete: function () {
        }
    })
}
function SetInterniKomentar(e) {
    var text = $(e).attr("data-tip");
    var komentar = $("input[data-id='" + text + "']").val()
    if (komentar.length < 3) {
        alert("Komentra mora sadrzati bar tri slova");
        return;
    }
    var porudzbina = $(e).attr("data-porudzbina");
    $.ajax({
        method: "POST",
        url: "/api/Porudzbina/SetInterniKomentar",
        data: { komentar: komentar, porudzbina: porudzbina },
        success: function (a, s, d) {
            if (d.status == 200) {
                alert("Uspesno ste dodali interni komentar");
            } else {
                alert(d.responseText)
            }
        }
    })
}
function SetKomentar(e) {
    var komentar = $(e).val();
    if (komentar.length < 3) {
        alert("Komentra mora sadrzati bar tri slova");
        return;
    }
    var porudzbina = $(e).attr("data-porudzbina");
    $.ajax({
        method: "POST",
        url: "/api/Porudzbina/Kometar/Set",
        data: { komentar: komentar, porudzbina: porudzbina },
        success: function (a, s, d) {
            if (d.status == 200) {
                alert("Uspesno ste dodali  komentar")
            } else {

            }
        }
    })
}
function RazvezProracun(e) {

    var porudzbina = $(e).attr("data-id");
    if (confirm("Da li ste sigurni da zelite da razvezete porudzbinu")) {
        $.ajax({
            method: "POST",
            url: "/api/Porudzbina/BrDokKom/Remove",
            data: { porudzbina: porudzbina },
            success: function (a, s, d) {
                if (d.status == 200) {
                    window.location.reload();
                } else {
                    alert(d.responseText);
                }
            }

        })
    }
}
function PretvoriUProracun(e) {
    var porudzbina = $(e).attr("data-id");
    if (confirm("Da li ste sigurni da zelite da porudzbinu pretvorite u proracun?")) {
        $.ajax({
            method: "POST",
            url: "/api/Porudzbina/BrDokKom/Set",
            data: { porudzbina: porudzbina },
            complete: function (response) {
                if (response.status == 200) {
                    window.location.reload();
                } else {
                    alert(response.responseText);
                }
            }
        })
    }
}
function PretvoriUPonudu(e) {
    var porudzbina = $(e).attr("data-id");
    if (confirm("Da li ste sigurni da zelite da porudzbinu pretvorite u ponudu?")) {
        $.ajax({
            method: "POST",
            url: "/api/Porudzbina/BrDokKom/Set",
            data: { porudzbina: porudzbina, tip: 1 },
            complete: function (response) {
                if (response.status == 200) {
                    window.location.reload();
                } else {
                    alert(response.responseText);
                }
            }
        })
    }
}
function PreuzmiNaObradu(e, korisnikID) {
    var porudzbina = $(e).attr("data-id");
    $.ajax({
        method: "POST",
        url: "/api/Porudzbina/ReferentObrade/Set",
        data: { porudzbina: porudzbina, korisnikID: korisnikID },
        success: function (e, a, s) {
            if (s.status == 200) {
                window.location.reload();
            } else {
                alert(s.responseText);
            }
        }

    })
}

function ObavestiRadnju(e) {
    var id = $(e).attr("data-id");

    $.ajax({
        method: "GET",
        url: "/api/Porudzbina/ObavestiRanju",
        data: { id: id },
        complete: function (a, s, d) {
            if (a == 200) {
                alert("Radnja je obavestena");
            }
            else {
                alert(a.responseText);
            }
        }
    })
}
function ObavestiKorisnika(e) {
    var id = $(e).attr("data-id");

    $.ajax({
        method: "GET",
        url: "/api/Porudzbina/ObavestiKorisnika",
        data: { id: id },
        complete: function (a, s, d) {
            if (a == 200) {
                alert("Korisnik je obavesten");
            }
            else {
                alert(a.responseText);
            }
        }
    })
}
function PosaljiSMS(e) {
    var poruka = prompt("Tekst poruke", "");
    var id = $(e).attr("data-id");
    if (poruka == null || poruka.length == 0) {
        return;
    }
    $.ajax({
        method: "GET",
        url: "/api/Porudzbina/PosaljiSmsKorisniku",
        data: { id: id, poruka: poruka },
        complete: function (a, s, d) {
            alert(a.responseText);
        }
    })
}