import React, { useState, useEffect } from 'react';
import { getUtentesDetailsAPI, getPagesDetailsAPI, getPagesAPI, getUtentesAPI } from '../api/apiConnection';
import { useNavigate } from "react-router-dom";

const ProfilePage = () => {
    const [utentesList, setUtentesList] = useState(null);
    const [pagesList, setPagesList] = useState([]);
    const navigate = useNavigate();

    // Fetch the logged-in user's email from sessionStorage
    const email = sessionStorage.getItem('userLogged');

    const fetchData = async () => {
        try {
            const [pagesResponse, utentesResponse] = await Promise.all([getPagesAPI(), getUtentesAPI()]);
            const pagesData = await pagesResponse.json();
            const utentesData = await utentesResponse.json();
            setPagesList(pagesData);
            setUtentesList(utentesData);
        } catch (error) {
            console.error('Error fetching data:', error);
        }
    };

    const handleCardClick = (page) => {
        const autor = utentesList.find(utente => utente.id === page.utenteFK);
        navigate('/ViewPage', { state: { page, autor } });
    };

    const handleDelete = async (id) => {
        try {
            const response = await fetch(`http://localhost:5101/api/PaginasAPI/${id}`, {
                method: 'DELETE',
                headers: {
                    'Content-Type': 'application/json'
                },
                // Optionally, you can include credentials such as session cookies
                credentials: 'include',
            });
            if (response.ok) {
                // If successful, remove the deleted page from the state
                setPagesList(pagesList.filter(page => page.id !== id));
            } else {
                console.error('Failed to delete page');
            }
        } catch (error) {
            console.error('Error deleting page:', error);
        }
    };

    useEffect(() => {
        fetchData();
    }, []);

    const renderPages = () => {
        if (!utentesList || !pagesList) {
            return <div>Loading...</div>;
        }

        const userLogged = utentesList.find(utente => utente.email === email);
        const baseUrl = "http://localhost:5101/imagens/";

        const filteredPages = pagesList.filter(page => page.utenteFK === userLogged.id);

        return (
            <div className="container mt-5">
                <div className="row">
                    <div className="col-md-3">
                        <div className="card">
                            <img
                                src={`${baseUrl}${userLogged.icon}`}
                                className="card-img-top"
                                alt="Profile"
                            />
                            <div className="card-body text-center">
                                <h5 className="card-title">{userLogged.nome}</h5>
                            </div>
                        </div>
                    </div>
                    <div className="col-md-9">
                        <div className="card mb-3">
                            <div className="card-body">
                                <h5 className="card-title">Biografia</h5>
                                <p className="card-text">
                                    {userLogged.biografia || 'No biography available.'}
                                </p>
                            </div>
                        </div>
                        <div className="card mb-3">
                            <div className="card-body">
                                <h5 className="card-title">Páginas</h5>
                                <div className="row">
                                    {filteredPages.map((page, idx) => (
                                        <div className="col-md-4 mb-3" key={idx}>
                                            <div className="card">
                                                <img
                                                    onClick={() => handleCardClick(page)}
                                                    src={`${baseUrl}${page.thumbnail}`}
                                                    className="card-img-top"
                                                    alt={`Page ${idx + 1}`}
                                                />
                                                <div className="card-body">
                                                    <h6 className="card-title">{page.name}</h6>
                                                    <p className="card-text">{page.descricao}</p>
                                                    <button className="btn btn-danger" onClick={() => handleDelete(page.id)}>Apagar</button>
                                                </div>
                                            </div>
                                        </div>
                                    ))}
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        );
    };

    return (
        <div>
            {renderPages()}
        </div>
    );
};

export default ProfilePage;
