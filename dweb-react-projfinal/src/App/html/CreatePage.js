import React, { useState } from 'react';
import 'bootstrap/dist/css/bootstrap.min.css';
import axios from 'axios';
import { useAuth } from '../../App';
import { useNavigate } from 'react-router-dom';

function CreatePage() {
    const navigate = useNavigate();
    const { user } = useAuth();

    const [formData, setFormData] = useState({
        name: '',
        descricao: '',
        conteudo: '',
        dificuldade: '',  
        ImgThumbnail: null,
    });

    const handleChange = (e) => {
        const { name, value, type, files } = e.target;
        setFormData({
            ...formData,
            [name]: type === 'file' ? files[0] : value
        });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        // Create a FormData object to handle file uploads
        const submitData = new FormData();
        submitData.append('Name', formData.name);
        submitData.append('Descricao', formData.descricao);
        submitData.append('Conteudo', formData.conteudo);
        submitData.append('Dificuldade', formData.dificuldade);
        submitData.append('ImgThumbnail', formData.ImgThumbnail);
        submitData.append('UtenteFK', user.id); // Assuming 'user.id' is available from context

        try {
            // Make POST request to the API using axios
            const response = await axios.post('https://dwebprojfinalhowtomasterapp.azurewebsites.net/api/PaginasAPI', submitData, {
                withCredentials: true, // Include cookies (credentials)
                headers: {
                    'Content-Type': 'multipart/form-data', // Set the correct content type for file upload
                }
            });

            const responseData = response.data;
            console.log('Response:', responseData);
            alert('Página Criada com Sucesso!');
            navigate("/HomePage");

        } catch (error) {
            console.error('Error submitting form:', error);
            alert('Houve um problema ao criar a página.');
        }
    };

    return (
        <div className="d-flex justify-content-center align-items-center mt-3 bg-white">
            <div className="bg-dark text-white p-4 rounded-3 shadow">
                <h2 className="text-center">Criar Nova Página</h2>
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
                        <label htmlFor="descricao" className="form-label">Descrição</label>
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
                        <label htmlFor="conteudo" className="form-label">Conteúdo:</label>
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
                    <button type="submit" className="btn btn-primary">Criar</button>
                </form>
            </div>
        </div>
    );
}

export default CreatePage;
