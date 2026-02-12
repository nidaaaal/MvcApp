$(function () {

    for (let i = 1; i <= 31; i++)
        $('#ddlDay').append(`<option value="${i}">${i}</option>`);

    for (let i = 1; i <= 12; i++)
        $('#ddlMonth').append(`<option value="${i}">${i}</option>`);

    const currentYear = new Date().getFullYear();
    for (let i = currentYear; i >= 1900; i--)
        $('#ddlYear').append(`<option value="${i}">${i}</option>`);

    const MIN_AGE = 13;

    function isLeapYear(year) {
        return (year % 4 === 0 && year % 100 !== 0) || (year % 400 === 0);
    }

    function isValidDate(day, month, year) {
        if (!day || !month || !year) return false;

        const daysInMonth = [31, isLeapYear(year) ? 29 : 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];

        return day >= 1 &&
            month >= 1 &&
            month <= 12 &&
            year >= 1900 &&
            day <= daysInMonth[month - 1];
    }

    function calculateAge(day, month, year) {
        const today = new Date();
        let age = today.getFullYear() - year;

        const hasHadBirthdayThisYear =
            today.getMonth() + 1 > month ||
            (today.getMonth() + 1 === month && today.getDate() >= day);

        if (!hasHadBirthdayThisYear) age--;

        return age;
    }




    $('#ddlDay, #ddlMonth, #ddlYear').on('change', function () {

        const day = parseInt($('#ddlDay').val(), 10);
        const month = parseInt($('#ddlMonth').val(), 10);
        const year = parseInt($('#ddlYear').val(), 10);

        $('#txtAge').val('');
        $('#dobError').text('');
        $('#ageError').text('');

        if (!isValidDate(day, month, year)) {
            $('#dobError').text('Please select a valid date');
            return;
        }

        const dob = new Date(year, month - 1, day);
        const today = new Date();
        today.setHours(0, 0, 0, 0);

        if (dob > today) {
            $('#dobError').text('Date of birth cannot be in the future');
            return;
        }

        const age = calculateAge(day, month, year);

        if (age < MIN_AGE) {
            $('#ageError').text('You must be at least 13 years old');
            return;
        }


        $('#txtAge').val(age);

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


function TriggerEdit() {
    document.getElementById("editbtn").style.visibility = "hidden";
    document.getElementById("savebtn").style.visibility = "visible";
    document.getElementById("cancelbtn").style.visibility = "visible";
    document.getElementById("ddlDay").style.display = "inline-block";
    document.getElementById("ddlMonth").style.display = "inline-block";
    document.getElementById("ddlYear").style.display = "inline-block";
    document.getElementById("hiddenDob").style.display = "none";
    document.getElementById("ddlState").style.display = "inline-block";
    document.getElementById("ddlCity").style.display = "inline-block";
    document.getElementById("hiddenState").style.display = "none";
    document.getElementById("hiddenCity").style.display = "none";
    document.getElementById("M").style.display = "inline-block";
    document.getElementById("F").style.display = "inline-block";
    document.getElementById("hiddenGender").style.display = "none";








    document.querySelectorAll('.form-control').forEach(input => {

        if (input.name !== "Age") {
            input.removeAttribute('readonly');
            input.classList.add("editable-field");
        }
    })
};

function CancelEdit() {

    document.getElementById("editbtn").style.visibility = "visible";
    document.getElementById("savebtn").style.visibility = "hidden";
    document.getElementById("cancelbtn").style.visibility = "hidden";
    document.getElementById("ddlDay").style.display = "none";
    document.getElementById("ddlMonth").style.display = "none";
    document.getElementById("ddlYear").style.display = "none";
    document.getElementById("ddlState").style.display = "none";
    document.getElementById("ddlCity").style.display = "none";
    document.getElementById("hiddenCity").style.display = "inline-block";
    document.getElementById("hiddenState").style.display = "inline-block";
    document.getElementById("hiddenDob").style.display = "inline-block";
    document.getElementById("M").style.display = "none";
    document.getElementById("F").style.display = "none";
    document.getElementById("hiddenGender").style.display = "inline-block";





    document.querySelectorAll('.form-control').forEach(input => {
        input.setAttribute('readonly', 'readonly');
        input.classList.remove("editable-field");

    })
};