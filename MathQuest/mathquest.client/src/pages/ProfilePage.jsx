import { useState } from 'react';
import { Link, Outlet } from 'react-router-dom';
import './ProfilePage.css';

const ProfilePage = () => {
    const [isContentVisible, setIsContentVisible] = useState(false);

    const handleLinkClick = () => {
        setIsContentVisible(true);
    };

    return (
        <div className="profile-page">
            <div className="profile-header">
                <h2>Profile</h2>
            </div>
            <div className="profile-main">
                <nav className="profile-nav">
                    <ul className="profile-nav-list">
                        <li className="profile-nav-item">
                            <Link to="info" className="profile-nav-link" onClick={handleLinkClick}>User Info</Link>
                        </li>
                        <li className="profile-nav-item">
                            <Link to="change-password" className="profile-nav-link" onClick={handleLinkClick}>Change Password</Link>
                        </li>
                        <li className="profile-nav-item">
                            <Link to="two-factor" className="profile-nav-link" onClick={handleLinkClick}>Two-Factor Authentication</Link>
                        </li>
                    </ul>
                </nav>
                <div className={`profile-content ${isContentVisible ? 'visible' : 'hidden'}`}>
                    <Outlet />
                </div>
            </div>
        </div>
    );
};

export default ProfilePage;