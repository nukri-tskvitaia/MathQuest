import { useState, useMemo } from 'react';
import PropTypes from 'prop-types';
import { AuthContext } from './useAuth';

export const AuthProvider = ({ children }) => {

    const [isAuthenticated, setIsAuthenticated] = useState(false);
    const [roles, setRoles] = useState([]);

    const value = useMemo(() => ({
        isAuthenticated,
        setIsAuthenticated,
        roles,
        setRoles
    }), [isAuthenticated, roles]);

    return (
        <AuthContext.Provider value={value}>
            {children}
        </AuthContext.Provider>
    );
};

AuthProvider.propTypes = {
    children: PropTypes.node.isRequired,
};