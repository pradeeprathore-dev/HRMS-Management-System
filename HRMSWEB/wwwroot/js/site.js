// =============================
// SweetAlert2 Delete Confirmation
// =============================

document.addEventListener("DOMContentLoaded", function ()
{

    document.querySelectorAll(".delete-btn").forEach(function (button)
    {

        button.addEventListener("click", function (e)
        {

            e.preventDefault();

            let url = this.getAttribute("href");

            Swal.fire({

                title: "Delete Record?",

                text: "This action cannot be undone.",

                icon: "warning",

                showCancelButton: true,

                confirmButtonColor: "#d33",

                cancelButtonColor: "#6c757d",

                confirmButtonText: "Yes, Delete",

                cancelButtonText: "Cancel"

            }).then((result) => {

                if (result.isConfirmed) {

                    window.location.href = url;

                }

            });

        });

    });

});
// =============================
// Loading Button
// =============================

document.addEventListener("submit", function (e) {

    let form = e.target;

    let btn = form.querySelector("button[type='submit']");

    if (btn) {

        btn.disabled = true;

        btn.dataset.oldText = btn.innerHTML;

        btn.innerHTML =
            '<span class="spinner-border spinner-border-sm me-2"></span>Processing...';
    }

});