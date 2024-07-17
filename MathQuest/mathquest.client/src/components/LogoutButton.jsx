import { useNavigate } from 'react-router-dom';

const LogoutButton = () => {
    const navigate = useNavigate();

    const handleLogoutClick = () => {
        navigate('/logout');
    };

    return <button onClick={handleLogoutClick}>Logout</button>;
};

export default LogoutButton;