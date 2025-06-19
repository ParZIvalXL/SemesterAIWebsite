
document.addEventListener("DOMContentLoaded", async function () {
    let loginForm = document.getElementById("login_form");
    let email = document.getElementById("Email");
    let password = document.getElementById("Password");
    let loginButton = document.getElementById("submit-btn");

    loginForm.addEventListener( "submit", async function (e) {
        e.preventDefault();
        console.log("clicked");
        if((!email.value || email.value === "") && (!password.value || password.value === "")) {
            ShowError("All fields are required!");
            return;
        }
        if(!email.value || email.value === "") {
            ShowError("Login is required");
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
            const formData = new FormData(loginForm);
            
            loginButton.disabled = true;
            loginButton.innerHTML = "Logging in...";
            
            await fetch("/auth/login", {
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
                    loginButton.disabled = false;
                    loginButton.innerHTML = "Login";
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