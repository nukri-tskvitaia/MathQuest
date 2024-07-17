import React from 'react';
import ReactDOM from 'react-dom';
import App from './App';

//import { disableReactDevTools } from '@fvilers/disable-react-devtools';

/*if (process.env.REACT_APP_ENVIRONMENT === 'production') {
    disableReactDevTools();
} */

const root = ReactDOM.createRoot(document.getElementById('root')); // Create a root

root.render(
    <React.StrictMode>
        < App />
    </React.StrictMode>
);