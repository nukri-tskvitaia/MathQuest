import { useNavigate } from 'react-router-dom';
import './PermissionDeniedPage.css';

const PermissionDeniedPage = () => {
    const navigate = useNavigate();

    const handleBackToHome = () => {
        navigate('/main');
    };

    return (
        <div className="permission-container">
            <h1 className="title">Permission Denied</h1>
            <p className="message">You do not have the required permissions to access this page.</p>
            <button onClick={handleBackToHome} className="button">Back to Home</button>
        </div>
    );
};

export default PermissionDeniedPage;