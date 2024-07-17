import { useState } from 'react';
import { register, resendConfirmationEmail } from '../services/auth';
import './RegisterPage.css';

const genderMapping = {
    'Male': 0,
    'Female': 1,
    'Neither': 2
};

const RegisterPage = () => {
    const [firstName, setFirstName] = useState('');
    const [lastName, setLastName] = useState('');
    const [profilePicture, setProfilePicture] = useState(null);
    const [birthDate, setBirthDate] = useState('');
    const [gender, setGender] = useState('');
    const [phoneNumber, setPhoneNumber] = useState('');
    const [userName, setUserName] = useState('');
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [confirmPassword, setConfirmPassword] = useState('');
    const [error, setError] = useState('');
    const [message, setMessage] = useState('');

    const [tempEmail, setTempEmail] = useState('');
    const [registered, setRegistered] = useState(false);
    const [resendAttempts, setResendAttempts] = useState(0);

    const handleFileChange = (e) => {
        const file = e.target.files[0];
        if (file && (file.type === 'image/jpeg' || file.type === 'image/png' || file.type === 'image/jpg')) {
            setProfilePicture(file);
            setError('');
        } else {
            setError('Only JPG, JPEG and PNG files are allowed.');
            setProfilePicture(null);
        }
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError('');
        setMessage('');

        // Validate First Name
        if (!/^[A-Za-z]+$/.test(firstName)) {
            setError('First Name should only contain English characters.');
            return;
        }
        if (firstName.length < 3) {
            setError('First Name should be at least 3 characters long.');
            return;
        }

        // Validate Last Name
        if (!/^[A-Za-z]+$/.test(lastName)) {
            setError('Last Name should only contain English characters.');
            return;
        }
        if (lastName.length < 3) {
            setError('Last Name should be at least 3 characters long.');
            return;
        }

        // Format First Name and Last Name
        const formattedFirstName = firstName.charAt(0).toUpperCase() + firstName.slice(1).toLowerCase();
        const formattedLastName = lastName.charAt(0).toUpperCase() + lastName.slice(1).toLowerCase();

        // Validate Age
        const age = new Date().getFullYear() - new Date(birthDate).getFullYear();
        if (age < 5 || age > 90) {
            setError('Age must be between 5 and 90.');
            return;
        }

        // Validate Phone Number
        if (phoneNumber !=='' && (!/^\d{9,10}$/.test(phoneNumber))) {
            setError('Phone Number should only contain digits and be 9 or 10 digits long.');
            return;
        }

        // Validate Username
        if (!/^[A-Za-z][A-Za-z\d]{2,29}$/.test(userName) || /\d{5,}/.test(userName) || (userName.match(/_/g) || []).length > 3) {
            setError('Username should begin with a character, contain only characters, digits, and up to three underscores and at most 4 digits. Username can be 3-30 characters long');
            return;
        }

        // Validate Password
        if (password.length < 8 || !/[A-Z]/.test(password) || !/[a-z]/.test(password) || !/\d/.test(password) || !/[!@#$%^&*]/.test(password)) {
            setError('Password should be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one number, and one special character.');
            return;
        }

        // Validate Confirm Password
        if (password !== confirmPassword) {
            setError('Passwords do not match');
            return;
        }

        const mappedGender = genderMapping[gender];

        const formData = new FormData();
        formData.append('firstName', formattedFirstName);
        formData.append('lastName', formattedLastName);
        if (profilePicture) {
            formData.append('profilePicture', profilePicture);
        }
        formData.append('birthDate', birthDate);
        formData.append('gender', mappedGender);
        formData.append('phoneNumber', phoneNumber);
        formData.append('userName', userName);
        formData.append('email', email);
        formData.append('password', password);
        formData.append('confirmPassword', confirmPassword);

        setTempEmail(email);

        try {
            for (let [key, value] of formData.entries()) {
                console.log(`${key}: ${value}`);
            }
            const response = await register(formData);

            if (response.success) {
                setMessage('Registration successful! Please check your email to confirm your account.');
                setFirstName('');
                setLastName('');
                setProfilePicture(null);
                setBirthDate('');
                setGender('');
                setPhoneNumber('');
                setUserName('');
                setEmail('');
                setPassword('');
                setConfirmPassword('');
                setResendAttempts(0);
                setRegistered(true);
            } else {
                setError(response.message || 'Registration failed');
            }
        } catch (err) {
            setError('An error occurred. Please try again.');
            console.error(err);
        }
    };

    const handleResendConfirmation = async () => {
        if (resendAttempts >= 10) {
            setMessage('You have exceeded the maximum number of resend attempts.');
            setRegistered(false);
            return;
        }

        try {
            const response = await resendConfirmationEmail(tempEmail);
            setMessage(response.message);
            setResendAttempts(prevAttempts => prevAttempts + 1);
        } catch (error) {
            setMessage('Failed to resend confirmation email. Email is already confirmed.');
            console.error('Error resending confirmation email:', error);
        }
    };

    return (
        <div className="register-container">
            <form onSubmit={handleSubmit} className="register-form">
                <h2>Register</h2>
                <h4>* Required</h4>
                <div className="form-group">
                    <label>*First Name:</label>
                    <input
                        type="text"
                        value={firstName}
                        onChange={(e) => setFirstName(e.target.value)}
                        placeholder="Enter your first name"
                        required
                    />
                </div>
                <div className="form-group">
                    <label>*Last Name:</label>
                    <input
                        type="text"
                        value={lastName}
                        onChange={(e) => setLastName(e.target.value)}
                        placeholder="Enter your last name"
                        required
                    />
                </div>
                <div className="form-group">
                    <label>Profile Picture:</label>
                    <input
                        type="file"
                        onChange={handleFileChange}
                        accept="image/jpeg, image/jpg, image/png"
                    />
                    <small>Only JPG, JPEG, and PNG image formats are allowed.</small>
                </div>
                <div className="form-group">
                    <label>*Birth Date:</label>
                    <input
                        type="date"
                        value={birthDate}
                        onChange={(e) => setBirthDate(e.target.value)}
                        required
                    />
                </div>
                <div className="form-group">
                    <label>*Gender:</label>
                    <select
                        value={gender}
                        onChange={(e) => setGender(e.target.value)}
                        required
                    >
                        <option value="">Select Gender</option>
                        <option value="Male">Male</option>
                        <option value="Female">Female</option>
                        <option value="Neither">Neither</option>
                    </select>
                </div>
                <div className="form-group">
                    <label>Phone Number:</label>
                    <input
                        type="text"
                        value={phoneNumber}
                        onChange={(e) => setPhoneNumber(e.target.value)}
                        placeholder="Enter your phone number"
                    />
                </div>
                <div className="form-group">
                    <label>*Username:</label>
                    <input
                        type="text"
                        value={userName}
                        onChange={(e) => setUserName(e.target.value)}
                        placeholder="Enter your username"
                        required
                    />
                </div>
                <div className="form-group">
                    <label>*Email:</label>
                    <input
                        type="email"
                        value={email}
                        onChange={(e) => setEmail(e.target.value)}
                        placeholder="Enter your email"
                        required
                    />
                </div>
                <div className="form-group">
                    <label>*Password:</label>
                    <input
                        type="password"
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}
                        placeholder="Enter your password"
                        required
                    />
                    <small>Password should be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one number, and one special character.</small>
                </div>
                <div className="form-group">
                    <label>*Confirm Password:</label>
                    <input
                        type="password"
                        value={confirmPassword}
                        onChange={(e) => setConfirmPassword(e.target.value)}
                        placeholder="Confirm your password"
                        required
                    />
                </div>
                {error && <p className="error-message">{error}</p>}
                {message && <p className="success-message">{message}</p>}
                <button type="submit" className="register-button">Register</button>
                {registered && <button type="button" className="resend-button" onClick={handleResendConfirmation}>Resend Confirmation Email</button>}
            </form>
        </div>
    );
};

export default RegisterPage;