import { useState, useEffect } from 'react';
import customAxios from '../../services/customAxios';
import './AdminPage.css';

const AdminPage = () => {
    const [users, setUsers] = useState([]);
    const [selectedUser, setSelectedUser] = useState(null);
    const [feedbacks, setFeedbacks] = useState([]);
    const [isFeedbackModalOpen, setIsFeedbackModalOpen] = useState(false);
    const [error, setError] = useState('');
    const [searchEmail, setSearchEmail] = useState('');
    const [fromDate, setFromDate] = useState('');
    const [toDate, setToDate] = useState('');
    const [hasSearched, setHasSearched] = useState(false);
    const [userRoles, setUserRoles] = useState([]);
    const [newRole, setNewRole] = useState('');

    useEffect(() => {
        if (selectedUser) {
            fetchUserRoles(selectedUser.email);
        }
    }, [selectedUser]);

    const fetchUsers = async () => {
        try {
            const response = await customAxios.get('/api/admin/get-users');
            if (response.status === 200) {
                setUsers(response.data);
                setSelectedUser(null);
                setHasSearched(false);
            } else if (response.status === 204) {
                setUsers([]);
            }
        } catch (error) {
            setError('Error fetching users.');
            console.error(error);
        }
    };

    const handleUserClick = async (email) => {
        try {
            const response = await customAxios.get(`/api/admin/get-user/${email}`);
            if (response.status === 200) {
                setSelectedUser(response.data);
            }
        } catch (error) {
            setError('Error fetching user details.');
            console.error(error);
        }
    };

    const fetchUserFeedbacks = async (userId, fromDate, toDate) => {
        try {
            setError('');
            const response = await customAxios.get(`/api/admin/feedbacks/${userId}`, {
                params: {
                    fromDate: new Date(fromDate).toISOString(),
                    toDate: new Date(toDate).toISOString()
                }
            });
            if (response.status === 200) {
                setFeedbacks(response.data);
                setIsFeedbackModalOpen(true);
            }
        } catch (error) {
            setError('Error fetching feedbacks.');
            console.error(error);
        }
    };

    const fetchAllFeedbacks = async () => {
        try {
            setError('');
            const response = await customAxios.get(`/api/admin/feedbacks`, {
                params: {
                    fromDate: new Date(fromDate).toISOString(),
                    toDate: new Date(toDate).toISOString()
                }
            });
            if (response.status === 200) {
                setFeedbacks(response.data);
            }
        } catch (error) {
            setError('Error fetching feedbacks.');
            console.error(error);
        }
    };

    const handleDeleteUser = async (email) => {
        try {
            const response = await customAxios.delete(`/api/admin/delete-user/${email}`);
            if (response.status === 200) {
                fetchUsers();
                setSelectedUser(null);
            }
        } catch (error) {
            setError('Error deleting user.');
            console.error(error);
        }
    };

    const handleLockoutUser = async (email) => {
        try {
            const response = await customAxios.post(`/api/admin/user-lockout`, { email });
            if (response.status === 200) {
                fetchUsers();
                setSelectedUser(null);
            }
        } catch (error) {
            setError('Error locking out user.');
            console.error(error);
        }
    };

    const handleSearchUser = async () => {
        if (!searchEmail) {
            setError('Please enter an email to search.');
            return;
        }
        setHasSearched(true);
        setError('');
        try {
            const response = await customAxios.get(`/api/admin/get-user/${searchEmail}`);
            if (response.status === 200) {
                setUsers([response.data]);
                setSelectedUser(response.data);
            } else {
                setUsers([]);
                setSelectedUser(null);
            }
        } catch (error) {
            setError('Error searching for user.');
            console.error(error);
        }
    };

    const fetchUserRoles = async (email) => {
        try {
            const response = await customAxios.get(`/api/admin/user/role/${email}`);
            if (response.status === 200) {
                setUserRoles(response.data);
            }
        } catch (error) {
            setError('Error fetching user roles.');
            console.error(error);
        }
    };

    const handleAddUserRole = async () => {
        try {
            const response = await customAxios.post(`/api/admin/user/role`, { roleName: newRole, email: selectedUser.email });
            if (response.status === 200) {
                fetchUserRoles(selectedUser.email);
                setNewRole('');
            }
        } catch (error) {
            setError('Error adding user role.');
            console.error(error);
        }
    };

    const handleDeleteUserRole = async (roleName) => {
        try {
            const response = await customAxios.delete(`/api/admin/user/role`, { data: { email: selectedUser.email, roleName } });
            if (response.status === 200) {
                fetchUserRoles(selectedUser.email);
            }
        } catch (error) {
            setError('Error deleting user role.');
            console.error(error);
        }
    };

    return (
        <div className="admin-page">
            <div className="users-section">
                <h2>Users</h2>
                <button onClick={fetchUsers}>See All Users</button>
                <input
                    type="text"
                    placeholder="Search by email"
                    value={searchEmail}
                    onChange={(e) => setSearchEmail(e.target.value)}
                />
                <button onClick={handleSearchUser}>Search User</button>
                {users.length > 0 ? (
                    <ul>
                        {users.map(user => (
                            <li key={user.email} onClick={() => handleUserClick(user.email)}>
                                {user.email}
                            </li>
                        ))}
                    </ul>
                ) : (
                    hasSearched && <p>No such user exists.</p>
                )}
            </div>
            <div className="details-section">
                {selectedUser && (
                    <div>
                        <h2>{selectedUser.email}</h2>
                        <button onClick={() => handleDeleteUser(selectedUser.email)}>Delete User</button>
                        <button onClick={() => handleLockoutUser(selectedUser.email)}>Lockout User</button>
                        <input
                            type="date"
                            placeholder="From Date"
                            value={fromDate}
                            onChange={(e) => setFromDate(e.target.value)}
                        />
                        <input
                            type="date"
                            placeholder="To Date"
                            value={toDate}
                            onChange={(e) => setToDate(e.target.value)}
                        />
                        <button onClick={() => fetchUserFeedbacks(selectedUser.id, fromDate, toDate)}>View Feedbacks</button>

                        <div className="roles-section">
                            <h3>User Roles</h3>
                            <ul>
                                {userRoles.map(role => (
                                    <li key={role}>
                                        {role}
                                        <button onClick={() => handleDeleteUserRole(role)}>Remove Role</button>
                                    </li>
                                ))}
                            </ul>
                            <input
                                type="text"
                                placeholder="Add new role"
                            value={newRole}
                            onChange={(e) => setNewRole(e.target.value)}
                            />
                            <button onClick={handleAddUserRole}>Add Role</button>
                        </div>
                    </div>
                )}
            </div>
            {isFeedbackModalOpen && (
                <div className="feedback-modal">
                    <div className="feedback-modal-content">
                        <h2>Feedbacks for {selectedUser.email}</h2>
                        <ul>
                            {feedbacks.map(feedback => (
                                <li key={feedback.id}>
                                    <p><strong>Date:</strong> {new Date(feedback.feedbackDate).toLocaleDateString()}</p>
                                    <p><strong>Text:</strong> {feedback.feedbackText}</p>
                                    <p><strong>Description:</strong> {feedback.feedbackDescription}</p>
                                </li>
                            ))}
                        </ul>
                        <button onClick={() => setIsFeedbackModalOpen(false)}>Close</button>
                    </div>
                </div>
            )}
            <div className="feedbacks-section">
                <h2>Feedbacks</h2>
                <input
                    type="date"
                    placeholder="From Date"
                    value={fromDate}
                    onChange={(e) => setFromDate(e.target.value)}
                />
                <input
                    type="date"
                    placeholder="To Date"
                    value={toDate}
                    onChange={(e) => setToDate(e.target.value)}
                />
                <button onClick={fetchAllFeedbacks}>Fetch All Feedbacks</button>
                <ul>
                    {feedbacks.map(feedback => (
                        <li key={feedback.id}>
                            <p><strong>Date:</strong> {new Date(feedback.feedbackDate).toLocaleDateString()}</p>
                            <p><strong>Text:</strong> {feedback.feedbackText}</p>
                            <p><strong>Description:</strong> {feedback.feedbackDescription}</p>
                        </li>
                    ))}
                </ul>
            </div>
            {error && <p className="error-message">{error}</p>}
        </div>
    );
};

export default AdminPage;
