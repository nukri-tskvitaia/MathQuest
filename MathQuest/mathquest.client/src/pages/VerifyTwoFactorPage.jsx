import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { verifyTwoFactor } from '../services/auth';
import { useAuth } from '../context/useAuth';
import './VerifyTwoFactor.css';

const VerifyTwoFactor = () => {
    const email = location.state?.email || '';
    const [verificationCode, setVerificationCode] = useState('');
    const [rememberMe, setRememberMe] = useState(false);
    const [rememberTwoFactor, setRememberTwoFactor] = useState(false);
    const [error, setError] = useState('');
    const [attempts, setAttempts] = useState(0);
    const navigate = useNavigate();
    const { setIsAuthenticated, setRoles } = useAuth();

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError('');
        try {
            const response = await verifyTwoFactor({
                verificationCode,
                email,
                rememberMe,
                rememberTwoFactor
            });
            if (response.success) {
                // Set authenticated state to true and handle access token if needed
                setIsAuthenticated(true);
                setRoles(response.roles);
                // Optionally, handle access token (e.g., save to local storage)
                // localStorage.setItem('accessToken', accessToken);
                navigate('/main'); // Redirect to main page on successful verification
            } else {
                setError(response.message);
                setAttempts(attempts + 1);
                // If the number of attempts exceeds 3, redirect to the login page
                if (attempts >= 3) {
                    setIsAuthenticated(false);
                    setRoles([]);
                    navigate('/login');
                }

                // Clear the verification code input field to allow the user to enter a new code
                setVerificationCode('');
            }
        } catch (err) {
            setError('An error occurred. Please try again.');
            console.error(err);
        }
    };

    return (
        <div className="verify-container">
            <div className="verify-card">
                <h2>Two-Factor Authentication Verification</h2>
                <form onSubmit={handleSubmit} className="verify-form">
                    <div className="form-group">
                        <label htmlFor="verificationCode">Verification Code:</label>
                        <input
                            type="text"
                            id="verificationCode"
                            value={verificationCode}
                            onChange={(e) => setVerificationCode(e.target.value)}
                            placeholder="Enter your verification code"
                            required
                        />
                    </div>
                    <div className="form-group">
                        <label>
                            <input type="checkbox" checked={rememberMe} onChange={(e) => setRememberMe(e.target.checked)}/>
                            Remember Me
                        </label>
                    </div>
                    <div className="form-group">
                        <label>
                            <input type="checkbox" checked={rememberTwoFactor} onChange={(e) => setRememberTwoFactor(e.target.checked)}/>
                            Remember this device for two-factor authentication
                        </label>
                    </div>
                    {error && <p className="error-message">{error}</p>}
                    <button type="submit" className="submit-button">Submit</button>
                </form>
            </div>
        </div>
    );
};

export default VerifyTwoFactor;