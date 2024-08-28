import React from 'react'
import ReactDOM from 'react-dom/client'
import App from './App.jsx'
import './index.css'
import CustomAxiosProvider from './services/customAxiosProvider';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import { AuthProvider } from './context/AuthProvider';

//import { disableReactDevTools } from '@fvilers/disable-react-devtools';

/*if (process.env.REACT_APP_ENVIRONMENT === 'production') {
    disableReactDevTools();
} */

ReactDOM.createRoot(document.getElementById('root')).render(
    <React.StrictMode>
        <Router>
            <AuthProvider>
                <CustomAxiosProvider>
                    <Routes>
                        <Route path="/*" element={<App />} />
                    </Routes>
                </CustomAxiosProvider>
            </AuthProvider>
        </Router>
    </React.StrictMode>,
)