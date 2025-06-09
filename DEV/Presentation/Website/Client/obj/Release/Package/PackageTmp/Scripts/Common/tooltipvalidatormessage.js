$(document).ready(function () {

    $(document).tooltip({
        items: ".input-validation-error",
        content: function () {
            if ($(this).hasClass('input-validation-error')) { 
                var a = $(this).siblings('span').children('span').text();
                a = a.replace("--Select--", "");
                return a;
            }

        },
        tooltipClass: "ui-tooltip1",
        position: {
            my: "center bottom-10",
            at: "center top",
            using: function (position, feedback) {
                $(this).css(position);
                $("<div>")
                    .addClass("arrow")
                    .addClass(feedback.vertical)
                    .addClass(feedback.horizontal)
                    .appendTo(this);
            }
        }
    });
});

