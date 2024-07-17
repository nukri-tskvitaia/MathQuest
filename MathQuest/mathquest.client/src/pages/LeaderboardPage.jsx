import { useState, useEffect } from "react";
import customAxios from "../services/customAxios";
import './LeaderboardPage.css';

const LeaderboardPage = () => {
    const [leaderboard, setLeaderboard] = useState([]);

    const getUserData = async () => {
        try {
            const response = await customAxios.get('api/leaderboard/user/scores');
            console.log(JSON.stringify(response.data));
            setLeaderboard(response.data);
        } catch(error) {
            console.error('error fetching leaderboard data', error);
        }
    };

    useEffect(() => {
        getUserData();
    }, []);

    return (
        <div className="leaderboard-container">
            <h1>Leaderboard</h1>
            <table className="leaderboard-table">
                <thead>
                    <tr>
                        <th>Rank</th>
                        <th>User Name</th>
                        <th>Score</th>
                    </tr>
                </thead>
                <tbody>
                    {leaderboard && leaderboard.map((user, index) => (
                        <tr key={user.userId}>
                            <td>{index + 1}</td>
                            <td>{user.userName}</td>
                            <td>{user.points}</td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
};

export default LeaderboardPage;