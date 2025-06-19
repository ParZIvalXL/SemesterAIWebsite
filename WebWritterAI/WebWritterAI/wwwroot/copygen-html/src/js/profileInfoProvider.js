import { fetchData, hasJwtToken } from "./fetchData.js";

document.addEventListener("DOMContentLoaded", async function(){
    if(!hasJwtToken()) return;
    let user = await fetchData("/auth/user", "GET", );

    console.log(user);
    if(!user.ok) {
        return;
    }
    
    user = await user.json();
    const tier = document.getElementById("info-tier");
    const subText = document.getElementById("info-sub");
    if(user.role === "admin" || user.role === "user") tier.innerHTML = "You are: " + user.role;
    if (user.role === "admin") {
        tier.insertAdjacentHTML('afterend', '<a href="/admin/panel" class="link link-primary">Admin panel</a>')
    }
    else 
    {
        try {
            let params = new URLSearchParams();
            params.append("id", user.role);
            const response = await fetch(`/Pricing/subscription?${params}`, {method: "GET"});
            if (!response.ok) {
                throw new Error(`Response status: ${response.status}`);
            }
    
            const json = await response.json();
            if(response.ok) {
                subText.innerHTML = `Your subscription is: ${json.name} costs ${json.price}, it allows you to generate text with limit of ${json.wordsLimit} words in ${json.languages} languages. You can use ${json.templatesLimit} templates.` +
                `You can cancel your subscription using <a href="/user/cancel-subscription" class="link link-primary" id="cancel-sub">this link</a>`;
                subText.style.display = "block";

                const cancelSub = document.getElementById("cancel-sub");
                cancelSub.addEventListener("click", async function(e) {
                    e.preventDefault();
                    let response = await fetch("/user/cancel-subscription", {method: "POST"});

                    if (!response.ok) {
                        throw new Error(`Response status: ${response.status}`);
                    }

                    location.reload();
                });
            }
            
            tier.innerHTML = `Your tier is: ${json.name}`;
            console.log(json);
        } catch (error) {
            console.error(error.message);
        }
    }
    
    
    
    const greeting = document.getElementById("greeting");
    greeting.innerHTML = `Hello, ${user.fullName}!`;
    const fullnameText = document.getElementById("info-full-name");
    fullnameText.innerHTML = `${user.fullName}`;
    const emailText = document.getElementById("info-email");
    emailText.innerHTML = `${user.email}`;
});


