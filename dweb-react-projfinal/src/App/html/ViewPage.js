import { useLocation } from "react-router-dom";
import { Link } from "react-router-dom";

function ViewPage() {
    const location = useLocation();
    const { page, autor } = location.state;

    return (
        <div className="d-flex justify-content-center align-items-center">
          <div className="row">
            <h1 className="text-center"></h1>
            <div className="container text-center">
              <img src="https://picsum.photos/id/1/200/300" alt="Thumbnail" className="img-center" />
              <p className="text-center">Escrito Por: {autor.nome}</p>
            </div>
  
            <div className="row mt-3">
              <div className="">
                <h1 className="text-center">{page.name}</h1>
  
                <h2 className="text-center">Introdução</h2>
                <p className="text-center">{page.conteudo}</p>
  
                <h2 className="text-center">Tutorial</h2>
                <p className="text-center"></p>
  
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
