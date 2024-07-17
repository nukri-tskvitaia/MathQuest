import { useState, useMemo, useEffect, useRef } from 'react';
import PropTypes from 'prop-types';
import { AuthContext } from './useAuth';
import RefreshToken from '../services/RefreshToken';
import customAxios from '../services/customAxios';

export const AuthProvider = ({ children }) => {

    const [isAuthenticated, setIsAuthenticated] = useState(() => {

        // Check localStorage for the initial state
        const savedState = localStorage.getItem('isAuthenticated');
        console.log(`isAuthenticated is ${savedState ? JSON.parse(savedState) : false }`);
        return savedState ? JSON.parse(savedState) : false;
    });

    const [roles, setRoles] = useState(() => {
        const savedRoles = localStorage.getItem('roles');
        console.log(`roles are ${savedRoles}`);
        if (savedRoles !== "undefined" && savedRoles !== "null") {
            try {
                return JSON.parse(savedRoles);
            } catch (error) {
                console.error("Error parsing roles from localStorage:", error);
                return [];
            }
        }
        return [];
    });

    const [loading, setLoading] = useState(true);
    const hasRunRef = useRef(false);

    useEffect(() => {

        // Save the state to localStorage whenever it changes
        localStorage.setItem('isAuthenticated', JSON.stringify(isAuthenticated));
    }, [isAuthenticated]);

    useEffect(() => {
        // Save the roles to localStorage whenever it changes
        localStorage.setItem('roles', JSON.stringify(roles));
    }, [roles]);

    const checkAndRefreshToken = async () => {

        if (isAuthenticated) {
            try {
                console.log(`is it authenticated? let's see ${isAuthenticated}`)
                const isAuthorized = await customAxios.get('/api/authorization/check-auth');

                if (isAuthorized.status == 200) {
                    console.log("access token is valid no need to call refresh token endpoint")
                    setLoading(false);
                    return;
                }
                else {
                    setLoading(false);
                }
            }
            catch (error) {
                if (error.response && error.response.status === 401) {
                    console.log("Unauthorized, attempting to refresh token");

                    try {
                        const response = await RefreshToken();
                        if (response.success && response.status == 200) {
                            console.log(" access token refreshed successfully");
                        }
                        else if (response.success && response.status == 204) {
                            console.log("access token is valid");
                        }
                        else {
                            console.log("Token refresh failed, expired maybe");
                            setIsAuthenticated(false);
                            setRoles([]);
                            return;
                        }
                    }
                    catch (error) {
                        console.error("Token refresh failed:", error);
                        setIsAuthenticated(false);
                        setRoles([]);
                    }
                }
                else {
                    console.error("An error occurred:", error);
                }
            }
            finally {
                setLoading(false);
            }
        }
        else {
            setLoading(false);
        }
    };

    useEffect(() => {
        if (!hasRunRef.current) {
            checkAndRefreshToken();
            hasRunRef.current = true;
        }
    }, [isAuthenticated]);

    useEffect(() => {
        const interval = setInterval(() => {
            checkAndRefreshToken();
        }, 30 * 60 * 1000); // Refresh token every 30 minutes

        return () => clearInterval(interval); // Cleanup on unmount
    }, [isAuthenticated]);

    const value = useMemo(() => ({
        isAuthenticated,
        setIsAuthenticated,
        roles,
        setRoles,
    }), [isAuthenticated, roles]);

    if (loading) {
        return <div>Loading...</div>;
    }

    return (
        <AuthContext.Provider value={value}>
            {children}
        </AuthContext.Provider>
    );
};

AuthProvider.propTypes = {
    children: PropTypes.node.isRequired,
};