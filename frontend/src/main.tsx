import { createRoot } from 'react-dom/client'
import { BrowserRouter as Router } from 'react-router-dom';
import './fonts.tsx'
import './index.css'
import App from './App.tsx'
import AuthProvider from "./features/auth/components/AuthProvider.tsx";
import {StrictMode} from "react";

createRoot(document.getElementById('root')!).render(
    <StrictMode>
      <AuthProvider>
          <Router>
              <App />
          </Router>
      </AuthProvider>
    </StrictMode>
)
