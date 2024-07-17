import { useLocation, Navigate, Outlet } from 'react-router-dom'
import PropTypes from 'prop-types';
import { useAuth } from '../context/useAuth'

const RequireAuth = ({ allowedRoles }) => {
    const { isAuthenticated, roles } = useAuth();
    const location = useLocation();

    const hasRequiredRole = roles?.find(role => allowedRoles?.includes(role));

    if (!isAuthenticated) {
        return <Navigate to="/login" state={{ from: location }} replace />
    }

    return (
        hasRequiredRole
            ? <Outlet /> : <Navigate to="/permission-denied" state={{ from: location }} replace />
    )
}

RequireAuth.propTypes = {
    allowedRoles: PropTypes.arrayOf(PropTypes.string).isRequired,
};

export default RequireAuth;