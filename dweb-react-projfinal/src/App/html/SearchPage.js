// SearchPage.js

import React from "react";
import 'bootstrap/dist/css/bootstrap.css';

function SearchPage({ searchResults }) {
    const renderSearchResults = () => {
        return searchResults.map((page, index) => (
            <div className="col-lg-6 col-md-6 col-sm-12 col-xs-12 mb-4" key={index}>
                <div className="card border border-2 rounded-3 h-100">
                    <img className="card-img-top" src="https://picsum.photos/id/1/200/300" alt="Page image" style={{ height: '150px', objectFit: 'cover' }} />
                    <div className="card-body">
                        <h5 className="card-title">{page.name}</h5>
                        <h6 className="card-subtitle mb-2 text-muted">Author: {page.author}</h6>
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
