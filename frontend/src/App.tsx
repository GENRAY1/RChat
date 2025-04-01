import {FC} from "react";
import {Route, Routes} from "react-router-dom";
import LoginPage from "./pages/LoginPage.tsx";
import ProtectedRoute from "./auth/components/ProtectedRoute.tsx";
import MainPage from "./pages/MainPage/MainPage.tsx";
import RegisterPage from "./pages/RegisterPage.tsx";

const App: FC = () => {

    return (
        <>
            <Routes>
                <Route element={<ProtectedRoute/>}>
                    <Route path="/" element={<MainPage/>}/>
                </Route>

                <Route path="/login" element={<LoginPage />} />
                <Route path="/register" element={<RegisterPage/>}/>
            </Routes>
        </>
    )
}

export default App;
