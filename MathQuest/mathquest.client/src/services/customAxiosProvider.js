import { useEffect } from 'react';
import customAxios from './customAxios';
import { useAuth } from '../context/useAuth';
import RefreshToken from './RefreshToken';

const CustomAxiosProvider = ({ children }) => {
    const { setIsAuthenticated, setRoles } = useAuth();
    // useEffect to setup axios only once
    useEffect(() => {
        console.log("Checking response status");
        const responseIntercept = customAxios.interceptors.response.use(
            response => response, // if response is good just return it
            async error => {
                const originalRequest = error.config;
                console.log("Backend returned error ");
                if (error.response.status === 401 && !originalRequest._retry) {
                    console.log("Backend returned unauthorized error");
                    originalRequest._retry = true;
                    try {
                        console.log("Trying calling token refresh");
                        const response = await RefreshToken();
                        if (response.success && response.status == 200) {
                            console.log("Token refresh successfull");
                            return customAxios(originalRequest);
                        }
                        else {
                            console.log("Token refresh failed expired maybe");
                            setIsAuthenticated(false);
                            setRoles([]);
                        }
                    } catch (e) {
                        console.log("Token refresh failed!");
                        console.error('Token refresh failed', e);
                    }
                }
                return Promise.reject(error);
            }
        );

        return () => {
            console.log("cleaning...");
            customAxios.interceptors.response.eject(responseIntercept);
        }

    }, []);

    return children;
};

export default CustomAxiosProvider;