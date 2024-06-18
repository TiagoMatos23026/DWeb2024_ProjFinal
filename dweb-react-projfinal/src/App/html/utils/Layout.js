import { useState, useEffect } from "react";
import { Link, useNavigate, Outlet, useLocation } from "react-router-dom";
import 'bootstrap/dist/css/bootstrap.css';
import { getPagesAPI, getUtentesAPI } from "../../api/apiConnection"; // Assuming these functions fetch pages and utentes from an API
import SearchPage from "../SearchPage";
import LoginPage from "../LoginPage";

function Layout() {
    const [isLoggedIn, setLogIn] = useState(false);
    const [searchQuery, setSearchQuery] = useState('');
    const [searchResults, setSearchResults] = useState([]);
    const [pagesList, setPagesList] = useState([]);
    const [utentesList, setUtentesList] = useState([]);
    const [showSearchResults, setShowSearchResults] = useState(false); // State to control when to show search results
    const navigate = useNavigate();
    const location = useLocation();

    useEffect(() => {
        fetchData();
    }, []);

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
        setShowSearchResults(true); // Show search results after filtering
    };

    const handleSearchLinkClick = () => {
        setShowSearchResults(false); // Hide search results when clicking on the search link
    };

    const handleHomePageClick = () => {
        if (location.pathname === "/HomePage") {
            window.location.reload();
        } else {
            navigate("/HomePage");
        }
    };

    return (
        <>
            <nav className="navbar navbar-expand-lg navbar-dark bg-dark sticky-top">
                <Link className="navbar-brand ms-3" to="/HomePage" onClick={handleHomePageClick}>HowToMaster</Link>

                <div className="collapse navbar-collapse d-flex" id="navbarSupportedContent">
                    <form onSubmit={handleSearch} className="d-flex me-auto">
                        <input
                            className="form-control me-2"
                            type="search"
                            placeholder="Search..."
                            value={searchQuery}
                            onChange={(e) => setSearchQuery(e.target.value)}
                        />
                        <button className="btn btn-success ms-2" type="submit">Pesquisar</button>
                    </form>

                    <ul className="navbar-nav d-flex align-items-center me-3">
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
                                    <Link className="btn btn-info ms-2" to="RegisterPage">Registar</Link>
                                </li>
                                <li className="nav-item">
                                    <Link className="btn btn-warning ms-2" to="LoginPage">Log In</Link>
                                </li>
                            </>
                        )}
                    </ul>
                </div>
            </nav>

            {showSearchResults && <SearchPage searchResults={searchResults} />} {/* Render SearchPage only if showSearchResults is true */}
            {!showSearchResults && <Outlet />} {/* Render Outlet (default content) if showSearchResults is false */}
        </>
    );
}

export default Layout;
