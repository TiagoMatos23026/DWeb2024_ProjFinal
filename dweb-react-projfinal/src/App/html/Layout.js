import { useState } from "react";
import { Outlet, Link } from "react-router-dom";
import 'bootstrap/dist/css/bootstrap.css';

function Layout() {
    const [isLoggedIn, setLogIn] = useState(false);

    return (
        <>
            <nav className="navbar navbar-expand-lg navbar-dark bg-dark sticky-top">
                <Link className="navbar-brand ms-3" to="/Home">HowToMaster</Link>

                <div className="collapse navbar-collapse d-flex" id="navbarSupportedContent">
                    <form className="d-flex me-auto">
                        <input className="form-control me-2" type="search" placeholder="" aria-label="Search" />
                        <button className="btn btn-success ms-2" type="submit">Pesquisar</button>
                    </form>

                    <ul className="navbar-nav d-flex align-items-center">
                        {isLoggedIn ? (
                            <>
                                <li className="nav-item">
                                    <Link className="btn btn-primary ms-2" to="/VerPerfil">Meu Perfil</Link>
                                </li>
                                <li className="nav-item">
                                    <Link className="btn btn-danger ms-2" to="/CriarPagina">Criar Página</Link>
                                </li>
                                <li className="nav-item">
                                    <button className="btn btn-warning ms-2" onClick={() => setLogIn(false)}>Log Out</button>
                                </li>
                            </>
                        ) : (
                            <>
                                <li className="nav-item">
                                    <Link className="btn btn-info ms-2" to="/CriarUtilizador">Registar</Link>
                                </li>
                                <li className="nav-item">
                                    <button className="btn btn-warning ms-2" onClick={() => setLogIn(true)}>Log In</button>
                                </li>
                            </>
                        )}
                    </ul>
                </div>
            </nav>

            <Outlet />
        </>
    );
}

export default Layout;
