import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { useNavigate, useLocation } from 'react-router-dom';
import 'bootstrap/dist/css/bootstrap.min.css';
import { useAuth } from '../../App';

// Set up axios to include credentials by default
axios.defaults.withCredentials = true;

const EditPaginaPage = () => {
    const location = useLocation();
    const { page } = location.state;

    const { user } = useAuth();
    const [formData, setFormData] = useState({
        name: page.name || '',
        descricao: page.descricao || '',
        conteudo: page.conteudo || '',
        dificuldade: page.dificuldade || '',
        ImgThumbnail: null,
    });

    const navigate = useNavigate();

    useEffect(() => {
        const fetchPageData = async () => {
            try {
                const response = await axios.get(`http://localhost:5101/api/PaginasAPI/${page.id}`, {
                    withCredentials: true, // Include credentials with the request
                });
                const { name, descricao, conteudo, dificuldade } = response.data;
                setFormData({ name, descricao, conteudo, dificuldade });
            } catch (error) {
                console.error('Error fetching page data:', error);
            }
        };

        fetchPageData();
    }, [page.id]);

    const handleChange = (e) => {
        const { name, value, type, files } = e.target;
        setFormData({
            ...formData,
            [name]: type === 'file' ? files[0] : value,
        });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        const submitData = new FormData();
        submitData.append('name', formData.name);
        submitData.append('descricao', formData.descricao);
        submitData.append('conteudo', formData.conteudo);
        submitData.append('dificuldade', formData.dificuldade);
        submitData.append('ImgThumbnail', formData.ImgThumbnail);

        submitData.append('utenteFK', user.id);

        try {
            const response = await axios.put(`http://localhost:5101/api/PaginasAPI/${page.id}`, submitData, {
                headers: {
                    'Content-Type': 'multipart/form-data',
                },
                withCredentials: true, // Include credentials with the request
            });

            if (response.status === 200) {
                alert('Página editada com sucesso!');
                navigate('/ProfilePage');
            } else {
                console.error('Erro ao editar página:', response.statusText);
            }
        } catch (error) {
            console.error('Erro ao editar página:', error);
        }
    };

    return (
        <div className="d-flex justify-content-center align-items-center mt-3 bg-white" style={{ minHeight: '80vh' }}>
            <div className="bg-dark text-white p-5 rounded-3 shadow" style={{ width: '600px' }}>
                <h2 className="text-center">Editar Página</h2>
                <form onSubmit={handleSubmit}>
                    <div className="mb-2">
                        <label htmlFor="name" className="form-label">Título:</label>
                        <input
                            type="text"
                            id="name"
                            name="name"
                            className="form-control"
                            value={formData.name}
                            onChange={handleChange}
                            required
                        />
                    </div>
                    <div className="mb-2">
                        <label htmlFor="thumbnail" className="form-label">Thumbnail:</label>
                        <input
                            type="file"
                            id="thumbnail"
                            name="ImgThumbnail"
                            className="form-control"
                            onChange={handleChange}
                        />
                    </div>
                    <div className="mb-2">
                        <label htmlFor="descricao" className="form-label">Descrição:</label>
                        <textarea
                            type="text"
                            id="descricao"
                            name="descricao"
                            className="form-control"
                            rows="3"
                            value={formData.descricao}
                            onChange={handleChange}
                            required
                        />
                    </div>
                    <div className="mb-2">
                        <label htmlFor="conteudo" className="form-label">Conteúdo:</label>
                        <textarea
                            type="text"
                            id="conteudo"
                            name="conteudo"
                            className="form-control"
                            rows="3"
                            value={formData.conteudo}
                            onChange={handleChange}
                            required
                        />
                    </div>
                    <div className="mb-2">
                        <label htmlFor="dificuldade" className="form-label">Dificuldade:</label>
                        <input
                            type="number"
                            id="dificuldade"
                            name="dificuldade"
                            className="form-control"
                            value={formData.dificuldade}
                            onChange={handleChange}
                            required
                        />
                    </div>
                    <div className="d-flex justify-content-between">
                        <button type="submit" className="btn btn-primary">Guardar</button>
                        <button type="button" className="btn btn-warning" onClick={() => navigate('/HomePage')}>Cancelar</button>
                    </div>
                </form>
            </div>
        </div>
    );
};

export default EditPaginaPage;
