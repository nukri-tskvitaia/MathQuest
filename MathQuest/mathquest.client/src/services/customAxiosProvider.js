import { useEffect, useState } from 'react';
import customAxios from './customAxios';
import { useAuth } from '../context/useAuth';
import RefreshToken from './RefreshToken';
import PropTypes from 'prop-types';

const CustomAxiosProvider = ({ children }) => {
    const { setIsAuthenticated, setRoles } = useAuth();
    const [interceptorSet, setInterceptorSet] = useState(false);

    useEffect(() => {
        console.log("Setting up response interceptor");

        const responseIntercept = customAxios.interceptors.response.use(
            response => {
                // If the response is successful, return it
                return response;
            },

            async error => {
                const originalRequest = error.config;
                console.log("#1 _retry flag:", originalRequest?._retry);

                if (
                    error.response && error.response.status === 401 &&
                    originalRequest && !originalRequest._retry &&
                    !originalRequest.url.includes(`/api/authorization/refresh`)) // this is crucial to prevent infinite loop
                {
                    console.log("Unauthorized error detected, attempting token refresh");

                    originalRequest._retry = true;  // To prevent looping
                    console.log("#2 _retry flag set to true");

                    try {
                        const response = await RefreshToken();

                        if (response?.success && response.status === 200) {
                            console.log("Token refresh successful, updating state and retrying original request");
                            setRoles(response.roles);
                            setIsAuthenticated(true);

                            // Retry the original request
                            return customAxios(originalRequest);
                        } else {
                            console.log("Message is: ", response.message);
                            setIsAuthenticated(false);
                            setRoles([]);
                        }
                    } catch (error) {
                        console.error("Token refresh failed. Logging out!", error);
                        setIsAuthenticated(false);
                        setRoles([]);
                        return Promise.reject(error)
                    }
                }

                // If the retry flag is already set, reject the error to prevent infinite loops
                return Promise.reject(error);
            }
        );

        setInterceptorSet(true); // Indicate that the interceptor has been set up

        return () => {
            console.log("Cleaning up interceptor");
            customAxios.interceptors.response.eject(responseIntercept);
        };

    }, [setIsAuthenticated, setRoles]);

    if (!interceptorSet) {
        return null; // Optionally render a setup indicator
    }

    return children;
};

CustomAxiosProvider.propTypes = {
    children: PropTypes.node.isRequired,
};

export default CustomAxiosProvider;