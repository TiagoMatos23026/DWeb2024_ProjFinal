import logo from './logo.svg';
import './App.css';
import Layout from './App/html/utils/Layout';
import HomePage from './App/html/HomePage';
import ViewPage from './App/html/ViewPage';
import SearchPage from './App/html/SearchPage';
import LoginPage from './App/html/LoginPage';
import CreatePage from './App/html/CreatePage';

import { createContext, useEffect, useState, useContext } from 'react'
import {
    BrowserRouter as Router,
    Routes,
    Route,
    redirect
  } from 'react-router-dom';
import RegisterPage from './App/html/RegisterPage';
import ProfilePage from './App/html/ProfilePage';

export const AuthContext = createContext({ });
export const useAuth = () => useContext(AuthContext);

function App() {

    const [user, setUser] = useState(null);

    return (
        <AuthContext.Provider value={{user, setUser}} >
            <Router>
                <Routes>
                    <Route className="container" path="/" element={<Layout/>}>
                        <Route className="container" path="/HomePage" element={<HomePage />} />
                        <Route className="container" path="/ViewPage" element={<ViewPage />} />
                        <Route className="container" path="/SearchPage" element={<SearchPage />} />
                        <Route className="container" path="/LoginPage" element={<LoginPage />} />
                        <Route className="container" path="/RegisterPage" element={<RegisterPage />} />
                        <Route className="container" path="/ProfilePage" element={<ProfilePage />} />
                        <Route className="container" path="/CreatePage" element={<CreatePage />} />
                    </Route>
                </Routes>
            </Router>
        </AuthContext.Provider>
    );
}



export default App;