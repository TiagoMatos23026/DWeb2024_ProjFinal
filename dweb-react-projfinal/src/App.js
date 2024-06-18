import logo from './logo.svg';
import './App.css';
import Layout from './App/html/Layout';
import HomePage from './App/html/HomePage';
import ViewPage from './App/html/ViewPage';
import SearchPage from './App/html/SearchPage';

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
                        <Route className="container" path="HomePage" element={<HomePage />} />
                        <Route path="ViewPage" element={<ViewPage />} />
                        <Route path="SearchPage" element={<SearchPage />} />
                    </Route>
                </Routes>
            </Router>
        </AppContext.Provider>
    );
}

export default App;
