import logo from './logo.svg';
import './App.css';
import Layout from './App/html/utils/Layout';
import HomePage from './App/html/HomePage';
import ViewPage from './App/html/ViewPage';
import SearchPage from './App/html/SearchPage';
import LoginPage from './App/html/LoginPage';

import { createContext, useEffect, useState } from 'react'
import {
    BrowserRouter as Router,
    Routes,
    Route,
  } from 'react-router-dom';
import RegisterPage from './App/html/RegisterPage';

var contextInterface = {
    context: { user: "Tiago", themeIsLight: false },
    setContext: () => { }
}

export const AppContext = createContext({ ...contextInterface });


function App() {

    const [ctx, setCtx] = useState({ ...contextInterface.context });

    return (
        <AppContext.Provider value={{ context: ctx, setContext: setCtx }} >
            <Router>
                <Routes>
                    <Route className="container" path="/" element={<Layout />}>
                        <Route className="container" path="HomePage" element={<HomePage />} />
                        <Route className="container" path="ViewPage" element={<ViewPage />} />
                        <Route className="container" path="SearchPage" element={<SearchPage />} />
                        <Route className="container" path="LoginPage" element={<LoginPage />} />
                        <Route className="container" path="RegisterPage" element={<RegisterPage />} />
                    </Route>
                </Routes>
            </Router>
        </AppContext.Provider>
    );
}

export default App;
