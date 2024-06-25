/*export async function getUtentesAPI() {
    var users = null;
    users = await fetch("https://localhost:7027/api/UtentesAPI/")
        .then(res => res.json())
        .catch(error => console.log('error', error));

    return users;
}*/

export function getUtentesAPI(){
    return fetch("http://localhost:5101/api/UtentesAPI")
}

export const getPagesAPI = async () => {
    return fetch("http://localhost:5101/api/PaginasAPI")
};

export const getPagesDetailsAPI = async () => {
    const response = await fetch("http://localhost:5101/api/PaginasAPI");
    if (!response.ok) {
        throw new Error('Network response was not ok');
    }
    return response.json();
};

export const getUtentesDetailsAPI = async (email) => {
    const response = await fetch(`http://localhost:5101/api/UtentesAPI?email=${encodeURIComponent(email)}`);
    if (!response.ok) {
        throw new Error('Network response was not ok');
    }
    return response.json();
};
