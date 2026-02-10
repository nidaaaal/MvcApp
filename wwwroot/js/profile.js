

function TriggerEdit() {
    document.getElementById("editbtn").style.visibility = "hidden";
    document.getElementById("savebtn").style.visibility = "visible";
    document.getElementById("cancelbtn").style.visibility = "visible";

    document.querySelectorAll('.form-control').forEach(input => {
        if (input.name !== "Age") {
            input.removeAttribute('readonly');
            input.classList.add("editable-field");
        }

    });
}

function CancelEdit() {
    document.getElementById("editbtn").style.visibility = "visible";
    document.getElementById("savebtn").style.visibility = "hidden";
    document.getElementById("cancelbtn").style.visibility = "hidden";

    document.querySelectorAll('.form-control').forEach(input => {
        input.setAttribute('readonly', 'readonly');
        input.classList.remove("editable-field");
    });

}

