export function isAuthenticated() {
    const token = localStorage.getItem('user-token');

    if (token) {
        const jwtPayload = JSON.parse(atob(token.split('.')[1]));

        return jwtPayload && jwtPayload.exp > Date.now() / 1000;
    }

    return false;
}

export function logout() {
    localStorage.removeItem('user-token');
}