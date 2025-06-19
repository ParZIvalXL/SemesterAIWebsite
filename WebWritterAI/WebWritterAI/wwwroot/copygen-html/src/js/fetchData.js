export async function fetchData(url, method = "GET", body = null, headers = {}) {
    const options = {
        method,
        headers: {
            "Content-Type": "application/json",
            ...headers,
            credentials: 'include'
        },
    };

    if (body) {
        options.body = JSON.stringify(body);
    }

    const response = await fetch(url, options);
    if (!response.ok) throw new Error(`HTTP error ${response.status}`);
    return response;
}

export function hasJwtToken() {
    // Проверка cookies
    const tokenNames = ['jwt', 'token', 'access_token', 'jwtToken', 'ass-token'];
    const cookies = document.cookie.split(';');

    for (const cookie of cookies) {
        const trimmed = cookie.trim();
        for (const name of tokenNames) {
            if (trimmed.startsWith(`${name}=`)) {
                return true;
            }
        }
    }

    return false;
}

export function getJwtToken() {
    const tokenNames = ['jwt', 'token', 'access_token', 'jwtToken', 'ass-token'];
    const cookies = document.cookie.split(';');

    for (const cookie of cookies) {
        const trimmed = cookie.trim();
        for (const name of tokenNames) {
            if (trimmed.startsWith(`${name}=`)) {
                return trimmed.substring(name.length + 1);
            }
        }
    }

    return null;
}

export function removeJwtToken() {
    document.cookie = 'ass-token=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;';
}