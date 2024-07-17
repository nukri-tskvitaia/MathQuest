import { useEffect, useState } from 'react';
import customAxios from '../services/customAxios';
import { format } from 'date-fns';
import './UserInfoPage.css';

const UserInfo = () => {
    const [userInfo, setUserInfo] = useState(null);
    const [error, setError] = useState('');
    const [editMode, setEditMode] = useState(false);
    const [formData, setFormData] = useState({
        firstName: '',
        lastName: '',
        email: '',
        birthDate: '',
        gender: ''
    });
    const [successMessage, setSuccessMessage] = useState('');
    const [fetchTrigger, setFetchTrigger] = useState(false);

    useEffect(() => {
        const fetchUserInfo = async () => {
            try {
                const response = await customAxios.get('/api/account/manage/user-info');
                if (response.status === 200) {
                    setUserInfo(response.data);
                    setFormData({
                        firstName: response.data.firstName,
                        lastName: response.data.lastName,
                        email: response.data.email,
                        birthDate: format(new Date(response.data.birthDate), 'yyyy-MM-dd'),
                        gender: response.data.gender
                    });
                } else {
                    throw new Error('Failed to fetch user info.');
                }
            } catch (err) {
                setError('An error occurred while fetching user info. Please try again.');
                console.error(err);
            }
        };

        fetchUserInfo();
    }, [fetchTrigger]);

    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData((prevData) => ({
            ...prevData,
            [name]: value
        }));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        const { firstName, lastName, email, birthDate, gender } = formData;

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

        // Clear any previous error messages
        setError('');

        // Prepare the data to be sent to the backend
        const updatedFormData = {
            ...formData,
            firstName: formattedFirstName,
            lastName: formattedLastName
        };

        try {
            console.log(updatedFormData);
            const response = await customAxios.put('/api/account/manage/update/user-info', updatedFormData);
            if (response.status === 200) {
                setUserInfo(response.data);
                setEditMode(false);
                setSuccessMessage('User info updated successfully.');
                setTimeout(() => setSuccessMessage(''), 3000);
                setFetchTrigger(prev => !prev);
            } else {
                throw new Error('Unexpected response status');
            }
        } catch (err) {
            if (err.response && err.response.status === 400) {
                setError(err.response.data.message);
            } else {
                setError('Failed to update user info.');
            }
            console.error(err);
        }
    };

    if (!userInfo) {
        return <p>Loading...</p>;
    }

    return (
        <div className="user-info-container">
            <h2>User Info</h2>
            {successMessage && <p className="success-message">{successMessage}</p>}
            {editMode ? (
                <form onSubmit={handleSubmit} className="user-info-form">
                    {error && <p className="error-message">{error}</p>}
                    <div className="form-group">
                        <label>First Name:</label>
                        <input
                            type="text"
                            name="firstName"
                            value={formData.firstName}
                            onChange={handleChange}
                        />
                    </div>
                    <div className="form-group">
                        <label>Last Name:</label>
                        <input
                            type="text"
                            name="lastName"
                            value={formData.lastName}
                            onChange={handleChange}
                        />
                    </div>
                    <div className="form-group">
                        <label>Email:</label>
                        <input
                            type="email"
                            name="email"
                            value={formData.email}
                            onChange={handleChange}
                        />
                    </div>
                    <div className="form-group">
                        <label>Birthdate:</label>
                        <input
                            type="date"
                            name="birthDate"
                            value={formData.birthDate}
                            onChange={handleChange}
                        />
                    </div>
                    <div className="form-group">
                        <label>Gender:</label>
                        <select
                            name="gender"
                            value={formData.gender}
                            onChange={handleChange}
                        >
                            <option value="male">Male</option>
                            <option value="female">Female</option>
                            <option value="other">Other</option>
                        </select>
                    </div>
                    <button type="submit">Save</button>
                    <button type="button" onClick={() => setEditMode(false)}>Cancel</button>
                </form>
            ) : (
                <div className="user-info">
                    <p><strong>First Name:</strong> {userInfo.firstName}</p>
                    <p><strong>Last Name:</strong> {userInfo.lastName}</p>
                    <p><strong>Email:</strong> {userInfo.email}</p>
                    <p><strong>Birthdate:</strong> {format(new Date(userInfo.birthDate), 'yyyy-MM-dd')}</p>
                    <p><strong>Gender:</strong> {userInfo.gender}</p>
                    <button onClick={() => setEditMode(true)}>Edit</button>
                </div>
            )}
        </div>
    );
};

export default UserInfo;