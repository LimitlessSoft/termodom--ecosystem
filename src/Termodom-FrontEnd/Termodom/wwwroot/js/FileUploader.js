class Gallery {
    constructor(EmptyDivContainer, ImagesNameArray, RootFolder) {
        this.MainDiv = EmptyDivContainer;
        this.Images = ImagesNameArray;
        this.RootFolder = RootFolder;
        
        this.MainDiv.append(`
                <style>
                    .imageItem:hover {
                        border: 2px solid orange !important;
                        cursor: pointer;
                    }

                    .AR-Gallery {
                        position: fixed;
                        overflow: hidden;
                        overflow-y: scroll;
                        top: 0;
                        left: 0;
                        right: auto;
                        width: 100%;
                        height: 500px;
                        background: whitesmoke;
                        border-left: 5px solid #ff5b5b;
                        z-index: 1050;
                        padding: 15px;
                    }
                </style>
                `);


        this.MainDiv.append("<button id='close_btn'><img src='/Images/x-mark.png' style='width: 20px' /></button>");

        $(this.MainDiv).addClass("AR-Gallery");

        $(this.MainDiv).append(`
            <div style='display: block;width: 100%'>
                <h2>Galerija</h2>
                <button id='GalerijaDodavanjeSlike'>Dodaj sliku</button>
                <br />
                <br />
            </div>`);
        

        for (var i = 0; i < this.Images.length; i++) {
            var string = `<img class='imageItem' style="max-height: 100px; border: 2px solid; display: block; float: left; margin: 10px" src="`;

            string += this.RootFolder + this.Images[i];

            string += `" />`;
            $(this.MainDiv).append(string);
        }

        $(this.MainDiv).hide();

        $("#GalerijaDodavanjeSlike").click(function () {
            window.open('/Gallery/DodavanjeSlike');
        });

        $("#close_btn").click(function () {
            $(this).parent().hide();
        })
        
    }
    Show(callback) {
        $(this.MainDiv).show();

        $(".imageItem").click(function () {
            callback($(this).attr("src"));
        });
    }
}