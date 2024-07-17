import CommunitySignalRChat from './CommunitySignalRChat';
import './CommunityChatPage.css';

function CommunityChatPage() {
    return (
        <div className="chat-section">
            <main className="signalr-chat-header">
                <h1>Community Chat</h1>
                <CommunitySignalRChat />
            </main>
        </div>
    );
}

export default CommunityChatPage;