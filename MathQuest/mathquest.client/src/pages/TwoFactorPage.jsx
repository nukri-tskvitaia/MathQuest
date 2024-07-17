import { useState, useEffect } from 'react';
import customAxios from '../services/customAxios';
import './TwoFactorPage.css';

const TwoFactorAuth = () => {
    const [enabled, setEnabled] = useState(false);
    const [key, setKey] = useState('');
    const [image, setImage] = useState('');
    const [token, setToken] = useState('');
    const [message, setMessage] = useState('');
    const [error, setError] = useState('');

    useEffect(() => {
        isTwoFactorEnabled();
    }, []);

    const isTwoFactorEnabled = async () => {
        try {
            const response = await customAxios.get('api/account/is-two-factor-enabled');
            if (response.status === 200) {
                setEnabled(response.data);
            }
        } catch (err) {
            setError('An error occurred. Please try again.');
            console.error(err);
        }
    }

    const handleGenerateKey = async () => {
        setMessage('');
        setError('');
        try {
            const response = await customAxios.post('api/account/manage/setup-2fa');
            if (response.status === 200) {
                setKey(response.data.key);
                setImage(response.data.image);
                setMessage('Scan the QR code with your 2FA app and enter the generated token to verify.');
            } else {
                throw new Error('Failed to generate 2FA key.');
            }
        } catch (err) {
            setError('An error occurred. Please try again.');
            console.error(err);
        }
    };

    const handleVerifyToken = async () => {
        setMessage('');
        setError('');
        try {
            const response = await customAxios.post('api/account/manage/verify-2fa-token', JSON.stringify(token));
            if (response.status === 200) {
                setEnabled(true);
                setMessage('2FA was set up successfully.');
            } else {
                throw new Error('Invalid token.');
            }
        } catch (err) {
            setError('Invalid token. Please try again.');
            console.error(err);
        }
    };

    const handleDisable2FA = async () => {
        setMessage('');
        setError('');
        try {
            const response = await customAxios.post('api/account/manage/disable-2fa-token');
            if (response.status === 200) {
                setEnabled(false);
                setMessage('2FA has been disabled successfully.');
            } else {
                throw new Error('Failed to disable 2FA.');
            }
        } catch (err) {
            setError('An error occurred. Please try again.');
            console.error(err);
        }
    };

    return (
        <div className="two-factor-auth-container">
            <h2>Two-Factor Authentication</h2>
            <p>Two-Factor Authentication is currently <strong>{enabled ? 'enabled' : 'disabled'}</strong>.</p>
            {message && <p className="message">{message}</p>}
            {error && <p className="error-message">{error}</p>}
            {!enabled && (
                <div>
                    <button onClick={handleGenerateKey} className="button">Set Up 2FA</button>
                    {key && (
                        <div className="setup-container">
                            <p><strong>Key:</strong> {key}</p>
                            <img src={`data:image/png;base64,${image}`} alt="QR Code" className="qr-code" />
                            <div className="form-group">
                                <label htmlFor="token">Enter Token:</label>
                                <input
                                    type="text"
                                    id="token"
                                    value={token}
                                    onChange={(e) => setToken(e.target.value)}
                                    placeholder="Enter token from 2FA app"
                                    required
                                    className="input"
                                />
                                <button onClick={handleVerifyToken} className="button">Verify Token</button>
                            </div>
                        </div>
                    )}
                </div>
            )}
            {enabled && (
                <div>
                    <button onClick={handleDisable2FA} className="button">Disable 2FA</button>
                </div>
            )}
        </div>
    );
};

export default TwoFactorAuth;