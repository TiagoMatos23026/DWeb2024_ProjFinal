import { useLocation } from "react-router-dom";
import { Link } from "react-router-dom";

function ViewPage() {
    const location = useLocation();
    const { page, autor } = location.state;
    const baseUrl = "http://localhost:5101/imagens/";

    return (
        <div className="d-flex justify-content-center align-items-center">
          <div className="row">
            <h1 className="text-center"></h1>

            <div className="container text-center">
              <img src={`${baseUrl}${page.thumbnail}`} alt="Thumbnail" className="img-center" />
            </div>
  
            <div className="row mt-3">
              <div className="">
                <h1 className="text-center">{page.name}</h1>
                <p className="text-center">Escrito Por: {autor.nome}</p>
  
                <h2 className="text-center">Descrição</h2>
                <p className="text-center">{page.descricao}</p>
  
                <h2 className="text-center">Tutorial</h2>
                <p className="text-center">{page.conteudo}</p>
  
                <div className="text-center mb-3">
                  <Link className="btn btn-warning" to="/HomePage">Voltar</Link>
                </div>
  
              </div>
            </div>
  
          </div>
        </div >
      );
}

export default ViewPage;
