// Write your Javascript code.
$().ready(function () {
    $(".cost-slider").slider({
        max: 10000,
        range: true,
        values: [10, 1000],
        slide: function (event, ui) {
            $("#MinimumCost").val(ui.values[0]);
            $("#MaximumCost").val(ui.values[1]);
        }
    });


    $(".js-toggle-chevron").on("click",function (e) {
        var chevronContainer = $(e.target);
            chevronContainer.toggleClass("fa-chevron-down fa-chevron-right");
        }
    );
    
});