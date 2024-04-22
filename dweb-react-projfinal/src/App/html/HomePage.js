import { AppContext } from "../../App";
import 'bootstrap/dist/css/bootstrap.min.css';

import { useEffect, useState, useContext } from "react";
import { Outlet, Link } from "react-router-dom";
import 'bootstrap/dist/css/bootstrap.css';
import { getUtenteAPI } from "../api/apiConnection";



function FrontPage() {

    const user = getUtenteAPI(1);

    const ctx = useContext(AppContext);
    
    const [showModal, setShowModal] = useState(false);
    const [isLoggedIn, setLogIn] = useState(false);


    return <>
        <div className="container-fluid">
            <div className="row justify-content-start">
                <h4> {user} </h4>

            </div>
        </div>


    </>





}

export default FrontPage;