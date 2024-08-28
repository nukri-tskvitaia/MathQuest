import CommunitySignalRChat from './CommunitySignalRChat';
import styles from './CommunityChatPage.module.css';

function CommunityChatPage() {
    return (
        <div className={styles.chatSection}>
            <main className={styles.signalrChatHeader}>
                <h1>Community Chat</h1>
                <CommunitySignalRChat />
            </main>
        </div>
    );
}

export default CommunityChatPage;