import PropTypes from 'prop-types';
import { useAuth } from '../context/useAuth';

const allowedRoles = ["$2b$10$2aXYuiABCDy/VHZMBsgLkO4PyXPZORPSAF3KDJTG1iNp2VCEI.dGk"];

const AdminElement = ({ children }) => {
    const { isAuthenticated, roles } = useAuth();
    const hasRequiredRole = roles?.find(role => allowedRoles?.includes(role));

    return isAuthenticated  && hasRequiredRole ? children : null;
};

AdminElement.propTypes = {
    children: PropTypes.node.isRequired,
};

export default AdminElement;