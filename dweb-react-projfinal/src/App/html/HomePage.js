import { AppContext } from "../../App";
import 'bootstrap/dist/css/bootstrap.min.css';

import { useEffect, useState, useContext } from "react";
import { Outlet, Link } from "react-router-dom";
import 'bootstrap/dist/css/bootstrap.css';
import { getUtenteAPI } from "../api/apiConnection";



function FrontPage() {
    const [showModal, setShowModal] = useState(false);
    const [isLoggedIn, setLogIn] = useState(false);

    const [pagesList, setPagesList] = useState([]);
    const [usersList, setUsersList] = useState([]);

    setUsersList(getUtentesAPI());


    for (let i = 0; i < usersList.length; i++) {
        usersList.push(
            <div className="col-3 mt-3">
                <div className="card-body">
                    <img className="card-img-top rounded float-start" alt="imagem"></img>
                    <h5 className="card-title ms-1">Nome</h5>
                    <p className="card-text ms-1">Autor</p>
                </div>
            </div>
        )
    }


    return <>
        <div className="container-fluid">
            <div className="row justify-content-start">
                <h4></h4>

            </div>
        </div>


    </>





}

export default FrontPage;