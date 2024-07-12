import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { getUtenteDetails, getPagesByUtente } from '../api/apiConnection';
import { useNavigate, useLocation } from "react-router-dom";
import { useAuth } from '../../App';

function ProfilePage() {
    const location = useLocation();
    const { id } = location.state;
    const [pages, setPages] = useState([]);
    const [utente, setUtente] = useState(null);

    const { user, setUser } = useAuth();
    const navigate = useNavigate();

    const handleVoltar = () => {
        navigate("/HomePage");
    }

    const handleEdit = () => {
        navigate("/EditProfile", { state: { id } });
    }

    const handleDeletePage = async (pageId) => {
        // Confirm before deleting
        const confirmed = window.confirm("Tem certeza de que deseja excluir esta página?");
        if (!confirmed) return;

        try {
            await axios.delete(`http://localhost:5101/api/PaginasAPI/${pageId}`, {
                withCredentials: true, // Include credentials (cookies)
            });

            // Remove the deleted page from the local state
            setPages(pages.filter(page => page.id !== pageId));
            alert('Página excluída com sucesso!');
        } catch (error) {
            console.error('Error deleting page:', error);
            alert('Houve um problema ao excluir a página.');
        }
    }

    const handleCardClick = (page) => {
        const autor = utente;
        navigate('/ViewPage', { state: { page, autor } });
    };

    async function fetchData() {
        try {
            const utenteData = await getUtenteDetails(id);
            const pagesData = await getPagesByUtente(id);
            setUtente(utenteData);
            setPages(pagesData);
        } catch (error) {
            console.error('Error fetching data:', error);
        }
    }

    useEffect(() => {
        fetchData();
    }, [id]);

    const renderPages = () => {
        if (!utente) {
            return <div>A carregar...</div>;
        }

        const baseUrl = "http://localhost:5101/imagens/";

        return (
            <div className="container mt-5">
                <div className="row">
                    <div className="col-md-3">
                        <div className="card">
                            <img
                                src={`${baseUrl}${utente.icon}`}
                                className="card-img-top"
                                alt="Profile"
                            />
                            <div className="card-body text-center">
                                <h5 className="card-title">{utente.nome}</h5>
                            </div>
                        </div>
                        <div className="text-center mt-4">
                            <button className="btn btn-success me-4" onClick={handleEdit}>Editar Perfil</button>
                            <button className="btn btn-warning" onClick={handleVoltar}>Voltar</button>                        
                        </div>
                    </div>
                    <div className="col-md-9">
                        <div className="card mb-3">
                            <div className="card-body">
                                <h5 className="card-title">Biografia</h5>
                                <p className="card-text">
                                    {utente.biografia || 'Sem biografia.'}
                                </p>
                            </div>
                        </div>
                        <div className="card mb-3">
                            <div className="card-body">
                                <h5 className="card-title">Páginas</h5>
                                <div className="row">
                                    {pages.map((page, idx) => (
                                        <div className="col-md-4 mb-3" key={page.id}>
                                            <div className="card">
                                                <img
                                                    onClick={() => handleCardClick(page)}
                                                    src={`${baseUrl}${page.thumbnail}`}
                                                    className="card-img-top"
                                                    alt={`Page ${idx + 1}`}
                                                    style={{ height: '150px', objectFit: 'cover' }}
                                                />
                                                <div className="card-body">
                                                    <h6 className="card-title">{page.name}</h6>
                                                    <button className="btn btn-danger" onClick={() => handleDeletePage(page.id)}>Apagar</button>
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
}

export default ProfilePage;
