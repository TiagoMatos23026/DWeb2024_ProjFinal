import React, { useState } from 'react';
import { useAuth } from '../../App';
import 'bootstrap/dist/css/bootstrap.min.css';

const LoginPage = () => {

  const LoginUtilizadorAPI = async () => {
    const response = await fetch('http://localhost:5101/api/LoginUtilizadorAPI');
    const data = await response.json();
    return data;
  };

  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');
  //const { user, setUser } = useAuth();

  const handleSubmit = async (e) => {
    e.preventDefault();
    const users = await LoginUtilizadorAPI();
    const userAPI = users.find(userAPI => userAPI.email === email);
    const userEmail = userAPI.email;

    if (userAPI && userAPI.password === password) {
      //setUser(userAPI);  
      sessionStorage.setItem('userLogged', userEmail )
      alert('Login successful!');
      window.location.href="/HomePage";
    } else {
      setError('Invalid email or password');
    }
  };

  return (
    <div className="d-flex justify-content-center align-items-center mt-5 bg-white">
      <div className="bg-dark text-white p-4 rounded-3 shadow">
        <h2 className="text-center">Login</h2>
        <form onSubmit={handleSubmit}>
          <div className="mb-5">
            <label htmlFor="email" className="form-label">Email:</label>
            <input 
              type="email" 
              id="email" 
              name="email" 
              className="form-control" 
              value={email} 
              onChange={(e) => setEmail(e.target.value)} 
              required 
            />
          </div>
          <div className="mb-5">
            <label htmlFor="password" className="form-label">Password:</label>
            <input 
              type="password" 
              id="password" 
              name="password" 
              className="form-control" 
              value={password} 
              onChange={(e) => setPassword(e.target.value)} 
              required 
            />
          </div>
          {error && <div className="alert alert-danger">{error}</div>}
          <div className="text-center">
            <button type="submit" className="btn btn-primary">Login</button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default LoginPage;
