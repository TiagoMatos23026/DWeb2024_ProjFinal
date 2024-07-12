import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import 'bootstrap/dist/css/bootstrap.min.css';
import 'bootstrap/dist/css/bootstrap.css';
import { getUtentes, getPages } from "../api/apiConnection";
import { useAuth } from "../../App";

function HomePage() {
    const [pagesList, setPagesList] = useState([]);
    const [utentesList, setUtentesList] = useState([]);
    const navigate = useNavigate();
    const { user } = useAuth();

    const shufflePages = (array) => {
        for (let i = array.length - 1; i > 0; i--) {
            const j = Math.floor(Math.random() * (i + 1));
            [array[i], array[j]] = [array[j], array[i]];
        }
        return array;
    };

    const fetchData = async () => {
        try {
            let utentesData = await getUtentes();
            let pagesData = await getPages();
            setUtentesList(shufflePages(utentesData));
            setPagesList(shufflePages(pagesData));
        } catch (error) {
            console.error("Error fetching data: ", error);
        }
    };

    useEffect(() => {
        fetchData();
    }, [user]); // Added user as a dependency

    const handleCardClick = (page) => {
        const autor = utentesList.find(utente => utente.id === page.utenteFK);
        navigate('/ViewPage', { state: { page, autor } });
    };

    const renderPages = () => {
        const baseUrl = "http://localhost:5101/imagens/";
        return pagesList.map((page, index) => {
            const autor = utentesList.find(utente => utente.id === page.utenteFK)?.nome || 'Desconhecido';
            const imagePath = `${baseUrl}${page.thumbnail}`;
            return (
                <div className="col-12 col-sm-6 col-md-4 col-lg-3 mt-3" key={index}>
                    <div className="card p-3 border border-2 rounded-3" style={{ height: '100%' }} onClick={() => handleCardClick(page)}>
                        <img className="card-img-top" src={imagePath} alt="Page image" style={{ height: '150px', objectFit: 'cover' }} />
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
        <>
            {user ? (
                <div className="text-center mt-3">
                    <h1>Bem-Vindo, {user.nome} ao HowToMaster</h1>
                </div>
            ) : (
                <div className="text-center mt-3">
                    <h1>Bem-Vindo ao HowToMaster</h1>
                </div>
            )}

            <div className="container-fluid">
                <div className="row">
                    {renderPages()}
                </div>
            </div>
        </>
    );
}

export default HomePage;
