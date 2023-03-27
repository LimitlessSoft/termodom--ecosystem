class InitializeProizvodiIndex {
    constructor() {
        $(".grupe-in a[data-grupa-ime='" + this.trenutnaGrupa + "']").addClass("active");

        var obj = this;
        $(function () {
            $(".proizvodi-wrapper .proizvodi-pretraga input").on("keydown", function (e) {
                if (e.keyCode == 13) {
                    obj.PretraziProizvod(obj);
                }
            })
        })

        this.UcitavanjeProizvoda = false;
        this.PretraziProizvod(this);
    }

    PretraziProizvod(mainObject) {
        if (mainObject.UcitavanjeProizvoda) {
            setTimeout(function () {
                mainObject.PretraziProizvod(mainObject);
            }, 1000);
        } else {
            $("#proizvodi-holder").html("");
            this.UcitavanjeProizvoda = true;
            this.UcitajProizvode(0, 20, $(".proizvodi-wrapper .proizvodi-pretraga input").val());
        }
    }

    UcitajProizvode(offset, count, searchKey) {
        var obj = this;
        $.ajax({
            type: "POST",
            url: "/p/Proizvodi/Get",
            data: {
                "offset": offset,
                "count": count,
                "searchKey": searchKey
            },
            complete: function (data) {
                if (data.status == 200) {
                    $("#proizvodi-holder").append(data.responseText);
                    obj.UcitajProizvode(offset + count, count, searchKey);
                } else if (data.status == 204) {
                    $(".proizvodi-wrapper .animacija").hide();
                    obj.UcitavanjeProizvoda = false;
                } else {
                    console.log("Error Loading Partial");
                    $(".proizvodi-wrapper .animacija").hide();
                }
            }
        });
    }
}