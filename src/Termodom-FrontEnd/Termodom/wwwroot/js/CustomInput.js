class CustomNumberInput {
    constructor(ContainerID, Title) {
        this.MainDiv = document.getElementById(ContainerID);

        this.IncreaseValueMethod;
        this.Title = Title;

        var HtmlToAppend = "";

        var styleElement = document.createElement("style");
        var inputElement = document.createElement("input");
        var incButton = document.createElement("button");
        var decButton = document.createElement("button");



        styleElement.innerHTML = `
                .CustomNumberInput input::-webkit-outer-spin-button, .CustomNumberInput input::-webkit-inner-spin-button {
                    -webkit-appearance: none;
                    margin: 0;
                }

                .CustomNumberInput {
                    height: 50px;
                }

                .CustomNumberInput input {
                    height: 100%;
                    width: 50px;
                    text-align: center;
                    border-radius: 3px;
                    outline: none;
                    box-shadow: none;
                    border: 1px solid lightgray;
                    float: left;
                }

                .CustomNumberInput button {
                    width: 25px;
                    height: 50%;
                    display: block;
                    border: 1px solid lightgray;
                    padding: 0;
                }`;

        inputElement.type = "number";
        inputElement.value = 1;

        incButton.innerHTML = "+";
        $(incButton).addClass("IncreaseButton");

        decButton.innerHTML = "-";
        $(decButton).addClass("DecreaseButton");



        $(this.MainDiv).addClass("CustomNumberInput");
        this.MainDiv.appendChild(styleElement);
        this.MainDiv.appendChild(inputElement);
        this.MainDiv.appendChild(incButton);
        this.MainDiv.appendChild(decButton);

        if (this.Title != null) {
            var title = document.createElement("label");
            $(title).addClass("Title");
            title.style = "position: absolute; margin: 0; passing: 0; top: -20px; width: 100%; text-align: center";
            title.innerHTML = this.Title;
            this.MainDiv.appendChild(title);
        }
    }
}