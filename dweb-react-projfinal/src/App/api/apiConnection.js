/*export async function getUtentesAPI() {
    var users = null;
    users = await fetch("https://localhost:7027/api/UtentesAPI/")
        .then(res => res.json())
        .catch(error => console.log('error', error));

    return users;
}*/

export const getUtentes = async () => {
    const res = await fetch("http://localhost:5101/api/UtentesAPI")
    if (!res.ok) {
        throw new Error('Houve um problema na rede');
    }
    return res.json();
}

export const getPages = async () => {
    const res = await fetch("http://localhost:5101/api/PaginasAPI")
    if (!res.ok) {
        throw new Error('Houve um problema na rede');
    }
    return res.json();
};

export const getPagesByUtente = async (utenteFk) => {
    const res = await fetch("http://localhost:5101/api/PaginasAPI/utente/" + utenteFk)
    if (!res.ok) {
        throw new Error('Houve um problema na rede');
    }
    return res.json();
};

export const getPageDetails = async () => {
    const res = await fetch("http://localhost:5101/api/PaginasAPI");
    if (!res.ok) {
        throw new Error('Houve um problema na rede');
    }
    return res.json();
};

export const getUtenteDetails = async (id) => {
    const res = await fetch("http://localhost:5101/api/UtentesAPI/" + id);
    if (!res.ok) {
        throw new Error('Houve um problema na rede');
    }
    return res.json();
};

export const postLogin = async (formData) =>{
    const res = await fetch("http://localhost:5101/api/UtentesAPI/login", {
    method: 'POST',
    body: formData
  });
  if (!res.ok) {
    throw new Error('Houve um problema no login');
  }
  return res.json();
}
/*
export const login = async (email, password) => {
    // Create a FormData object
    const formData = new FormData();
    formData.append('email', email);
    formData.append('password', password);

    // Send the request using fetch
    const res = await fetch("https://dwebprojfinalhowtomasterapp.azurewebsites.net/api/UtentesAPI/login", {
        method: 'POST',
        headers: {
            // 'Content-Type': 'multipart/form-data' is automatically set by the browser when using FormData
        },
        body: formData
    });

    if (!res.ok) {
        throw new Error('Houve um problema no login');
    }
    return res.json();
}*/


