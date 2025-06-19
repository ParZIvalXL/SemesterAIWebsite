
document.addEventListener("DOMContentLoaded", async function () {
    let form = document.getElementById("payment-form");
    let cardNum = document.getElementById("card-num");
    let expiry = document.getElementById("expiry");
    let cvv = document.getElementById("cvv");
    let button = document.getElementById("submit-btn");
    let selection = document.getElementById("plan");
    
    cardNum.addEventListener("input", function (e) {
        if(cardNum.value.length > 16) {
            cardNum.value = cardNum.value.slice(0, 16);
        }
    })
    
    expiry.addEventListener("input", function (e) {
        if(expiry.value.length > 4) {
            expiry.value = expiry.value.slice(0, 4);
        }
    })
    
    cvv.addEventListener("input", function (e) {
        if(cvv.value.length > 3) {
            cvv.value = cvv.value.slice(0, 3);
        }
    })

    form.addEventListener( "submit", async function (e) {
        e.preventDefault();
        console.log("clicked");
        
        if(!cardNum.value || cardNum.value === "") {
            ShowError("Card number is required");
            return;
        }
        
        if(cardNum.value.length !== 16) {
            ShowError("Card number must be 16 characters");
            return;
        }
        
        if(!cvv.value || cvv.value === "") {
            ShowError("CVV is required");
            return;
        }
        
        if(cvv.value.length !== 3) {
            ShowError("CVV must be 3 characters");
            return;
        }
        
        if(!expiry.value || expiry.value === "") {
            ShowError("CVV is required");
            return;
        }
        
        if(expiry.value.length !== 4) {
            ShowError("Expiry must be 4 characters");
            return;
        }
        
        if (selection.value) {
            const formData = new FormData();
            formData.append("id", selection.value);
            
            button.disabled = true;
            button.innerHTML = "Loading...";
            
            await fetch("/Pricing/buy-subscription", {
                method: "POST",
                body: formData,
            }).then((response) => {
                if (response.ok) {
                    ShowError("Success");
                    button.disabled = false;
                    button.innerHTML = "Buy";
                }
                if (!response.ok) { throw response }
                
            }).then(json => {
                
            }).catch( err => {
                err.text().then( errorMessage => {
                    ShowError(errorMessage);
                    button.disabled = false;
                    button.innerHTML = "Buy";
                })
            })
        }
    });
})

function ShowError(text) {
    let errorWrap = document.getElementById("payment_error");
    let errorText = document.getElementById("error_text");
    
    errorWrap.style.display = "block";
    errorText.innerHTML = text
}