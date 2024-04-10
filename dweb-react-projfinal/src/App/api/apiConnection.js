export function getPaginasAPI() {
    return fetch("https://spring-server.azurewebsites.net/todo/getTarefas");
}

{/*export function getPaginasAPIPaged(idPagina) {
    return fetch("https://spring-server.azurewebsites.net/todo/getTarefasPaged?idPagina="+idPagina
    +"&paginaSize=5");
}*/}

export function createPaginaAPI(pagina) {
    return fetch("https://spring-server.azurewebsites.net/todo/createTarefa", {
        body: JSON.stringify(pagina),
        headers: {
            Accept: "*/*",
            "Content-Type": "application/json"
        },
        method: "POST"
    });
}

export function deletePaginaAPI(id) {
    return fetch("https://spring-server.azurewebsites.net/todo/deleteTarefa?idTarefa=" + id, {
        headers: {
            Accept: "*/*"
        },
        method: "DELETE"
    })
}

export function editPaginaAPI(paginaEditar) {
    return fetch("https://spring-server.azurewebsites.net/todo/updateTarefa?tarefaId="+paginaEditar.id, {
        body: JSON.stringify(paginaEditar),
        headers: {
            Accept: "*/*",
            "Content-Type": "application/json"
        },
        method: "PUT"
    })
}