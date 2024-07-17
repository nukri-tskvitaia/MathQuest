import PropTypes from 'prop-types';
import { useAuth } from '../context/useAuth';

const PrivateElement = ({ children }) => {
    const { isAuthenticated } = useAuth();
    return isAuthenticated ? children : null;
};

PrivateElement.propTypes = {
    children: PropTypes.node.isRequired,
};

export default PrivateElement;