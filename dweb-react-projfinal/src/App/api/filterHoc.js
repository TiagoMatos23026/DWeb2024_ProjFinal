import { useContext } from "react";
import { AppContext } from "../App";
import { useLocation, useNavigate } from "react-router-dom";

export default function FilterHoc({children}){
    const ctx = useContext(AppContext);
    const navigate = useNavigate();
    const location = useLocation();

    if(ctx.context.jwtToken=="" && location.pathname.includes("todo")){
        return setTimeout(()=>navigate("/login"), 100);
    }

    return <>{children}</>;
}