import { useLocation } from "react-router-dom";
import { Link } from "react-router-dom";
import 'bootstrap/dist/css/bootstrap.min.css';
import 'bootstrap/dist/css/bootstrap.css';

function ViewPage() {
    const location = useLocation();
    const { page, autor } = location.state;
    const baseUrl = "http://localhost:5101/imagens/";

    return (
        <div className="d-flex justify-content-center align-items-center vh-100">
            <div className="card rounded-3 shadow" style={{ maxWidth: '800px', width: '100%' }}>
                <img src={`${baseUrl}${page.thumbnail}`} alt="Thumbnail" className="card-img-top rounded-top" style={{ maxHeight: '400px', objectFit: 'cover' }} />
                <div className="card-body">
                    <h1 className="card-title text-center">{page.name}</h1>
                    <p className="card-text text-center"><strong>Escrito Por:</strong> {autor.nome}</p>
                    <hr />
                    <h2 className="card-title text-center">Descrição</h2>
                    <p className="card-text text-center">{page.descricao}</p>
                    <hr />
                    <h2 className="card-title text-center">Tutorial</h2>
                    <p className="card-text text-center">{page.conteudo}</p>
                    <div className="text-center mt-4">
                        <Link className="btn btn-warning" to="/HomePage">Voltar</Link>
                    </div>
                </div>
            </div>
        </div>
    );
}

export default ViewPage;
