import { AppContext } from "../../App";
import 'bootstrap/dist/css/bootstrap.min.css';

import { useEffect, useState, useContext } from "react";
import { Outlet, Link } from "react-router-dom";
import 'bootstrap/dist/css/bootstrap.css';
import { getUtentesAPI, getPagesAPI } from "../api/apiConnection";

function FrontPage() {
    const [pagesList, setPagesList] = useState([]);
    const [utentesList, setUtentesList] = useState([]);
    const [showModal, setShowModal] = useState(false);
    const [isLoggedIn, setLogIn] = useState(false);

    const fetchData = async () => {
        try {
            const [pagesResponse, utentesResponse] = await Promise.all([getPagesAPI(), getUtentesAPI()]);
            const pagesData = await pagesResponse.json();
            const utentesData = await utentesResponse.json();
            setPagesList(pagesData);
            setUtentesList(utentesData);
        } catch (error) {
            console.error('Erro', error);
        }
    };

    useEffect(() => {
        fetchData();
    }, []);

    const renderPages = () => {
        return pagesList.map((page, index) => {
            const autor = utentesList.find(utente => utente.id === page.utenteFK)?.nome || 'Desconhecido';
            return (
                <div className="col-3 mt-3" key={index}>
                    <div className="card-body">
                        <img className="card-img-top" src="https://picsum.photos/500/500"></img>
                        <div className="card-body">
                            <h5 className="card-title ms-1">{page.name}</h5>
                            <h5 className="card-title ms-1">Autor: {autor}</h5>
                        </div>
                    </div>
                </div>
            );
        });
    };

    return (
        <div className="container-fluid">
            <div className="row justify-content-start">
                {renderPages()}
            </div>
        </div>
    );
}

export default FrontPage;
