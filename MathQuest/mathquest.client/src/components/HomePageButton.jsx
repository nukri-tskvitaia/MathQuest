import { useNavigate } from 'react-router-dom';
import { useAuth } from '../context/useAuth';

const HomeButton = () => {
    const navigate = useNavigate();
    const { isAuthenticated } = useAuth();

    const handleHomeClick = () => {
        if (isAuthenticated) {
            navigate('/main');
        } else {
            navigate('/');
        }
    };

    return <button onClick={handleHomeClick}>Home</button>;
};

export default HomeButton;