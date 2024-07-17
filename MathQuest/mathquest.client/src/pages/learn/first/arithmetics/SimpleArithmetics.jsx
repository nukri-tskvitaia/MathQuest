import { Link } from 'react-router-dom';
import './SimpleArithmetics.css';
import '../../QuizSection.css';

const SimpleArithmetics = () => {
    return (
        <div className="simple-arithmetics">
            <h1>Simple Arithmetics</h1>

            <div className="topic">
                <h2>Addition and Subtraction within 20</h2>
                <p>
                    Addition means putting numbers together. For example, if you have 3 apples and you get 2 more, you have 5 apples in total. Subtraction means taking away. For example, if you have 5 apples and you eat 2, you have 3 apples left.
                </p>
                <p>
                    <b>Addition:</b> 7 + 5 = 12<br />
                    <span className="example">Example: You have 7 apples 🍎🍎🍎🍎🍎🍎🍎 and you get 5 more 🍎🍎🍎🍎🍎. Now you have 12 apples 🍎🍎🍎🍎🍎🍎🍎🍎🍎🍎🍎🍎.</span>
                </p>
                <p>
                    <b>Subtraction:</b> 14 - 6 = 8<br />
                    <span className="example">Example: You have 14 apples 🍎🍎🍎🍎🍎🍎🍎🍎🍎🍎🍎🍎🍎🍎 and you eat 6 🍎🍎🍎🍎🍎🍎. Now you have 8 apples 🍎🍎🍎🍎🍎🍎🍎🍎.</span>
                </p>
            </div>

            <div className="topic">
                <h2>The Equal Sign (=)</h2>
                <p>
                    The equal sign (=) means that what is on one side of the sign is the same as what is on the other side. It is like a balance scale. If you have 3 apples on one side and 3 apples on the other side, they are equal.
                </p>
                <p>
                    <span className="example">Example: 5 + 3 = 8 means that 5 apples and 3 apples together make 8 apples. It is the same as saying 🍎🍎🍎🍎🍎 + 🍎🍎🍎 = 🍎🍎🍎🍎🍎🍎🍎🍎.</span>
                </p>
            </div>

            <div className="topic">
                <h2>Adding 1s and 10s</h2>
                <p>
                    Adding 1s means increasing the number by 1. Adding 10s means increasing the number by 10.
                </p>
                <p>
                    <b>Adding 1s:</b> 6 + 1 = 7<br />
                    <span className="example">Example: You have 6 apples 🍎🍎🍎🍎🍎🍎 and you get 1 more 🍎. Now you have 7 apples 🍎🍎🍎🍎🍎🍎🍎.</span>
                </p>
                <p>
                    <b>Adding 10s:</b> 20 + 10 = 30<br />
                    <span className="example">Example: You have 20 apples 🍎🍎🍎🍎🍎🍎🍎🍎🍎🍎🍎🍎🍎🍎🍎🍎🍎🍎🍎🍎 and you get 10 more 🍎🍎🍎🍎🍎🍎🍎🍎🍎🍎. Now you have 30 apples 🍎🍎🍎🍎🍎🍎🍎🍎🍎🍎🍎🍎🍎🍎🍎🍎🍎🍎🍎🍎🍎🍎🍎🍎🍎🍎🍎🍎🍎🍎.</span>
                </p>
            </div>
            <div className="quiz-section">
                <div className="quiz-title">Quizzes</div>
                <div className="quiz-links">
                    <Link className="quiz-link" to="/learn/first-grade/arithmetics/addition-quiz">Addition</Link>
                    <Link className="quiz-link" to="/learn/first-grade/arithmetics/subtraction-quiz">Subtraction</Link>
                    <Link className="quiz-link" to="/learn/first-grade/arithmetics/adding-1s-10s-quiz">Adding 1s and 10s</Link>
                </div>
            </div>
        </div>
    );
};

export default SimpleArithmetics;