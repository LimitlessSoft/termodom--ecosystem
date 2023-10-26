function DisableNodes(nodes) {
    for (var i = 0; i < nodes.length; i++) {
        $(nodes[i]).prop("disabled", true);
    }
}

function EnableNodes(nodes) {
    for (var i = 0; i < nodes.length; i++) {
        $(nodes[i]).prop("disabled", false);
    }
}

function Disable() {
    $("button").prop("disabled", true);
    $("input").prop("disabled", true);
    $("select").prop("disabled", true);
}

function Enable() {
    $("button").prop("disabled", false);
    $("input").prop("disabled", false);
    $("select").prop("disabled", false);
}