import { fetchData, hasJwtToken } from "./fetchData.js";

document.addEventListener("DOMContentLoaded", async function(){
    if(!hasJwtToken())
    {
        window.location.href = "/auth/login";
    }
});