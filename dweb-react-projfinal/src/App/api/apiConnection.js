/*export async function getUtentesAPI() {
    var users = null;
    users = await fetch("https://localhost:7027/api/UtentesAPI/")
        .then(res => res.json())
        .catch(error => console.log('error', error));

    return users;
}*/

export const getUtentesAPI = async () => {
    const res = await fetch("https://dwebprojfinalhowtomasterapp.azurewebsites.net/api/UtentesAPI")
    if (!res.ok) {
        throw new Error('Houve um problema na rede');
    }
    return res.json();
}

export const getPagesAPI = async () => {
    const res = await fetch("https://dwebprojfinalhowtomasterapp.azurewebsites.net/api/PaginasAPI")
    if (!res.ok) {
        throw new Error('Houve um problema na rede');
    }
    return res.json();
};

export const getPagesDetailsAPI = async () => {
    const res = await fetch("https://dwebprojfinalhowtomasterapp.azurewebsites.net/api/PaginasAPI");
    if (!res.ok) {
        throw new Error('Houve um problema na rede');
    }
    return res.json();
};

export const getUtentesDetailsAPI = async (email) => {
    const res = await fetch(`https://dwebprojfinalhowtomasterapp.azurewebsites.net/api/UtentesAPI?email=${encodeURIComponent(email)}`);
    if (!res.ok) {
        throw new Error('Houve um problema na rede');
    }
    return res.json();
};
