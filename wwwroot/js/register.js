$(function () {

    for (let i = 1; i <= 31; i++)
        $('#ddlDay').append(`<option value="${i}">${i}</option>`);

    for (let i = 1; i <= 12; i++)
        $('#ddlMonth').append(`<option value="${i}">${i}</option>`);

    const currentYear = new Date().getFullYear();
    for (let i = currentYear; i >= 1900; i--)
        $('#ddlYear').append(`<option value="${i}">${i}</option>`);

    $('#ddlDay, #ddlMonth, #ddlYear').change(function () {
        const d = $('#ddlDay').val();
        const m = $('#ddlMonth').val();
        const y = $('#ddlYear').val();

        if (d && m && y) {
            const dob = new Date(y, m - 1, d);
            const diff = Date.now() - dob.getTime();
            const age = new Date(diff).getUTCFullYear() - 1970;
            $('#txtAge').val(age);
        }
    });

    $('#ddlState').change(function () {
        const state = $(this).val();

        $('#ddlCity').empty().append('<option value="">Loading...</option>');

        $.get('/Location/GetCities', { state: state }, function (cities) {
            $('#ddlCity').empty().append('<option value="">-- Select City --</option>');
            $.each(cities, function (i, city) {
                $('#ddlCity').append(`<option value="${city}">${city}</option>`);
            });
        });
    });

});
