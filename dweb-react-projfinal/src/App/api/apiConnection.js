export const getUtentes = async () => {
    const res = await fetch("https://dwebprojfinalhowtomasterapp.azurewebsites.net/api/UtentesAPI", {
        method: 'GET',
        credentials: 'include', // Include credentials (cookies)
    });
    if (!res.ok) {
        throw new Error('Houve um problema na rede');
    }
    return res.json();
}

export const getPages = async () => {
    const res = await fetch("https://dwebprojfinalhowtomasterapp.azurewebsites.net/api/PaginasAPI", {
        method: 'GET',
        credentials: 'include', // Include credentials (cookies)
    });
    if (!res.ok) {
        throw new Error('Houve um problema na rede');
    }
    return res.json();
};

export const getPagesByUtente = async (utenteFk) => {
    const res = await fetch(`https://dwebprojfinalhowtomasterapp.azurewebsites.net/api/PaginasAPI/utente/${utenteFk}`, {
        method: 'GET',
        credentials: 'include', // Include credentials (cookies)
    });
    if (!res.ok) {
        throw new Error('Houve um problema na rede');
    }
    return res.json();
};

export const getPageDetails = async () => {
    const res = await fetch("https://dwebprojfinalhowtomasterapp.azurewebsites.net/api/PaginasAPI", {
        method: 'GET',
        credentials: 'include', // Include credentials (cookies)
    });
    if (!res.ok) {
        throw new Error('Houve um problema na rede');
    }
    return res.json();
};

export const getUtenteDetails = async (id) => {
    const res = await fetch(`https://dwebprojfinalhowtomasterapp.azurewebsites.net/api/UtentesAPI/${id}`, {
        method: 'GET',
        credentials: 'include', // Include credentials (cookies)
    });
    if (!res.ok) {
        throw new Error('Houve um problema na rede');
    }
    return res.json();
};

export const getLoggedUtenteDetails = async (email) => {
    const res = await fetch(`https://dwebprojfinalhowtomasterapp.azurewebsites.net/api/UtentesAPI/email/${email}`, {
        method: 'GET',
        credentials: 'include', // Include credentials (cookies)
    });
    if (!res.ok) {
        throw new Error('Houve um problema na rede');
    }
    return res.json();
};

export const postLogin = async (formData) => {
    const res = await fetch("https://dwebprojfinalhowtomasterapp.azurewebsites.net/api/UtentesAPI/login", {
        method: 'POST',
        body: formData,
        credentials: 'include', // Include credentials (cookies)
    });
    if (!res.ok) {
        throw new Error('Houve um problema no login');
    }
    return res.json();
}
