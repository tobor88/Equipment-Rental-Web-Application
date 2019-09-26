// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var today = new Date(new Date().getFullYear(), new Date().getMonth(), new Date().getDate());
$('#DateStart').datepicker({
    uiLibrary: 'bootstrap4',
    minDate: today,
    maxDate: function () {
        return $('#DateEnd').val();
    }
});
$('#DateEnd').datepicker({
    uiLibrary: 'bootstrap4',
    minDate: function () {
        return $('#DateStart').val()
    },
    maxDate: function () {
        var endDate = new Date($('#DateStart').val());
        var max = endDate.setDate(endDate.getDate() + 30);
        return max
    }
});