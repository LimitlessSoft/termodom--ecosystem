class AlertBox {
    constructor(EmptyDivContainer) {
        this.MainDiv = EmptyDivContainer;

        this.MainDiv.attr("style", `position: fixed; min-width: 200px; max-width: 80%; background-color: #ffeb3b; left: 50%; 
                            transform: translateX(-50%); top: 50px; z-index: 1050; border-radius: 20px; padding: 15px;
                            padding-left: 25px; padding-right: 25px; text-align: center; font-weight: bolder; font-size: x-large`);

        this.MainDiv.hide();
    }

    Show(Text) {
        this.MainDiv.html(Text);
        $(this.MainDiv).fadeIn(2000).promise().done(function () {
            $(this).delay(2000).fadeOut(2000);
        });
    }
}

