import { useNavigate } from 'react-router-dom';

const ChallengeButton = () => {
    const navigate = useNavigate();

    const handleChallengeClick = () => {
        navigate('/matchmaking');
    };

    return <button onClick={handleChallengeClick}>Challenge</button>;
};

export default ChallengeButton;