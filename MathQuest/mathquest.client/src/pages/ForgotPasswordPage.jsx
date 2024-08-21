import { useState } from 'react';
import customAxios from '../services/customAxios';
import './ForgotPasswordPage.css';

const ForgotPasswordPage = () => {
    const [email, setEmail] = useState('');
    const [message, setMessage] = useState('');
    const [error, setError] = useState('');

    const handleSearch = async (e) => {
        e.preventDefault();
        setMessage('');
        setError('');
        try {
            const response = await customAxios.get('https://localhost:5001/api/authorization/search-email', {
                params: {
                    email: email
                }
            });
            if (response.status === 200) {
                setMessage('Password reset link has been sent to your email.');
            } else {
                throw new Error('User not found.');
            }
        } catch (err) {
            setError('User not found. Please try again.');
            console.error(err);
        }
    };

    return (
        <div className="forgot-password-container">
            <div className="forgot-password-form">
                <h2>Forgot Password</h2>
                <form onSubmit={handleSearch}>
                    <div className="form-group">
                        <label>Email:</label>
                        <input
                            type="email"
                            value={email}
                            onChange={(e) => setEmail(e.target.value)}
                            placeholder="Enter your email"
                            required
                        />
                    </div>
                    {error && <p className="error-message">{error}</p>}
                    {message && <p className="success-message">{message}</p>}
                    <button type="submit" className="forgot-password-button">
                        Search
                    </button>
                </form>
            </div>
        </div>
    );
};

export default ForgotPasswordPage;