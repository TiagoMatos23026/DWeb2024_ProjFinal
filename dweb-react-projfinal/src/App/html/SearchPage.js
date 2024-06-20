// SearchPage.js

import React from "react";
import 'bootstrap/dist/css/bootstrap.css';

function SearchPage({ searchResults }) {
    const baseUrl = "http://localhost:5101/imagens/";
    const renderSearchResults = () => {
        return searchResults.map((page, index) => (
            <div className="col-12 col-sm-6 col-md-4 col-lg-3 mt-3" key={index}>
                <div className="card p-3 border border-2 rounded-3" style={{ height: '100%' }}>
                    <img className="card-img-top" src={`${baseUrl}${page.thumbnail}`} alt="Page image" style={{ height: '150px', objectFit: 'cover' }} />
                    <div className="card-body">
                        <h5 className="card-title">{page.name}</h5>
                        <h6 className="card-subtitle mb-2 text-muted">Autor: {page.autor}</h6>
                    </div>
                </div>
            </div>
        ));
    };

    return (
        <div className="container-fluid">
            <div className="row">
                {renderSearchResults()}
            </div>
        </div>
    );
}

export default SearchPage;
