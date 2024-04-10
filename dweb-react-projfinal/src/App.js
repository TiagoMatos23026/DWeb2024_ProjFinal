import logo from './logo.svg';
import './App.css';
import Layout from './App/html/Layout';
import HomePage from './App/html/HomePage';

import { createContext, useEffect, useState } from 'react'
import {
    BrowserRouter as Router,
    Routes,
    Route,
  } from 'react-router-dom';

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
                        <Route className="container" path="Home" element={<HomePage />} />
                    </Route>
                </Routes>
            </Router>
        </AppContext.Provider>
    );
}

export default App;
