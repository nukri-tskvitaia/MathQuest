import { useState, useEffect } from 'react';

function UsersInfo() {
    const [users, setUsers] = useState([]);

    useEffect(() => {
        populateUserData();
    }, []);

    async function populateUserData() {
        try {
            const response = await fetch(`/api/authorization/users`, {
                method: 'GET',
                credentials: 'include',
                headers: {
                    'Content-Type': 'application/json'
                }
            });
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            const data = await response.json();
            setUsers(data);
        } catch (error) {
            console.error('Error fetching users:', error);
        }
    }

    function renderUsersTable(users) {
        if (!users || users.length === 0) {
            return <p>No users available</p>;
        }

        return (
            <table className="table table-striped" aria-labelledby="tableLabel">
                <thead>
                    <tr>
                        <th>Id</th>
                        <th>PersonId</th>
                        <th>First Name</th>
                        <th>Last Name</th>
                        <th>Birth Date</th>
                        <th>Gender</th>
                    </tr>
                </thead>
                <tbody>
                    {users.map(user =>
                        <tr key={user.id}>
                            <td>{user.id}</td>
                            <td>{user.personId}</td>
                            <td>{user.firstName}</td>
                            <td>{user.lastName}</td>
                            <td>{new Date(user.birthDate).toLocaleDateString()}</td>
                            <td>{user.gender}</td>
                        </tr>
                    )}
                </tbody>
            </table>
        );
    }

    const contents = users.length === 0
        ? <p><em>Forbidded!</em></p>
        : renderUsersTable(users);

    return (
        <div className="users-section">
            <h1 id="tableLabel">Users</h1>
            <p>This component demonstrates fetching data from the server.</p>
            {contents}
        </div>
    );
}

export default UsersInfo;