import { useEffect, useState } from "react";
import { Outlet, Link } from "react-router-dom";
import 'bootstrap/dist/css/bootstrap.css';

function Layout() {
    const [showModal, setShowModal] = useState(false);
    const [isLoggedIn, setLogIn] = useState(false);

    

    return <>
        <div>
            <nav className="navbar navbar-expand-lg navbar-dark bg-dark sticky-top">

                <Link className="navbar-brand ms-3" to="/Home">HowToMaster</Link>

                <div className="collapse navbar-collapse" id="navbarSupportedContent">


                    <form className="d-flex">
                        <input className="form-control me-2" type="search" placeholder="" aria-label="Search"></input>
                        <button className="btn btn-success ms-2" type="submit">Pesquisar</button>
                    </form>


                    {isLoggedIn && <ul className="navbar-nav me-auto mb-2 mb-lg-0">
                        <li className="nav-item">
                            <Link className="btn btn-primary ms-3" to="/VerPerfil">Meu Perfil</Link>
                        </li>
                        <li className="nav-item">
                            <Link className="btn btn-danger ms-3" to="/CriarPagina">Criar Página</Link>
                        </li>
                    </ul>}

                    {!isLoggedIn && <ul className="navbar-nav">
                        <li className="nav-item">
                            <Link className="btn btn-info ms-3" to="/CriarUtilizador">Registar</Link>
                        </li>
                        <li className="nav-item">
                            <button className="btn btn-warning ms-3" onClick={() => {setLogIn(true)}}/*to="/Login"*/ >Log In</button>
                        </li>
                    </ul>}

                    {isLoggedIn && <ul className="navbar-nav">
                        <li className="nav-item">
                            <button className="btn btn-warning ms-3" onClick={() => {setLogIn(false)}}/*to="/Logout"*/ >Log Out</button>
                        </li>
                    </ul>}


                </div>
            </nav>

            <Outlet />
        </div>


    </>





}

export default Layout;