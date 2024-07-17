import { useState } from 'react';
import { useNavigate, useLocation, Link } from 'react-router-dom';
import { login } from '../services/auth';
import { useAuth } from '../context/useAuth';
import './LoginPage.css';

const LoginPage = () => {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [remember, setRemember] = useState(false);
    const [error, setError] = useState('');
    const { setIsAuthenticated, setRoles } = useAuth();

    const navigate = useNavigate();
    const location = useLocation();
    const from = location.state?.from?.pathname || "/";

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError('');
        try {
            const response = await login(email, password, remember);
            if (response.success) {
                console.log(`requires two factor is ${response.requires2FA}!`)
                if (response.requires2FA) {
                    console.log('it went here!')
                    navigate('/login-2fa', { state: { email } });
                }
                else {
                    setIsAuthenticated(true);
                    console.log(`User roles: ${response.roles}`);
                    setRoles(response.roles);
                    navigate(from, { replace: true });
                }

            } else {
                setIsAuthenticated(false);
                setError(response.message);
            }
        } catch (err) {
            setError('An error occurred. Please try again.');
            console.error(err);
        }
    };

    return (
        <div className="login-container">
            <form onSubmit={handleSubmit} className="login-form">
                <h2>Login</h2>
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
                <div className="form-group">
                    <label>Password:</label>
                    <input
                        type="password"
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}
                        placeholder="Enter your password"
                        required
                    />
                </div>
                <div className="form-group remember-me">
                    <label>
                        <input
                            type="checkbox"
                            checked={remember}
                            onChange={(e) => setRemember(e.target.checked)}
                        />
                        Remember Me
                    </label>
                </div>
                {error && <p className="error-message">{error}</p>}
                <button type="submit" className="login-button">Login</button>
                <div className="forgot-password">
                    <Link to="/forgot-password">Forgot Password?</Link>
                </div>
            </form>
        </div>
    );
};

export default LoginPage;