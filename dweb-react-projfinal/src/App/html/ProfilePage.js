import React from 'react';
import { useState, useEffect } from 'react';
import { AuthContext, useAuth } from '../../App';
import { getUtentesDetailsAPI, getPagesAPI } from '../api/apiConnection';


const ProfilePage = () => {

    const { pages, setPagesList } = useState('');
    const { utente, setUtente } = useState('');

    //const { user, setUser } = useAuth();

    const fetchData = async () => {

        try {
            const response = await fetch('http://localhost:5101/api/LoginUtilizadorAPI', {
              method: 'POST',
              headers: {
                'Content-Type': 'application/json'
              },
              body: JSON.stringify(loginData)
            });
  
            if (response.ok) {
              const responseData = await response.json();
              console.log('Response:', responseData);
            } else {
              console.error('Error submitting form:', response.statusText);
            }
          } catch (error) {
            console.error('Error submitting form:', error);
          }




        try {
            const [pagesResponse, utentesResponse] = await Promise.all([getPagesAPI(), getUtentesDetailsAPI(sessionStorage(user))]);
            const pages = await pagesResponse.json();
            const utenteData = await utentesResponse.json();


            setPagesList(pages);
            setUtente(utenteData);
        } catch (error) {
            console.error('Erro', error);
        }
    };

    useEffect(() => {
        fetchData();
    }, []);

    return (
        <div className="container mt-5">
            <div className="row">
                <div className="col-md-3">
                    <div className="card">
                        <img
                            src="https://via.placeholder.com/150"
                            className="card-img-top"
                            alt="Profile"
                        />
                        <div className="card-body text-center">
                            <h5 className="card-title">{utente.nome}</h5>
                            <p className="card-text">Content Creator</p>
                            <button className="btn btn-primary btn-block">Subscribe</button>
                        </div>
                    </div>
                </div>
                <div className="col-md-9">
                    <div className="card mb-3">
                        <div className="card-body">
                            <h5 className="card-title">Biografia</h5>
                            <p className="card-text">
                                {user.biografia}
                            </p>
                        </div>
                    </div>
                    <div className="card mb-3">
                        <div className="card-body">
                            <h5 className="card-title">Videos</h5>
                            <div className="row">
                                {Array.from({ length: 6 }).map((_, idx) => (
                                    <div className="col-md-4 mb-3" key={idx}>
                                        <div className="card">
                                            <img
                                                src={`https://via.placeholder.com/200?text=Video+${idx + 1}`}
                                                className="card-img-top"
                                                alt={`Video ${idx + 1}`}
                                            />
                                            <div className="card-body">
                                                <h6 className="card-title">Video Title {idx + 1}</h6>
                                                <p className="card-text">Some description of the video content.</p>
                                            </div>
                                        </div>
                                    </div>
                                ))}
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default ProfilePage;
