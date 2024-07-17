import { useState } from 'react';
import customAxios from '../services/customAxios';
import './ChangePasswordPage.css';

const ChangePassword = () => {
    const [oldPassword, setOldPassword] = useState('');
    const [newPassword, setNewPassword] = useState('');
    const [confirmNewPassword, setConfirmNewPassword] = useState('');
    const [message, setMessage] = useState('');
    const [error, setError] = useState('');

    const handleChangePassword = async (e) => {
        e.preventDefault();
        setMessage('');
        setError('');

        // Validate New Password
        if (newPassword.length < 8 ||
            !/[A-Z]/.test(newPassword) ||
            !/[a-z]/.test(newPassword) ||
            !/\d/.test(newPassword) ||
            !/[!@#$%^&*]/.test(newPassword)) {
            setError('Password should be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one number, and one special character.');
            return;
        }

        if (newPassword !== confirmNewPassword) {
            setError('New passwords do not match.');
            return;
        }

        try {
            const response = await customAxios.post('/api/account/manage/change-password', {
                oldPassword,
                newPassword,
            });
            if (response.status === 200) {
                setMessage('Password changed successfully.');
            } else {
                throw new Error('Failed to change password.');
            }
        } catch (err) {
            setError('An error occurred. Please try again.');
            console.error(err);
        }
    };

    return (
        <div className="change-password-container">
            <h2>Change Password</h2>
            <form onSubmit={handleChangePassword} className="change-password-form">
                <div className="form-group">
                    <label>Old Password:</label>
                    <input
                        type="password"
                        value={oldPassword}
                        onChange={(e) => setOldPassword(e.target.value)}
                        placeholder="Enter your old password"
                        required
                    />
                </div>
                <div className="form-group">
                    <label>New Password:</label>
                    <input
                        type="password"
                        value={newPassword}
                        onChange={(e) => setNewPassword(e.target.value)}
                        placeholder="Enter your new password"
                        required
                    />
                    <small>Password should be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one number, and one special character.</small>
                </div>
                <div className="form-group">
                    <label>Confirm New Password:</label>
                    <input
                        type="password"
                        value={confirmNewPassword}
                        onChange={(e) => setConfirmNewPassword(e.target.value)}
                        placeholder="Confirm your new password"
                        required
                    />
                </div>
                {error && <p className="error-message">{error}</p>}
                {message && <p className="success-message">{message}</p>}
                <button type="submit" className="change-password-button">Change Password</button>
            </form>
        </div>
    );
};

export default ChangePassword;