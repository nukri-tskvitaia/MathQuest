import { useState } from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import customAxios from '../services/customAxios';
import { useAuth } from '../context/useAuth';
import './LoginTwoFactorPage.css';

const LoginTwoFactorPage = () => {
    const [code, setCode] = useState('');
    const [rememberMe, setRememberMe] = useState(false);
    const [rememberTwoFactorDevice, setRememberTwoFactorDevice] = useState(false);
    const [error, setError] = useState('');
    const navigate = useNavigate();
    const location = useLocation();
    const { email } = location.state || null
    const { setIsAuthenticated, setRoles } = useAuth();

    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            const response = await customAxios.post('/api/authorization/verify-2fa', { email, code, rememberMe, rememberTwoFactorDevice });
            if (response.status === 200) {
                setIsAuthenticated(true);
                console.log(`It went through 2 factor authentication`);
                setRoles(response.data.roles);
                navigate('/main');
            } else {
                setIsAuthenticated(false);
                setError('Invalid code. Please try again.');
            }
        } catch (error) {
            console.error('Error verifying 2FA code', error);
            setError('An error occurred. Please try again.');
        }
    };

    return (
        <div className="two-factor-container">
            <div className="two-factor-card">
                <h1>Two-Factor Authentication</h1>
                <form onSubmit={handleSubmit} className="two-factor-form">
                    <div className="form-group">
                        <label htmlFor="code">Enter 2FA Code</label>
                        <input
                            type="text"
                            id="code"
                            value={code}
                            onChange={(e) => setCode(e.target.value)}
                            required
                            placeholder="Enter your 2FA code"
                        />
                    </div>
                    <div className="form-group">
                        <input
                            type="checkbox"
                            id="rememberMe"
                            checked={rememberMe}
                            onChange={(e) => setRememberMe(e.target.checked)}
                        />
                        <label htmlFor="rememberMe">Remember Me</label>
                    </div>
                    <div className="form-group">
                        <input
                            type="checkbox"
                            id="rememberTwoFactorDevice"
                            checked={rememberTwoFactorDevice}
                            onChange={(e) => setRememberTwoFactorDevice(e.target.checked)}
                        />
                        <label htmlFor="rememberTwoFactorDevice">Remember This Device</label>
                    </div>
                    {error && <p className="error-message">{error}</p>}
                    <button type="submit" className="submit-button">Verify</button>
                </form>
            </div>
        </div>
    );
};

export default LoginTwoFactorPage;