import { Outlet, Link } from 'react-router-dom';
import ProtectedElement from './components/ProtectedElement';
import PrivateElement from './components/PrivateElement';
import AdminElement from './components/AdminElement';
import './Layout.css';
import { useState } from 'react';

const Layout = () => {
    return (
        <div className="App">
            <HeaderLayout />
            <main className="Main">
                <Outlet />
            </main>
            <FooterLayout />
        </div>
    );
};

function HeaderLayout() {
    const [isMenuOpen, setIsMenuOpen] = useState(false);

    const toggleMenu = () => {
        setIsMenuOpen(!isMenuOpen);
    };

    return (
        <header>
            <nav>
                <div className="nav-img-title">
                    <Link to="/main">
                        <img src="/images/mathquest-logo.png" alt="site title"></img>
                    </Link>
                </div>
                <div className={`nav-items ${isMenuOpen ? 'open' : ''}`}>
                    <div className="home-section">
                        <Link to="/main">Home</Link>
                    </div>
                    <div>
                        <Link to="/learn">Learn</Link>
                    </div>
                    <div className="challenge-section">
                        <PrivateElement>
                            <Link to="/community" className="messages-section">
                                Community Chat
                            </Link>
                        </PrivateElement>
                    </div>
                    <div className="messages-section">
                        <PrivateElement>
                            <Link to="/messages" className="messages-section">
                                Messages
                            </Link>
                        </PrivateElement>
                    </div>
                    <div className="friends-section">
                        <PrivateElement>
                            <Link to="/friends">Friends</Link>
                        </PrivateElement>
                    </div>
                    <div className="leaderboard-section">
                        <PrivateElement>
                            <Link to="/leaderboard">Leaderboard</Link>
                        </PrivateElement>
                    </div>
                    <div className="profile-section">
                        <PrivateElement>
                            <Link to="/profile">Profile</Link>
                        </PrivateElement>
                    </div>
                    <div className="admin-section">
                        <AdminElement>
                            <Link to="/admin">Admin</Link>
                        </AdminElement>
                    </div>
                    <div className="login-section">
                        <ProtectedElement>
                            <Link to="/login">Login</Link>
                        </ProtectedElement>
                    </div>
                    <div className="register-section">
                        <ProtectedElement>
                            <Link to="/register">Register</Link>
                        </ProtectedElement>
                    </div>
                    <div className="logout-section">
                        <PrivateElement>
                            <Link to="/logout">Logout</Link>
                        </PrivateElement>
                    </div>
                </div>
                <div className="hamburger" onClick={toggleMenu}>
                    <div></div>
                    <div></div>
                    <div></div>
                </div>
            </nav>
        </header>
    );
}

function FooterLayout() {
    return (
        <footer className="footer-content">
            <div className="footer-container">
                <div>
                    <PrivateElement>
                        <Link to="/feedback" className="footer-link">Give Feedback</Link>
                    </PrivateElement>
                </div>
                <div className="footer-section">
                    <h3>Follow Us</h3>
                    <div className="social-media">
                        <a href="https://twitter.com" target="_blank" rel="noopener noreferrer"><img src="/images/footer/twitter.png" alt="Twitter" /></a>
                        <a href="https://youtube.com" target="_blank" rel="noopener noreferrer"><img src="/images/footer/youtube.png" alt="YouTube" /></a>
                        <a href="https://facebook.com" target="_blank" rel="noopener noreferrer"><img src="/images/footer/facebook.png" alt="Facebook" /></a>
                        <a href="https://instagram.com" target="_blank" rel="noopener noreferrer"><img src="/images/footer/instagram.png" alt="Instagram" /></a>
                    </div>
                </div>
                <div className="footer-section">
                    <h3>Legal</h3>
                    <ul>
                        <li><Link to="/privacy" className="footer-link">Privacy Policy</Link></li>
                        <li><Link to="/terms-of-service" className="footer-link">Terms of Service</Link></li>
                    </ul>
                </div>
            </div>
            <h4>&copy; MathQuest 2024</h4>
        </footer>
    );
}

export default Layout;