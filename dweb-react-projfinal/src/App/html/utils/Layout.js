import { useState, useEffect } from "react";
import { Link, useNavigate, Outlet, useLocation } from "react-router-dom";
import 'bootstrap/dist/css/bootstrap.css';
import { getPagesAPI, getUtentesAPI } from "../../api/apiConnection";
import SearchPage from "../SearchPage";
import LoginPage from "../LoginPage";
import { useAuth } from "../../../App";

function Layout() {
    const [isLoggedIn, setLogIn] = useState(false);
    const [searchQuery, setSearchQuery] = useState('');
    const [searchResults, setSearchResults] = useState([]);
    const [pagesList, setPagesList] = useState([]);
    const [utentesList, setUtentesList] = useState([]);
    const [showSearchResults, setShowSearchResults] = useState(false);
    const [sessionUser, setSessionUser] = useState(sessionStorage.getItem('userLogged'));
    const navigate = useNavigate();
    const location = useLocation();

    useEffect(() => {
        fetchData();
    }, []);

    useEffect(() => {
        setSessionUser(sessionStorage.getItem('userLogged')); 
    }, [sessionStorage.getItem('userLogged')]);

    const fetchData = async () => {
        try {
            const [pagesResponse, utentesResponse] = await Promise.all([getPagesAPI(), getUtentesAPI()]);
            const pagesData = await pagesResponse.json();
            const utentesData = await utentesResponse.json();
            setPagesList(pagesData);
            setUtentesList(utentesData);
        } catch (error) {
            console.error('Error fetching data', error);
        }
    };

    const handleSearch = (e) => {
        e.preventDefault();
        const filteredResults = pagesList.filter(page => page.name.toLowerCase().includes(searchQuery.toLowerCase()));
        setSearchResults(filteredResults);
        setShowSearchResults(true); 
    };

    const handleSearchLinkClick = () => {
        setShowSearchResults(false); 
    };

    const handleHomePageClick = () => {
        if (location.pathname === "/HomePage") {
            window.location.reload();
        } else {
            navigate("/HomePage");
        }
    };

    const handleLogout = () => {
        sessionStorage.setItem('userLogged', 'null');
        setSessionUser('null');
        navigate("/HomePage");
    };

    const { user, setUser } = useAuth();

    return (
        <>
            <nav className="navbar navbar-expand-lg navbar-dark bg-dark sticky-top">
                <Link className="navbar-brand ms-3" to="/HomePage" onClick={handleHomePageClick}>HowToMaster</Link>

                <div className="collapse navbar-collapse d-flex" id="navbarSupportedContent">
                    <form onSubmit={handleSearch} className="d-flex me-auto">
                        <input
                            className="form-control me-2"
                            type="search"
                            placeholder="Pesquisa..."
                            value={searchQuery}
                            onChange={(e) => setSearchQuery(e.target.value)}
                        />
                        <button className="btn btn-success ms-2" type="submit">Pesquisar</button>
                    </form>

                    <ul className="navbar-nav d-flex align-items-center me-3">
                        {sessionUser !== 'null' && sessionUser !== null ? (
                            <>
                                <li className="nav-item">
                                    <Link className="btn btn-primary ms-2" to="/ProfilePage">Meu Perfil</Link>
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
