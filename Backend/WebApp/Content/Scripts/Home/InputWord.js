$("#btnSubmit").bind("click", function () {
    $.ajax({
        url: "/api/Word/InsertWord",
        type: "POST",
        dataType: "jsonp",
        //jsonpCallback: "ShangPinQiHuo", 
        data: {
            rnd: Math.floor(1e6 * Math.random() + 1),
            Text: $("#word").val(),
            Meaning: $("#meaning").val(),
            ExampleSentence: $("#exampleSentence").val()
        }
    }).done(function (e) {
        t.fill(e)
    }
    ).fail(function () { }
    ).always(function () { }
    );
});