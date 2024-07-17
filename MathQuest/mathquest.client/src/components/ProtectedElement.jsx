import PropTypes from 'prop-types';
import { useAuth } from '../context/useAuth';

const ProtectedElement = ({ children }) => {
    const { isAuthenticated } = useAuth();
    return isAuthenticated ? null : children;
};

ProtectedElement.propTypes = {
    children: PropTypes.node.isRequired,
};

export default ProtectedElement;