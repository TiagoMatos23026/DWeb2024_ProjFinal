import { AppContext } from "../../App";
import 'bootstrap/dist/css/bootstrap.min.css';

import { useEffect, useState, useContext } from "react";
import { Outlet, Link } from "react-router-dom";
import 'bootstrap/dist/css/bootstrap.css';
import { getUtentesAPI } from "../api/apiConnection";

function FrontPage() {

    const [pagesList, setPagesList] = useState(null);
    const [utentesList, setUtentesList] = useState([]);
    const [showModal, setShowModal] = useState(false);
    const [isLoggedIn, setLogIn] = useState(false);
    const [users, setUsers] = useState([]);


    const fetchData = async () => {
        try {
            // Chama a api, e só continua quando tiver uma resposta
            // metodo bloqueante, caso precise do data para chamar outros métodos

            /*const data = await getUtentesAPI();
            data = data.json();
            console.log(data);*/

            // metodo nao bloqueante, os dados só estão disponiveis no segundo then
            getUtentesAPI()
                .then(res => res.json())
                .then(response => setUtentesList(response));

            // Guarda a resposta da api em state
        } catch (error) {
            console.error('Erro', error);
        }
    }

    useEffect(() => {
        fetchData();
    }, []);

    for (let i = 0; i < utentesList.length; i++) {
        users.push(
            <div className="col-3 mt-3">
                <div className="card-body">
                    <h5 className="card-title ms-1">Utilizador: {utentesList[i].nome}</h5>
                </div>
            </div>
        )
    }


    return <>
    
        <div className="container-fluid">
            <button onClick={() => {console.log(utentesList.length)}}></button>
            <div className="row justify-content-start">
                {users}

            </div>
        </div>


    </>





}

export default FrontPage;