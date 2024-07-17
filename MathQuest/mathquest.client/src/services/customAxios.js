import axios from 'axios';

// custom Axios instance factory
const customAxios = axios.create({
    baseURL: 'https://localhost:5001',
    headers: {
        'Content-Type': 'application/json' // Default header for all requests
    },
    withCredentials: true // Ensure cookies are sent with requests by default
});

export default customAxios;