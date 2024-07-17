import { useState } from 'react';
import { useAuth } from '../context/useAuth';
import { useNavigate } from 'react-router-dom';
import customAxios from '../services/customAxios';
import './LogoutPage.css';

const LogoutPage = () => {
    const navigate = useNavigate();
    const { setIsAuthenticated, setRoles } = useAuth();
    const [confirmLogout, setConfirmLogout] = useState(false);

    const handleLogout = async () => {
        try {
            const response = await customAxios.post('/api/authorization/logout');

            if (response.status == 200) {
                setIsAuthenticated(false);
                setRoles([]);
                navigate('/login');
            } else {
                throw Error(response.message);
            }
        } catch (error) {
            console.error('Logout failed:', error);
        }
    };

    const handleYes = async () => {
        setConfirmLogout(false);
        await handleLogout();
    };

    const handleNo = () => {

        navigate('/');
    };

    if (!confirmLogout) {
        setConfirmLogout(true);
    }

    return (
        <div className="confirmation-dialog">
            <p className="confirmation-text">Are you sure you want to logout?</p>
            <div className="button-container">
                <button onClick={handleYes} className="confirmation-button yes-button">Yes</button>
                <button onClick={handleNo} className="confirmation-button no-button">No</button>
            </div>
        </div>
    );
};

export default LogoutPage;