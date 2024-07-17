import { useNavigate } from 'react-router-dom';

const RegisterButton = () => {
    const navigate = useNavigate();

    const handleRegisterClick = () => {
        navigate('/register');
    };

    return <button onClick={handleRegisterClick}>Register</button>;
};

export default RegisterButton;