import { useEffect, useState } from 'react';
import * as signalR from '@microsoft/signalr';
import './CommunitySignalRChat.css';

const CommunitySignalRChat = () => {
    const [connection, setConnection] = useState(null);
    const [messages, setMessages] = useState([]);
    const [user, setUser] = useState('');
    const [message, setMessage] = useState('');

    useEffect(() => {
        const newConnection = new signalR.HubConnectionBuilder()
            .withUrl(`https://localhost:5001/matchmakinghub`)
            .withAutomaticReconnect()
            .build();

        setConnection(newConnection);
    }, []);

    useEffect(() => {
        if (connection) {
            connection.start()
                .then(result => {
                    console.log('Connected!');

                    connection.on('ReceiveMessage', (user, message) => {
                        const newMessage = `${user}: ${message}`;
                        setMessages(messages => [...messages, newMessage]);
                    });
                })
                .catch(e => console.log('Connection failed: ', e));
        }
    }, [connection]);

    const sendMessage = async () => {
        if (connection && user && message) {
            try {
                await connection.invoke('SendMessagesAsync', user, message);
                setMessage('');
            } catch (e) {
                console.error('Sending message failed: ', e);
            }
        }
    };

    return (
        <div className="chat-container">
            <div className="input-container">
                <input
                    type="text"
                    placeholder="User Name"
                    value={user}
                    onChange={e => setUser(e.target.value)}
                    className="input-field"
                />
                <input
                    type="text"
                    placeholder="Message"
                    value={message}
                    onChange={e => setMessage(e.target.value)}
                    className="input-field"
                />
                <button onClick={sendMessage} className="send-button">Send Message</button>
            </div>
            <ul className="messages-list">
                {messages.map((msg, index) => <li key={index} className="message">{msg}</li>)}
            </ul>
        </div>
    );
};

export default CommunitySignalRChat;