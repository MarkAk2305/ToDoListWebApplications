$(document).ready(function () {
    $('.ActiveCheck').change(function () {
        var self = $(this);
        var id = self.attri('id)');
        var value = self.prop('checked');

        $.ajax({
            url: '/Task/IndexEdit',
            data: {
                id: id,
                value: value
            },
            type: 'POST',
            success: function (result) {
                $('#tableDiv').html(result);
            }
        })
    })
})
