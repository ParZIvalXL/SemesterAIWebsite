
document.addEventListener("DOMContentLoaded", async function () {
    let form = document.getElementById("reset-form");
    let email = document.getElementById("email");
    let newPassword = document.getElementById("password");
    let button = document.getElementById("submit-btn");

    form.addEventListener( "submit", async function (e) {
        e.preventDefault();
        console.log("clicked");
        
        if(!email.value || email.value === "") {
            ShowError("Email is required");
            return;
        }
        
        if(!email.value.includes("@") || !email.value.includes(".")) {
            ShowError("Invalid email");
            return;
        }
        
        if(!newPassword.value || newPassword.value === "") {
            ShowError("Password is required");
            return;
        }
        
        if(newPassword.value.length < 8) {
            ShowError("Password must be at least 8 characters");
            return;
        }
        
        if (email.value) {
            const formData = new FormData(form);
            
            button.disabled = true;
            button.innerHTML = "Loading...";
            
            await fetch("/auth/reset", {
                method: "POST",
                body: formData,
            }).then((response) => {
                if (response.ok) {
                    window.location.href = "/auth/login";
                }
                if (!response.ok) { throw response }
                
            }).then(json => {
                
            }).catch( err => {
                err.text().then( errorMessage => {
                    ShowError(errorMessage);
                    button.disabled = false;
                    button.innerHTML = "Send Reset Link";
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