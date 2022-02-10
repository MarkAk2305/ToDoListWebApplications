$(document).ready(function () {
    $.ajax({
        url: '/Task/BuildToDoTable',
        success: function (result) {
            $('#tableDiv').html(result);
        }
    });
});
