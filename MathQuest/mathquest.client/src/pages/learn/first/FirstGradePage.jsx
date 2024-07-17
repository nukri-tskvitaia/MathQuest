import { Link } from 'react-router-dom';
import '../GradePage.css';

const Grade1 = () => {
    return (
        <div className="grade">
            <h1>Grade 1 Mathematics</h1>

            <div className="learn-section">
                <div className="learn-title">Counting</div>
                <div className="learn-links">
                    <Link className="learn-link" to="/learn/first-grade/count">Learn to count from 1 to 100</Link>
                </div>
                <div className="quiz-links">
                    <Link className="quiz-link" to="/learn/first-grade/count/counting-quiz">Quiz: Counting</Link>
                    <Link className="quiz-link" to="/learn/first-grade/count/before-after-quiz">Quiz: Before and After</Link>
                </div>
            </div>
            <div className="learn-section">
                <div className="learn-title">Addition and Subtraction</div>
                <div className="learn-links">
                    <Link className="learn-link" to="/learn/first-grade/arithmetics">Learn Addition and Subtraction within 20</Link>
                </div>
                <div className="quiz-links">
                    <Link className="quiz-link" to="/learn/first-grade/arithmetics/addition-quiz">Quiz: Addition</Link>
                    <Link className="quiz-link" to="/learn/first-grade/arithmetics/subtraction-quiz">Quiz: Subtraction</Link>
                    <Link className="quiz-link" to="/learn/first-grade/arithmetics/adding-1s-10s-quiz">Quiz: Adding 1s and 10s</Link>
                </div>
            </div>
            <div className="learn-section">
                <div className="learn-title">Geometry, Measurement and Time</div>
                <div className="learn-links">
                    <Link className="learn-link" to="/learn/first-grade/geometry">Learn about Geometry, Measurement and Time</Link>
                </div>
                <div className="quiz-links">
                    <Link className="quiz-link" to="/learn/first-grade/geometry/length-quiz">Quiz: Length</Link>
                    <Link className="quiz-link" to="/learn/first-grade/geometry/time-quiz">Quiz: Time</Link>
                    <Link className="quiz-link" to="/learn/first-grade/geometry/shapes-quiz">Quiz: Shapes</Link>
                </div>
            </div>
        </div>
    );
};

export default Grade1;