$(document).ready(() => {


    // if ($(window).width() > 991) {
    // }


    $('input[type="tel"]').mask('+7 (999) 999-99-99');


    /** * Replace all SVG images with inline SVG */
    $("img.img-svg").each(function () {
        var $img = $(this);
        var imgID = $img.attr("id");
        var imgClass = $img.attr("class");
        var imgURL = $img.attr("src");
        $.get(
            imgURL,
            function (data) {
                var $svg = $(data).find("svg");
                if (typeof imgID !== "undefined") {
                    $svg = $svg.attr("id", imgID);
                }
                if (typeof imgClass !== "undefined") {
                    $svg = $svg.attr("class", imgClass + " replaced-svg");
                }
                $svg = $svg.removeAttr("xmlns:a");
                $img.replaceWith($svg);
            },
            "xml"
        );
    });

    $('.show-desires').click(function() {
        $('.desires__box .column').removeClass('hide')
        $(this).hide()
    })


});
