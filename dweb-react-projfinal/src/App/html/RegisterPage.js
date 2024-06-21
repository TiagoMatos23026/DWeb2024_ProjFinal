import React, { useState } from 'react';
import 'bootstrap/dist/css/bootstrap.min.css';

const RegisterPage = () => {
  const [formData, setFormData] = useState({
    nome: '',
    icon: '',
    email: '',
    telemovel: '',
    password: '',
    confirmPassword: '',
    dataNasc: '',
    biografia: ''
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

    const submitData = new FormData();
    submitData.append('nome', formData.nome);
    submitData.append('icon', formData.icon);
    submitData.append('email', formData.email);
    submitData.append('telemovel', formData.telemovel);
    submitData.append('password', formData.password);
    submitData.append('dataNasc', formData.dataNasc);
    submitData.append('biografia', formData.biografia);

    const loginData = {
      email: formData.email,
      password: formData.password
    };

    try {
      const response = await fetch('http://localhost:5101/api/UtentesAPI', {
        method: 'POST',
        body: submitData
      });

      if (response.ok) {
        const responseData = await response.json();

        try {
          const response = await fetch('http://localhost:5101/api/LoginUtilizadorAPI', {
            method: 'POST',
            headers: {
              'Content-Type': 'application/json'
            },
            body: JSON.stringify(loginData)
          });

          if (response.ok) {
            const responseData = await response.json();
            console.log('Response:', responseData);
            alert('Conta Criada com Sucesso!')
            window.location.href="/HomePage"
          } else {
            console.error('Error submitting form:', response.statusText);
          }
        } catch (error) {
          console.error('Error submitting form:', error);
        }

        console.log('Response:', responseData);
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
            <label htmlFor="nome" className="form-label">Nome:</label>
            <input
              type="text"
              id="nome"
              name="nome"
              className="form-control"
              value={formData.nome}
              onChange={handleChange}
              required
            />
          </div>
          <div className="mb-2">
            <label htmlFor="icon" className="form-label">Icon:</label>
            <input
              type="file"
              id="icon"
              name="icon"
              className="form-control"
              onChange={handleChange}
            />
          </div>
          <div className="mb-2">
            <label htmlFor="email" className="form-label">Email:</label>
            <input
              type="email"
              id="email"
              name="email"
              className="form-control"
              value={formData.email}
              onChange={handleChange}
              required
            />
          </div>
          <div className="mb-2">
            <label htmlFor="telemovel" className="form-label">Telemóvel:</label>
            <input
              type="text"
              id="telemovel"
              name="telemovel"
              className="form-control"
              value={formData.telemovel}
              onChange={handleChange}
              required
            />
          </div>
          <div className="mb-2">
            <label htmlFor="password" className="form-label">Password:</label>
            <input
              type="password"
              id="password"
              name="password"
              className="form-control"
              value={formData.password}
              onChange={handleChange}
              required
            />
          </div>
          <div className="mb-2">
            <label htmlFor="confirmPassword" className="form-label">Confirmar Password:</label>
            <input
              type="password"
              id="confirmPassword"
              name="confirmPassword"
              className="form-control"
              value={formData.confirmPassword}
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
            <label htmlFor="biografia" className="form-label">Biografia:</label>
            <textarea
              id="biografia"
              name="biografia"
              className="form-control"
              value={formData.biografia}
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
