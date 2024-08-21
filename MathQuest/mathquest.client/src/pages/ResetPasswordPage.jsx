import { useState } from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import customAxios from '../services/customAxios';
import './ResetPasswordPage.css';

const ResetPasswordPage = () => {
    const navigate = useNavigate();
    const location = useLocation();
    const queryParams = new URLSearchParams(location.search);
    const email = queryParams.get('email');
    const token = queryParams.get('token');

    console.log(`email: ${email}, token: ${token}`);

    const [password, setPassword] = useState('');
    const [confirmPassword, setConfirmPassword] = useState('');
    const [message, setMessage] = useState('');
    const [error, setError] = useState('');

    const handleResetPassword = async (e) => {
        e.preventDefault();
        setMessage('');
        setError('');
        if (password !== confirmPassword) {
            setError('Passwords do not match.');
            return;
        }
        try {
            const response = await customAxios.post('https://localhost:5001/api/authorization/reset-password', { email, password, token });
            if (response.status === 200) {
                setMessage('Password has been reset successfully.');
                navigate('/login');
            } else {
                throw new Error('Failed to reset password.');
            }
        } catch (err) {
            setError('An error occurred. Please try again.');
            console.error(err);
        }
    };

    return (
        <div className="reset-password-container">
            <form onSubmit={handleResetPassword} className="reset-password-form">
                <h2>Reset Password</h2>
                <div className="form-group">
                    <label>New Password:</label>
                    <input
                        type="password"
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}
                        placeholder="Enter your new password"
                        required
                    />
                </div>
                <div className="form-group">
                    <label>Confirm Password:</label>
                    <input
                        type="password"
                        value={confirmPassword}
                        onChange={(e) => setConfirmPassword(e.target.value)}
                        placeholder="Confirm your new password"
                        required
                    />
                </div>
                {error && <p className="error-message">{error}</p>}
                {message && <p className="success-message">{message}</p>}
                <button type="submit" className="reset-password-button">Reset Password</button>
            </form>
        </div>
    );
};

export default ResetPasswordPage;