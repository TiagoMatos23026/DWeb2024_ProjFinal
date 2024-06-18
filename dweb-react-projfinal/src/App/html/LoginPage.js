import React from 'react';
import 'bootstrap/dist/css/bootstrap.min.css';

const LoginPage = () => {
  return (
    <div className="d-flex justify-content-center align-items-center mt-5 bg-white">
      <div className="bg-dark text-white p-4 rounded-3 shadow">
        <h2 className="text-center">Login</h2>
        <form>
          <div className="mb-5">
            <label htmlFor="email" className="form-label">Email:</label>
            <input type="email" id="email" name="email" className="form-control" />
          </div>
          <div className="mb-5">
            <label htmlFor="password" className="form-label">Password:</label>
            <input type="password" id="password" name="password" className="form-control" />
          </div>
        </form>
      </div>
    </div>
  );
}

export default LoginPage;
