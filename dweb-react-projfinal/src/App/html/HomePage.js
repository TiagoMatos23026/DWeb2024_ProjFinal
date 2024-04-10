import { AppContext } from "../../App";
import 'bootstrap/dist/css/bootstrap.min.css';

import { useEffect, useState, useContext } from "react";
import { Outlet, Link } from "react-router-dom";
import 'bootstrap/dist/css/bootstrap.css';



function FrontPage() {

    const ctx = useContext(AppContext);
    
    const [showModal, setShowModal] = useState(false);
    const [isLoggedIn, setLogIn] = useState(false);


    return <>
        <div className="container-fluid">
            <div className="row justify-content-start">
                <h4>Olá {ctx.context.user}</h4>

            </div>
        </div>


    </>





}

export default FrontPage;