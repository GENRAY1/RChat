import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import { BrowserRouter as Router } from 'react-router-dom';
import './index.css'
import './fonts.tsx'
import App from './App.tsx'
import AuthProvider from "./auth/components/AuthProvider.tsx";

createRoot(document.getElementById('root')!).render(
    <StrictMode>
      <AuthProvider>
          <Router>
              <App />
          </Router>
      </AuthProvider>
    </StrictMode>
)
