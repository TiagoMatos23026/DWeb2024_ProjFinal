import React, { useState } from 'react';
import { useAuth } from '../../App';
import 'bootstrap/dist/css/bootstrap.min.css';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';

function LoginPage() {

  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [rememberMe, setRememberMe] = useState(false);
  const [error, setError] = useState('');

  const { user, setUser } = useAuth();
  const navigate = useNavigate();

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      console.log('Sending login request...');
      const response = await axios.post('http://localhost:5101/api/AccountAPI/login', {
        email,
        password,
        rememberMe
      }, {
        withCredentials: true // This will include cookies in the request
      });

      console.log('Login response:', response);

      if (response.status === 200) {
        const email = response.data;
        console.log('Login successful, fetching user details for email:', email);

        // Fetch user details after successful login
        const utenteResponse = await axios.get(`http://localhost:5101/api/UtentesAPI/email/${email}`, {
          withCredentials: true // Include credentials (cookies)
        });

        console.log('User details response:', utenteResponse);

        if (utenteResponse.status !== 200) {
          throw new Error('Failed to fetch user details');
        }

        const utente = utenteResponse.data;
        console.log('Fetched user details:', utente);
        await setUser(utente);
        alert('Login efetuado com sucesso!');
        navigate("/HomePage"); // Redirect to a protected page
      }
    } catch (err) {
      console.error('Error during login or fetching user details:', err);
      setError('Invalid login attempt');
      alert('Verifique as suas credenciais.');
    }
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

            <div className="mb-5">
              <label>
                <input
                  type="checkbox"
                  checked={rememberMe}
                  onChange={(e) => setRememberMe(e.target.checked)}
                />
                Remember me
              </label>
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
