import { useState } from 'react';
import customAxios from '../services/customAxios';

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
        <div>
            <h2>Forgot Password</h2>
            <form onSubmit={handleSearch}>
                <div>
                    <label>Email:</label>
                    <input
                        type="email"
                        value={email}
                        onChange={(e) => setEmail(e.target.value)}
                        placeholder="Enter your email"
                        required
                    />
                </div>
                {error && <p style={{ color: 'red' }}>{error}</p>}
                {message && <p style={{ color: 'green' }}>{message}</p>}
                <button type="submit">Search</button>
            </form>
        </div>
    );
};

export default ForgotPasswordPage;