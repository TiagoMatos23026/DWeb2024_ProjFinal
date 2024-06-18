import { useEffect, useState, useContext } from "react";
import { Outlet, Link, useNavigate } from "react-router-dom";
import 'bootstrap/dist/css/bootstrap.min.css';
import 'bootstrap/dist/css/bootstrap.css';
import { getUtentesAPI, getPagesAPI } from "../api/apiConnection";

function FrontPage() {
    const [pagesList, setPagesList] = useState([]);
    const [utentesList, setUtentesList] = useState([]);
    const [showModal, setShowModal] = useState(false);
    const [isLoggedIn, setLogIn] = useState(false);
    const navigate = useNavigate();

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

    const handleCardClick = (page) => {
        const autor = utentesList.find(utente => utente.id === page.utenteFK);
        navigate('/ViewPage', { state: { page, autor } });
    };

    const renderPages = () => {
        return pagesList.map((page, index) => {
            const autor = utentesList.find(utente => utente.id === page.utenteFK)?.nome || 'Desconhecido';
            return (
                <div className="col-12 col-sm-6 col-md-4 col-lg-3 mt-3" key={index}>
                    <div className="card p-3 border border-2 rounded-3" style={{ height: '100%' }} onClick={() => handleCardClick(page)}>
                        <img className="card-img-top" src="https://picsum.photos/id/1/200/300" alt="Page image" style={{ height: '150px', objectFit: 'cover' }} />
                        <div className="card-body">
                            <h5 className="card-title">{page.name}</h5>
                            <h6 className="card-subtitle mb-2 text-muted">Autor: {autor}</h6>
                        </div>
                    </div>
                </div>
            );
        });
    };

    return (
        <div className="container-fluid">
            <div className="row">
                {renderPages()}
            </div>
        </div>
    );
}

export default FrontPage;
