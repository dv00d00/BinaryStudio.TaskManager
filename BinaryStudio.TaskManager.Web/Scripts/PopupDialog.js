$.ajaxSetup({ cache: false });

$(document).ready(function () {
    $(".openDialog").live("click", function (e) {
        e.preventDefault();

        $("<div></div>")
                    .addClass("dialog")
                    .attr("id", $(this)
                    .attr("data-dialog-id"))
                    .appendTo("body")
                    .dialog({
                        title: $(this).attr("data-dialog-title"),
                        close: function () { $(this).remove(); },
                        modal: true,
                        height: 600,
                        width: 400,
                        left: 0

                    })
                    .load(this.href);
        $(".dropdown-menu").css("z-index", "10000");
    });

    $(".close").live("click", function (e) {
        e.preventDefault();
        $(this).closest(".dialog").dialog("close");
    });
});