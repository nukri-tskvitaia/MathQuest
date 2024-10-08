import './App.css';
import { useEffect, useState, useTransition } from 'react';
import { useAuth } from './context/useAuth';
import { Routes, Route } from 'react-router-dom';
import { checkAuth } from './services/authCheck';
import LoginPage from './pages/LoginPage';
import HomePage from './pages/HomePage';
import VerifyTwoFactor from './pages/VerifyTwoFactorPage';
import ConfirmEmail from './pages/EmailConfirmationPage';
import RegisterPage from './pages/RegisterPage';
import LogoutPage from './pages/LogoutPage';
import RequireAuth from './components/RequireAuth';
import Layout from './Layout';
import ResetPasswordPage from './pages/ResetPasswordPage';
import ForgotPasswordPage from './pages/ForgotPasswordPage';
import ProfilePage from './pages/ProfilePage';
import UserInfo from './pages/UserInfoPage';
import ChangePassword from './pages/ChangePasswordPage';
import TwoFactorAuth from './pages/TwoFactorPage';
import CommunityChatPage from './pages/signalR/CommunityChatPage';
import FriendsPage from './pages/friends/FriendsPage'
import LoginTwoFactorPage from './pages/LoginTwoFactorPage';
import NotFoundPage from './pages/NotFoundPage';
import MessengerPage from './pages/signalR/MessengerPage';
import LearnPage from './pages/LearnPage';
import Grade1 from './pages/learn/first/FirstGradePage';
import CountingPage from './pages/learn/first/counting/CountOneToHundred';
import SimpleArithemtics from './pages/learn/first/arithmetics/SimpleArithmetics';
import Adding1sAnd10sQuiz from './pages/learn/first/arithmetics/AddingOnesAndTenthQuiz';
import AdditionQuiz from './pages/learn/first/arithmetics/AdditionQuiz';
import SubtractionQuiz from './pages/learn/first/arithmetics/SubstractionQuiz';
import CountingQuiz from './pages/learn/first/counting/CountingQuiz';
import BeforeAfterQuiz from './pages/learn/first/counting/BeforeAfterQuiz';
import Geometry from './pages/learn/first/geometry/Geometry';
import LengthQuiz from './pages/learn/first/geometry/LengthQuiz';
import TimeQuiz from './pages/learn/first/geometry/TimeQuiz';
import ShapesQuiz from './pages/learn/first/geometry/ShapesQuiz';
import LeaderboardPage from './pages/LeaderboardPage';
import AdminPage from './pages/admin/AdminPage';
import PermissionDeniedPage from './pages/PermissionDeniedPage';
import FeedbackPage from './pages/FeedbackPage';
import PrivacyPolicyPage from './pages/PrivacyPolicyPage';
import TermsOfServicePage from './pages/TermsOfServicePage';
import ProtectedRoute from './components/ProtectedRoute';

function App() {

    const { setIsAuthenticated, setRoles } = useAuth()
    const [isPending, startTransition] = useTransition();
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        console.log("calling check-auth to get user auth status");

        startTransition(async () => {
            await checkAuth(setIsAuthenticated, setRoles);
            setLoading(false);
        })
    }, [setIsAuthenticated, setRoles]);

    if (isPending || loading) {
        return <div>Loading...</div>;
    }

    return (
        <Routes>
            <Route path="/" element={<Layout />}>

                {/* for all kinds of route requests */}
                <Route path="/reset-password" element={<ResetPasswordPage />} />
                <Route path="/privacy" element={<PrivacyPolicyPage />} />
                <Route path="/terms-of-service" element={<TermsOfServicePage />} />
                <Route path="/permission-denied" element={< PermissionDeniedPage />} />

                {/* only for non-authorized route requests */}
                <Route path="/login" element={
                    <ProtectedRoute element={<LoginPage />} redirectTo="/main" />
                } />
                <Route path="/register" element={
                    <ProtectedRoute element={<RegisterPage />} redirectTo="/main" />
                } />
                <Route path="/verify-2fa" element={
                    <ProtectedRoute element={<VerifyTwoFactor />} redirectTo="/main" />
                } />
                <Route path="/forgot-password" element={
                    <ProtectedRoute element={<ForgotPasswordPage />} redirectTo="/main" />
                } />
                <Route path="/login-2fa" element={
                    <ProtectedRoute element={<LoginTwoFactorPage />} redirectTo="/main" />
                } />
                <Route path="/confirm-email" element={
                    <ProtectedRoute element={<ConfirmEmail />} redirectTo="/main" />
                } />

                {/* for authorized route requests  */}
                <Route element={<RequireAuth allowedRoles={["Admin", "User"]} />}>
                    {/* Root URL displays HomePage */}
                    <Route index element={<HomePage />} />

                    <Route path="/logout" element={<LogoutPage />} />
                    <Route path="/main" element={<HomePage />} />
                    <Route path="/learn" element={<LearnPage />} />
                    <Route path="/community" element={<CommunityChatPage />} />
                    <Route path="/leaderboard" element={<LeaderboardPage />} />
                    <Route path="/profile" element={<ProfilePage />} >
                        <Route path="/profile/info" element={<UserInfo />} />
                        <Route path="/profile/change-password" element={<ChangePassword />} />
                        <Route path="/profile/two-factor" element={<TwoFactorAuth />} />
                    </Route>
                    <Route path="/friends" element={<FriendsPage />} />
                    <Route path="/messages" element={<MessengerPage />} />
                    <Route path="/feedback" element={<FeedbackPage />} />

                    {/* First Grade Content  */}
                    <Route path="/learn/first-grade" element={< Grade1 />} />
                    <Route path="/learn/first-grade/count/" element={< CountingPage />} />
                    <Route path="/learn/first-grade/count/counting-quiz" element={< CountingQuiz />} />
                    <Route path="/learn/first-grade/count/before-after-quiz" element={< BeforeAfterQuiz />} />
                    <Route path="/learn/first-grade/arithmetics" element={< SimpleArithemtics />} />
                    <Route path="/learn/first-grade/arithmetics/addition-quiz" element={< AdditionQuiz />} />
                    <Route path="/learn/first-grade/arithmetics/subtraction-quiz" element={< SubtractionQuiz />} />
                    <Route path="/learn/first-grade/arithmetics/adding-1s-10s-quiz" element={< Adding1sAnd10sQuiz />} />
                    <Route path="/learn/first-grade/geometry" element={< Geometry />} />
                    <Route path="/learn/first-grade/geometry/length-quiz" element={< LengthQuiz />} />
                    <Route path="/learn/first-grade/geometry/time-quiz" element={< TimeQuiz />} />
                    <Route path="/learn/first-grade/geometry/shapes-quiz" element={< ShapesQuiz />} />
                </Route>

                {/* only for admins  */}
                <Route element={<RequireAuth allowedRoles={["Admin"]} />}>
                    <Route path="/admin" element={< AdminPage />} />
                </Route>

                {/* Other routes <Route path="/messages:friendUserName" element={<MessagesPage />} />/ */}
                <Route path="*" element={<NotFoundPage />} />
            </Route>
        </Routes>
    );
}

export default App;