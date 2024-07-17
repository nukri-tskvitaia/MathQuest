import PropTypes from 'prop-types';
import { Navigate } from 'react-router-dom';
import { useAuth } from '../context/useAuth';

const PrivateRoute = ({ element }) => {
    const { isAuthenticated } = useAuth();
    console.log(`Is Authenticated from private route? ${isAuthenticated}`);
    return isAuthenticated ? element : <Navigate to="/login" />;
};

PrivateRoute.propTypes = {
    element: PropTypes.element.isRequired,
};

export default PrivateRoute;