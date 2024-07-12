import { useState, useEffect } from "react";
import { Link, useNavigate, Outlet, useLocation } from "react-router-dom";
import 'bootstrap/dist/css/bootstrap.css';
import axios from 'axios';
import { getPagesAPI, getUtentesAPI } from "../../api/apiConnection";
import SearchPage from "../SearchPage";
import LoginPage from "../LoginPage";
import { useAuth } from "../../../App";

function Layout() {
    const [searchQuery, setSearchQuery] = useState('');
    const [searchResults, setSearchResults] = useState([]);
    const [showSearchResults, setShowSearchResults] = useState(false);

    const [loggedUser, setLoggedUser] = useState(null);

    const { user, setUser } = useAuth();
    const navigate = useNavigate();
    const location = useLocation();

    const handleHomePageClick = () => {
        navigate("/HomePage");
    };

    const handleProfileClick = (id) => {
        navigate("/ProfilePage", { state: { id } });
    };

    const handleLogout = async () => {
        try {
            await axios.post('http://localhost:5101/api/AccountAPI/logout', {}, {
                withCredentials: true,
            });
            setUser(null);
            navigate("/HomePage");
        } catch (error) {
            console.error('Error logging out:', error);
            alert('Erro ao sair. Por favor, tente novamente.');
        }
    };

    return (
        <>
            <nav className="navbar navbar-expand-lg navbar-dark bg-dark sticky-top">
                <Link className="navbar-brand ms-3" to="/HomePage" onClick={handleHomePageClick}>HowToMaster</Link>

                <div className="collapse navbar-collapse d-flex" id="navbarSupportedContent">
                    <form className="d-flex me-auto">
                        <input
                            className="form-control me-2"
                            type="search"
                            placeholder="Pesquisa..."
                        />
                        <button className="btn btn-success ms-2" type="submit">Pesquisar</button>
                    </form>

                    <ul className="navbar-nav d-flex align-items-center me-3">
                        {user !== 'null' && user !== null ? (
                            <>
                                <li className="nav-item">
                                    <button className="btn btn-primary ms-2" onClick={() => handleProfileClick(user.id)}>Meu Perfil</button>
                                </li>
                                <li className="nav-item">
                                    <Link className="btn btn-danger ms-2" to="/CreatePage">Criar Página</Link>
                                </li>
                                <li className="nav-item">
                                    <button className="btn btn-warning ms-2" onClick={handleLogout}>Log Out</button>
                                </li>
                            </>
                        ) : (
                            <>
                                <li className="nav-item">
                                    <Link className="btn btn-info ms-2" to="/RegisterPage">Registar</Link>
                                </li>
                                <li className="nav-item">
                                    <Link className="btn btn-warning ms-2" to="/LoginPage">Log In</Link>
                                </li>
                            </>
                        )}
                    </ul>
                </div>
            </nav>

            {showSearchResults && <SearchPage searchResults={searchResults} />}
            {!showSearchResults && <Outlet />}
        </>
    );
}

export default Layout;
