var oldQTP;
var oldQN;
function set() {
    oldQN = $(".tp_kolicina_i").val();
    oldQTP = $(".n_Kolicina_i").val();
}
function RaiseQ(parent) {
    var tp = $(parent).attr("data-tp");
    if (tp == -1) {
        var old = +$(parent).find(".n_Kolicina_i").val();
        if (old % 1 != 0) {
            old = old - old % 1;
        }
        $(parent).find(".n_Kolicina_i").val(old + 1);
    } else {
        var old = +$(parent).find(".tp_kolicina_i").val();
        if (old % 1 != 0) {
             old = old - old % 1;
        }

        $(parent).find(".tp_kolicina_i").val((old + 1).toFixed(2));
        $(parent).find(".n_Kolicina_i").val((+$(parent).find(".tp_kolicina_i").val() * tp).toFixed(2));
        oldQTP = $(parent).find(".n_Kolicina_i").val();
        oldQN = $(parent).find(".tp_kolicina_i").val();
    }
}

function LowerQ(parent) {
    var tp = $(parent).attr("data-tp");
    if (tp == -1) {
        if (+$(parent).find(".n_Kolicina_i").val() > 1) {
            var old = +$(parent).find(".n_Kolicina_i").val();
            if (old % 1 != 0) {
                old = old - old % 1;
            }
            $(parent).find(".n_Kolicina_i").val(old - 1);
        }
        
    } else {
        if (+$(parent).find(".tp_kolicina_i").val() > 1) {
            var old = +$(parent).find(".tp_kolicina_i").val();
            if (old % 1 != 0) {
                old = old - old % 1;
            }
            $(parent).find(".tp_kolicina_i").val((old - 1).toFixed(2));
            $(parent).find(".n_Kolicina_i").val((+$(parent).find(".tp_kolicina_i").val() * tp).toFixed(2));
        }   
    }
}

function SetQN(parent) {
    var tp = $(parent).attr("data-tp");
    var old = $(parent).find(".tp_kolicina_i").val();
    old = TestInput(old)
    if (old == null) {
        $(parent).find(".tp_kolicina_i").val(oldQN);
        $(parent).find(".n_Kolicina_i").val(oldQTP);
        $(".dodajUkorpu").attr("disabled", true);
        return;
    }
    $(parent).find(".n_Kolicina_i").val((+$(parent).find(".tp_kolicina_i").val() * tp).toFixed(2));
    $(".dodajUkorpu").attr("disabled", false);
    oldQN = old;
}
var timeout = 1500;
var timer;

function SetQTP(parent) {
    if (TestInput($(parent).find(".n_Kolicina_i").val()) == null) {
        $(parent).find(".n_Kolicina_i").val(oldQTP);
    }
    clearTimeout(timer)
    timer = setTimeout(() => {
        var tp = $(parent).attr("data-tp");
        var oldTp = $(parent).find(".n_Kolicina_i").val();
        oldTp = TestInput(oldTp);
        if ( oldTp == 0 || oldTp == "") {
            $(parent).find(".n_Kolicina_i").val(oldTp);
            $(parent).find(".tp_kolicina_i").val(oldTp);
            $(".dodajUkorpu").attr("disabled", true);
            return;
        }
        else
            if (parseFloat(oldTp) % parseFloat(tp) != 0) {
                oldTp = parseFloat(oldTp) + parseFloat((tp - (oldTp % tp)));
                $(parent).find(".n_Kolicina_i").val(oldTp);
            }
        $(parent).find(".tp_kolicina_i").val((+$(parent).find(".n_Kolicina_i").val() / tp).toFixed(2));
        $(".dodajUkorpu").attr("disabled", false);
        oldQTP = oldTp;
        }, timeout);
    }












function Preracunaj(old, oldQtp) {
    if (oldQTP != old) {
        timeout = 1;
        SetQTP($(".n_Kolicina_i").parent().parent());
    }
    if (oldQN != oldQtp) {
        SetQN($(".tp_kolicina_i").parent().parent());
    }
       
}
















function TestInput(e) {
    var regex = new RegExp("[A-Za-z]").test(e)
    if (e == "") {
        return e;
    }
    if (e == 0) {
        return e;
    }
    if (String(e).includes(",")) {
        e = e.replace(",", ".");
    }
    if (regex) {
        return null;
    }
    if (parseFloat(e) < 0) {
        return null;
    }

    return e;
}