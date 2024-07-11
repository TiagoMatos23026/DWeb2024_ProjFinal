import React, { useState } from 'react';
import { useAuth } from '../../App';
import 'bootstrap/dist/css/bootstrap.min.css';
import { useNavigate } from 'react-router-dom';
import { postLogin } from '../api/apiConnection';

function LoginPage() {

  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');

  const { user, setUser } = useAuth();
  const navigate = useNavigate();

  const handleLogin = async (email, password) => {
    const formData = new FormData();
    formData.append('email', email);
    formData.append('password', password);
    return postLogin(formData);
  }

  const handleSubmit = async (e) => {
    e.preventDefault();
    let res = await handleLogin(email, password);
    setUser(res);
    alert("Login efetuado com sucesso!");
    navigate("/HomePage");
  };

  return (
    <>
      <div className="d-flex justify-content-center align-items-center mt-5 bg-white">
        <div className="bg-dark text-white p-4 rounded-3 shadow">
          <h2 className="text-center">Login</h2>
          <form onSubmit={handleSubmit}>

            <div className="mb-5">
              <label htmlFor="email" className="form-label">Email:</label>
              <input type="email" id="email" name="email" className="form-control" value={email} onChange={(e) => setEmail(e.target.value)} required />
            </div>

            <div className="mb-5">
              <label htmlFor="password" className="form-label">Password:</label>
              <input type="password" id="password" name="password" className="form-control" value={password} onChange={(e) => setPassword(e.target.value)} required />
            </div>

            {error && <div className="alert alert-danger">{error}</div>}

            <div className="text-center">
              <button type="submit" className="btn btn-primary">Login</button>
            </div>

          </form>
        </div>
      </div>
    </>
  );
};

export default LoginPage;
