import PropTypes from 'prop-types';
import { Navigate } from 'react-router-dom';
import { useAuth } from '../context/useAuth';

const ProtectedRoute = ({ element, redirectTo }) => {
    const { isAuthenticated, loading } = useAuth();

    if (loading) {
        return <div>Loading...</div>;
    }

    return isAuthenticated ? <Navigate to={redirectTo} /> : element;
};

ProtectedRoute.propTypes = {
    element: PropTypes.element.isRequired,
    redirectTo: PropTypes.string.isRequired,
};

export default ProtectedRoute;