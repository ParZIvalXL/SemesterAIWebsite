
document.addEventListener("DOMContentLoaded", async function () {
    let form = document.getElementById("register-form");
    let fullName = document.getElementById("FullName");
    let email = document.getElementById("Email");
    let password = document.getElementById("Password");
    let button = document.getElementById("submit-btn");
    let agreement = document.getElementById("iAgree");

    form.addEventListener( "submit", async function (e) {
        e.preventDefault();
        console.log("clicked");
        
        if(!agreement.checked) {
            ShowError("You must agree to the terms and conditions");
            return;
        }
        if((!email.value || email.value === "") && (!password.value || password.value === "")) {
            ShowError("All fields are required!");
            return;
        }
        
        if(!fullName.value || fullName.value === "") {
            ShowError("Full name is required");
            return;
        }
        
        if(!email.value || email.value === "") {
            ShowError("Email is required");
            return;
        }
        
        if(!email.value.includes("@") || !email.value.includes(".")) {
            ShowError("Invalid email");
            return;
        }
        
        if(!password.value || password.value === "") {
            ShowError("Password is required");
            return;
        }

        if(password.value.length < 8) {
            ShowError("Password must be at least 8 characters");
            return;
        }
        
        if (email.value && password.value) {
            const formData = new FormData(form);
            
            button.disabled = true;
            button.innerHTML = "Loading...";
            
            await fetch("/auth/register", {
                method: "POST",
                body: formData,
            }).then((response) => {
                if (response.ok) {
                    window.location.href = "/";
                }
                if (!response.ok) { throw response }
                
            }).then(json => {
                
            }).catch( err => {
                err.text().then( errorMessage => {
                    ShowError(errorMessage);
                    button.disabled = false;
                    button.innerHTML = "Register";
                })
            })
        }
    });
})

function ShowError(text) {
    let errorWrap = document.getElementById("login_error");
    let errorText = document.getElementById("error_text");
    
    errorWrap.style.display = "block";
    errorText.innerHTML = text
}