import { fetchData, hasJwtToken, removeJwtToken } from "./fetchData.js";
document.addEventListener("DOMContentLoaded", async function(){
    let login = document.getElementById("login-button");
    if(!hasJwtToken()) {
        login.style.display = "inline-flex";
        return;
    }
    try {
        let user = await fetchData("/auth/user", "GET");
        let profileDropdown = document.getElementById("profile-dropdown");
        let username = document.getElementById("username");
        if (user.ok) {
            user = await user.json();
            username.innerHTML = `${user.fullName}`;
            profileDropdown.style.display = "inline-flex";
        } else {
            throw new Error('Server error');
        }
    }
    catch (error) {
        
        login.style.display = "inline-flex";
        if(hasJwtToken()){
            removeJwtToken();
        }
    }
    
    
    /*login.href = `/user/profile`;
    login.innerHTML = `${user.fullName}`;*/
});