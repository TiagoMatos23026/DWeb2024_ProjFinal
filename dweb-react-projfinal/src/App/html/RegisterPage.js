import React, { useState } from 'react';
import 'bootstrap/dist/css/bootstrap.min.css';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';

const RegisterPage = () => {

  const navigate = useNavigate();

  const [formData, setFormData] = useState({
    Nome: '',
    Telemovel: '',
    dataNasc: '',
    Biografia: '',
    Email: '',
    Password: '',
    ConfirmPassword: '',
    IconFile: null,
  });

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

    submitData.append('Nome', formData.Nome);
    submitData.append('Telemovel', formData.Telemovel);
    submitData.append('dataNasc', formData.dataNasc);
    submitData.append('biografia', formData.Biografia);
    submitData.append('Email', formData.Email);
    submitData.append('Password', formData.Password);
    submitData.append('ConfirmPassword', formData.ConfirmPassword);
    submitData.append('IconFile', formData.IconFile);

    try {
      const response = await axios.post('https://dwebprojfinalhowtomasterapp.azurewebsites.net/api/AccountAPI/register', submitData, {
        headers: {
          'Content-Type': 'multipart/form-data',
        },
      });

      if (response.status === 200) {
        console.log('User registered successfully');
        alert('Registo efetuado com sucesso!')

        navigate('/HomePage')
      } else {
        console.error('Error submitting form:', response.statusText);
      }
    } catch (error) {
      console.error('Error submitting form:', error);
    }
  };

  return (
    <div className="d-flex justify-content-center align-items-center mt-3 bg-white">
      <div className="bg-dark text-white p-4 rounded-3 shadow">
        <h2 className="text-center">Registar</h2>
        <form onSubmit={handleSubmit}>
          <div className="mb-2">
            <label htmlFor="Nome" className="form-label">Nome:</label>
            <input
              type="text"
              id="Nome"
              name="Nome"
              className="form-control"
              value={formData.Nome}
              onChange={handleChange}
              required
            />
          </div>
          <div className="mb-2">
            <label htmlFor="IconFile" className="form-label">Icon:</label>
            <input
              type="file"
              id="IconFile"
              name="IconFile"
              className="form-control"
              onChange={handleChange}
              required
            />
          </div>
          <div className="mb-2">
            <label htmlFor="Email" className="form-label">Email:</label>
            <input
              type="email"
              id="Email"
              name="Email"
              className="form-control"
              value={formData.Email}
              onChange={handleChange}
              required
            />
          </div>
          <div className="mb-2">
            <label htmlFor="Telemovel" className="form-label">Telemóvel:</label>
            <input
              type="text"
              id="Telemovel"
              name="Telemovel"
              className="form-control"
              value={formData.Telemovel}
              onChange={handleChange}
              required
            />
          </div>
          <div className="mb-2">
            <label htmlFor="Password" className="form-label">Password:</label>
            <input
              type="password"
              id="Password"
              name="Password"
              className="form-control"
              value={formData.Password}
              onChange={handleChange}
              required
            />
          </div>
          <div className="mb-2">
            <label htmlFor="ConfirmPassword" className="form-label">Confirmar Password:</label>
            <input
              type="password"
              id="ConfirmPassword"
              name="ConfirmPassword"
              className="form-control"
              value={formData.ConfirmPassword}
              onChange={handleChange}
              required
            />
          </div>
          <div className="mb-2">
            <label htmlFor="dataNasc" className="form-label">Data de Nascimento (dd-mm-aaaa):</label>
            <input
              type="text"
              id="dataNasc"
              name="dataNasc"
              className="form-control"
              value={formData.dataNasc}
              onChange={handleChange}
              required
            />
          </div>
          <div className="mb-2">
            <label htmlFor="Biografia" className="form-label">Biografia:</label>
            <textarea
              id="Biografia"
              name="Biografia"
              className="form-control"
              value={formData.Biografia}
              onChange={handleChange}
            />
          </div>
          <button type="submit" className="btn btn-primary">Registar</button>
        </form>
      </div>
    </div>
  );
}

export default RegisterPage;
