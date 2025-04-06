import {FC} from "react";
import {Route, Routes} from "react-router-dom";
import LoginPage from "./pages/LoginPage.tsx";
import ProtectedRoute from "./features/auth/components/ProtectedRoute.tsx";
import ChatPage from "./pages/ChatPage/ChatPage.tsx";
import RegisterPage from "./pages/RegisterPage.tsx";
import {ToastContainer} from "react-toastify";

const App: FC = () => {

    return (
        <>
            <Routes>
                <Route element={<ProtectedRoute/>}>
                    <Route path="/" element={<ChatPage/>}/>
                </Route>

                <Route path="/login" element={<LoginPage />} />
                <Route path="/register" element={<RegisterPage/>}/>
            </Routes>

            <ToastContainer
                position="top-right"
                autoClose={3000}
                newestOnTop
                closeOnClick
                rtl={false}
                pauseOnFocusLoss
                draggable
                pauseOnHover
                theme="colored"
            />
        </>
    )
}

export default App;
