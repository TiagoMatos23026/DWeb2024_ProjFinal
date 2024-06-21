import React, { useEffect, useState } from 'react';
import 'bootstrap/dist/css/bootstrap.min.css';
import { render } from '@testing-library/react';
import { getPagesAPI, getUtentesAPI } from '../api/apiConnection';

const CreatePage = () => {
    const [ utentesList, setUtentesList ] = useState('');
    const [ pagesList, setPagesList ] = useState('');
    const email = sessionStorage.getItem('userLogged');

    const [formData, setFormData] = useState({
        name: '',
        descricao: '',
        conteudo: '',
        dificuldade: '',
        thumbnail: '',
        utenteFK: '',
    });

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

    const handleChange = (e) => {
        const { name, value, type, files } = e.target;
        setFormData({
            ...formData,
            [name]: type === 'file' ? files[0] : value
        });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        const userLogged = utentesList.find(utente => utente.email === email);

        const submitData = new FormData();
        submitData.append('name', formData.name);
        submitData.append('descricao', formData.descricao);
        submitData.append('conteudo', formData.conteudo);
        submitData.append('dificuldade', formData.dificuldade);
        submitData.append('thumbnail', formData.thumbnail);
        submitData.append('utenteFK', userLogged.id);

        try {
            const response = await fetch('http://localhost:5101/api/PaginasAPI', {
                method: 'POST',
                body: submitData
            });
            const responseData = await response.json();
            console.log('Response:', responseData);
            alert('Página Criada com Sucesso!');
            window.location.href="/HomePage";

        } catch (error) {
            console.error('Error submitting form:', error);
        }

    };

    useEffect(() => {
        fetchData();
    }, []);

    const renderPages = () => {
        return (
            <div className="d-flex justify-content-center align-items-center mt-3 bg-white">
                <div className="bg-dark text-white p-4 rounded-3 shadow">
                    <h2 className="text-center">Criar Nova Página</h2>
                    <form onSubmit={handleSubmit}>
                        <div className="mb-2">
                            <label htmlFor="nome" className="form-label">Título:</label>
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
                            <label htmlFor="icon" className="form-label">Thumbnail:</label>
                            <input
                                type="file"
                                id="thumbnail"
                                name="thumbnail"
                                className="form-control"
                                onChange={handleChange}
                            />
                        </div>
                        <div className="mb-2">
                            <label htmlFor="email" className="form-label">Descrição</label>
                            <input
                                type="text"
                                id="descricao"
                                name="descricao"
                                className="form-control"
                                value={formData.descricao}
                                onChange={handleChange}
                                required
                            />
                        </div>
                        <div className="mb-2">
                            <label htmlFor="telemovel" className="form-label">Conteúdo:</label>
                            <input
                                type="text"
                                id="conteudo"
                                name="conteudo"
                                className="form-control"
                                value={formData.conteudo}
                                onChange={handleChange}
                                required
                            />
                        </div>
                        <div className="mb-2">
                            <label htmlFor="password" className="form-label">Dificuldade:</label>
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

                        <button type="submit" className="btn btn-primary">Criar</button>
                    </form>
                </div>
            </div>
        );
    };

    return (
        <div>
            {renderPages()};
        </div>
    );



}

export default CreatePage;
