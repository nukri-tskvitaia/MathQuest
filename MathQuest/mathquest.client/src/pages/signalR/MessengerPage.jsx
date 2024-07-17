import { useEffect, useState, useRef } from 'react';
import CustomAxios from '../../services/customAxios';
import * as signalR from '@microsoft/signalr';
import { useLocation } from 'react-router-dom';
import './MessengerPage.css';

const MessengerPage = () => {
    const location = useLocation();
    const [currentUserId, setCurrentUserId] = useState(null);
    const [recentMessages, setRecentMessages] = useState([]);
    const [currentMessage, setCurrentMessage] = useState('');
    const [selectedConversation, setSelectedConversation] = useState(null);
    const hubConnection = useRef(null);

    if (recentMessages) {
        recentMessages.map((conversation) => {
            console.log(JSON.stringify(conversation.friendData.profilePictureUrl));
        }
        );
    }

    useEffect(() => {
        fetchCurrentUserId();
    }, []);

    useEffect(() => {
        if (currentUserId) {
            fetchRecentMessages();
            setupSignalRConnection();
        }

        return () => {
            if (hubConnection.current) {
                hubConnection.current.stop();
            }
        };
    }, [currentUserId]);

    useEffect(() => {
        if (location.state && location.state.friend && !location.state.preventCheckForExistingConversation) {
            checkForExistingConversation(location.state.friend);
        }

    }, [location.state, recentMessages]);

    const fetchCurrentUserId = async () => {
        try {
            const response = await CustomAxios.get('/api/authorization/user-id');
            setCurrentUserId(response.data.userId);
        } catch (error) {
            console.error('Error fetching user ID:', error);
        }
    };

    const fetchRecentMessages = async () => {
        try {
            const response = await CustomAxios.get('/api/message/users');
            const messagesWithFriends = await Promise.all(response.data.map(async conversation => {
                const friendId = conversation[0].receiverId !== currentUserId ? conversation[0].receiverId : conversation[0].senderId;
                const friendData = await fetchFriendData(friendId);
                return { friendId, friendData, messages: conversation };
            }));
            setRecentMessages(messagesWithFriends);
        } catch (error) {
            console.error('Error fetching recent messages:', error);
        }
    };

    const fetchFriendData = async (friendId) => {
        try {
            const response = await CustomAxios.get(`/api/friend/search/user/${friendId}`);
            return response.data.userInfo;
        } catch (error) {
            console.error('Error fetching friend data:', error);
        }
    };

    const setupSignalRConnection = () => {
        hubConnection.current = new signalR.HubConnectionBuilder()
            .withUrl('https://localhost:5001/messageshub')
            .withAutomaticReconnect()
            .build();

        hubConnection.current.start().catch(err => console.error('SignalR connection error:', err));

        hubConnection.current.on('ReceiveMessage', handleReceiveMessage);
    };

    const handleReceiveMessage = (messageModel) => {
        const friendId = messageModel.receiverId !== currentUserId ? messageModel.receiverId : messageModel.senderId;

        setRecentMessages(prevRecentMessages => {
            const existingConversationIndex = prevRecentMessages.findIndex(conv => conv.friendId === friendId);
            if (existingConversationIndex !== -1) {
                const updatedRecentMessages = [...prevRecentMessages];
                updatedRecentMessages[existingConversationIndex] = {
                    ...updatedRecentMessages[existingConversationIndex],
                    messages: [messageModel, ...updatedRecentMessages[existingConversationIndex].messages]
                };
                return updatedRecentMessages;
            } else {
                fetchFriendData(friendId).then(friendData => {
                    const newConversation = { friendId, friendData, messages: [messageModel] };
                    setRecentMessages(prev => [newConversation, ...prev]);
                });
                return prevRecentMessages;
            }
        });

        setSelectedConversation(prevSelectedConversation => {
            if (prevSelectedConversation && prevSelectedConversation.friendId === friendId) {
                return {
                    ...prevSelectedConversation,
                    messages: [messageModel, ...prevSelectedConversation.messages]
                };
            }
            return prevSelectedConversation;
        });
    };

    const sendMessage = async () => {
        try {
            if (!selectedConversation || !hubConnection.current) {
                console.error('No conversation selected or SignalR connection not established.');
                return;
            }
            const receiverUserName = selectedConversation.friendData.userName;
            await hubConnection.current.invoke('SendMessageAsync', receiverUserName, currentMessage);
            setCurrentMessage('');

            // Temporarily set the flag to prevent checkForExistingConversation
            location.state = { ...location.state, preventCheckForExistingConversation: true };
        } catch (error) {
            console.error('Error sending message:', error);
        }
    };

    const checkForExistingConversation = (friend) => {
        const existingConversation = recentMessages.find(convo => convo.friendData.userName === friend.userName);
        if (existingConversation) {
            setSelectedConversation(existingConversation);
        } else {
            const newConversation = { friendId: friend.id, friendData: friend, messages: [] };
            setSelectedConversation(newConversation);
            setRecentMessages(prev => [newConversation, ...prev]);
        }
    };

    const handleConversationSelect = (conversation) => {
        setSelectedConversation(conversation);
    };

    return (
        <div className="messenger-container">
            <h1>Messenger</h1>
            <div className="recent-messages">
                <h2>Recent Messages</h2>
                {recentMessages.map((conversation, index) => (
                    <div key={index} className="conversation-item" onClick={() => handleConversationSelect(conversation)}>
                        <div className="friend-info">
                            <img src={selectedConversation?.friendData?.profilePictureUrl} alt={`${conversation.friendData.userName}'s avatar`} className="friend-avatar" />
                            <p className="friend-username">{conversation.friendData.userName}</p>
                        </div>
                        <p className="latest-message">{conversation?.messages[0]?.text}</p>
                    </div>
                ))}
            </div>
            {selectedConversation && (
                <div className="conversation-details">
                    <div className="conversation-header">
                        <img src={selectedConversation?.friendData?.profilePictureUrl} alt={`${selectedConversation.friendData.userName}'s avatar`} className="friend-avatar-small" />
                        <h2>{selectedConversation.friendData.userName}</h2>
                    </div>
                    {selectedConversation.messages.slice().reverse().map((message, index) => (
                        <div key={index} className="message-container">
                            <div className="message-date">
                                {new Date(message.sentDate).toLocaleString()}
                            </div>
                            <div className={`message-item ${message.senderId === currentUserId ? 'message-right' : 'message-left'}`}>
                                <div className="message-text">
                                    {message?.text}
                                </div>
                            </div>
                        </div>
                    ))}
                    <div className="message-input">
                        <input type="text" value={currentMessage} onChange={(e) => setCurrentMessage(e.target.value)} />
                        <button onClick={sendMessage}>Send</button>
                    </div>
                </div>
            )}
        </div>
    );
};

export default MessengerPage;
