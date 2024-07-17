import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import customAxios from '../../services/customAxios';
import './FriendsPage.css';

const FriendsPage = () => {
    const [friends, setFriends] = useState([]);
    const [pendingRequests, setPendingRequests] = useState([]);
    const [searchResult, setSearchResult] = useState(null);
    const [searchQuery, setSearchQuery] = useState('');
    const [relationshipStatus, setRelationshipStatus] = useState(null);
    const [hoveredFriendUserName, setHoveredFriendUserName] = useState(null);
    const [isLoading, setIsLoading] = useState(false);
    const [searchInitiated, setSearchInitiated] = useState(false);

    // for message
    const navigate = useNavigate();

    useEffect(() => {
        fetchFriends();
        fetchPendingRequests();
    }, []);

    const fetchFriends = async () => {
        try {
            const response = await customAxios.get('/api/friend/search/friends');
            setFriends(response.data.friends);
        } catch (error) {
            console.error('Error fetching friends', error);
        }
    };

    const fetchPendingRequests = async () => {
        try {
            const response = await customAxios.get('/api/friend/pending-requests');
            setPendingRequests(response.data.requests);
        } catch (error) {
            console.error('Error fetching pending requests', error);
        }
    };

    const handleSearch = async () => {
        setIsLoading(true);
        try {
            const response = await customAxios.get(`/api/friend/search/user`, {
                params: {
                    username: searchQuery
                }
            });
            if (response.status === 200) {
                setSearchResult(response.data.userInfo);
                setRelationshipStatus(response.data.status);
                setSearchInitiated(true);
                setIsLoading(false);
            }
        } catch (error) {
            if (error.response && error.response.status === 404) {
                setSearchResult(null);
                setRelationshipStatus(null);
            }
            setSearchInitiated(true);
            setIsLoading(false);
            console.error('Error searching for user', error);
        }
    };

    const handleAddFriend = async (username) => {
        try {
            await customAxios.post('/api/friend/add/friend', { userName: username });
            setRelationshipStatus("Pending");
        } catch (error) {
            setRelationshipStatus("Failed");
            console.error('Error adding friend', error);
        }
    };

    const handleUnfriend = async (username) => {
        try {
            await customAxios.post('/api/friend/remove/friend', { userName: username });
            console.log(username);
            setRelationshipStatus("None");
            fetchFriends();
        } catch (error) {
            console.error('Error removing friend', error);
        }
    };

    const handleConfirmRequest = async (username) => {
        try {
            await customAxios.post('/api/friend/confirm', { userName: username });
            fetchFriends();
            fetchPendingRequests();
        } catch (error) {
            console.error('Error confirming request', error);
        }
    };

    const handleRejectRequest = async (username) => {
        try {
            await customAxios.post('/api/friend/reject', { userName: username });
            fetchPendingRequests();
        } catch (error) {
            console.error('Error rejecting request', error);
        }
    };

    // for message
    const handleMessage = async (friend) => {
        navigate('/messages', { state: { friend } });
    };

    return (
        <div className="friends-page">
            <h1>Friends Page</h1>
            <div>
                <div className="search-container">
                    <input
                        type="text"
                        value={searchQuery}
                        onChange={(e) => setSearchQuery(e.target.value)}
                        placeholder="Search username"
                        className="search-input"
                    />
                    <button onClick={handleSearch} className="search-button" disabled={isLoading}>
                        {isLoading ? 'Searching...' : 'Search'}
                    </button>
                </div>
                <ul className="friend-requests-list">
                    {searchInitiated && !searchResult && (
                        <p>User with username &quot;{searchQuery}&quot; not found.</p>
                    )}
                    {searchResult && (
                        <li key={searchResult?.userName} className="search-result-item">
                            <img
                                src={searchResult?.profilePictureUrl}
                                alt={`${searchResult?.userName}'s profile`}
                                className="profile-picture"
                            />
                            <span className="user-name">{searchResult?.userName}</span>
                            {relationshipStatus === 'Pending' ? (
                                <span className="status pending">Request Sent</span>
                            ) : relationshipStatus === 'Failed' ? (
                                <span className="status failed">Request Failed</span>
                            ) : relationshipStatus === 'Approved' ? (
                                <div>
                                    <div
                                        onMouseEnter={() => setHoveredFriendUserName(searchResult.userName)}
                                        onMouseLeave={() => setHoveredFriendUserName(null)}
                                        className={`friend-status ${hoveredFriendUserName === searchResult.userName ? 'hovered' : ''}`}
                                    >
                                        Friends
                                        {hoveredFriendUserName === searchResult.userName && (
                                            <button onClick={() => handleUnfriend(searchResult.userName)}>
                                                Unfriend
                                            </button>
                                        )}
                                    </div>

                                </div>
                            ) : (
                                <button onClick={() => handleAddFriend(searchResult.userName)}>Add Friend</button>
                            )}
                        </li>
                    )}
                </ul>
            </div>
            <h2>Friends</h2>
            <ul className="friend-requests-list">
                {friends.map(friend => (
                    <li key={friend.userName} className="friend-request-item"
                        onMouseEnter={() => setHoveredFriendUserName(friend.userName)}
                        onMouseLeave={() => setHoveredFriendUserName(null)}
                    >
                        <div className="profile-picture">
                            <img
                                src={friend.profilePicture}
                                alt={`${friend.userName}'s profile`}
                            />
                        </div>
                        <div className="request-info">
                            <span className="user-name">{friend.userName}</span>
                            <div className={`friend-status ${hoveredFriendUserName === friend.userName ? 'hovered' : ''}`}>
                                Friends
                                {hoveredFriendUserName === friend.userName && (
                                    <button onClick={() => handleUnfriend(friend.userName)}>
                                        Unfriend
                                    </button>
                                )}
                            </div>
                            <div>
                                <button onClick={() => handleMessage(friend)}>Message</button>
                            </div>
                        </div>
                    </li>
                ))}
            </ul>
            <h2>Pending Friend Requests</h2>
            <ul className="friend-requests-list">
                {pendingRequests.map(request => (
                    <li key={request.userName} className="friend-request-item">
                        <div className="profile-picture">
                            <img
                                src={request.profilePicture}
                                alt={`${request.userName}'s profile`}
                                style={{ width: '100%', height: 'auto', borderRadius: '50%' }}
                            />
                        </div>
                        <div className="request-info">
                            <span className="user-name">{request.userName}</span>
                            <div className="button-container">
                                <button onClick={() => handleConfirmRequest(request.userName)}>Confirm</button>
                                <button onClick={() => handleRejectRequest(request.userName)}>Reject</button>
                            </div>
                        </div>
                    </li>
                ))}
            </ul>
        </div>
    );
};

export default FriendsPage;