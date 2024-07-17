import { useState } from 'react';
import '../../Quiz.css';
import StorePoints from '../../../../services/StorePoints';

const LengthQuiz = () => {
    const [questions] = useState(generateQuizQuestions());
    const [currentQuestionIndex, setCurrentQuestionIndex] = useState(0);
    const [userAnswers, setUserAnswers] = useState([]);
    const [showResults, setShowResults] = useState(false);
    const [quizPoints, setQuizPoints] = useState(0);

    const handleAnswerSelect = (selectedAnswer) => {
        const isCorrect = selectedAnswer === questions[currentQuestionIndex].correctAnswer;
        const pointsToAdd = isCorrect ? 5 : 0;
        setQuizPoints(quizPoints + pointsToAdd);
        setUserAnswers([...userAnswers, { questionIndex: currentQuestionIndex, selectedAnswer, isCorrect }]);
        if (currentQuestionIndex < questions.length - 1) {
            setCurrentQuestionIndex(currentQuestionIndex + 1);
        } else {
            setShowResults(true);

            StorePoints(quizPoints + pointsToAdd);
        }
    };

    const restartQuiz = () => {
        setCurrentQuestionIndex(0);
        setUserAnswers([]);
        setShowResults(false);
        setQuizPoints(0);
    };

    return (
        <div className="quiz-container">
            {!showResults && (
                <div className="question-container">
                    <h2>Question {currentQuestionIndex + 1}</h2>
                    <p>{questions[currentQuestionIndex].question}</p>
                    <div className="answers">
                        {questions[currentQuestionIndex].options.map((option, index) => (
                            <button key={index} onClick={() => handleAnswerSelect(option)}>
                                {option}
                            </button>
                        ))}
                    </div>
                </div>
            )}
            {showResults && (
                <div className="results-container">
                    <h2>Quiz Results</h2>
                    <p>Total Points: {quizPoints}</p>
                    <button onClick={restartQuiz}>Restart Quiz</button>
                </div>
            )}
        </div>
    );
};

const generateQuizQuestions = () => {
    const questions = [];
    const lengths = [
        { question: 'What is 1 meter in centimeters?', correctAnswer: '100 cm', options: ['50 cm', '100 cm', '200 cm', '10 cm'] },
        { question: 'How many millimeters are in a centimeter?', correctAnswer: '10 mm', options: ['10 mm', '100 mm', '1 mm', '1000 mm'] },
        { question: 'What is 50 cm in meters?', correctAnswer: '0.5 m', options: ['5 m', '50 m', '0.5 m', '0.05 m'] },
        { question: 'How many centimeters are in 2 meters?', correctAnswer: '200 cm', options: ['20 cm', '200 cm', '100 cm', '50 cm'] },
        { question: 'What is 10 millimeters in centimeters?', correctAnswer: '1 cm', options: ['1 cm', '10 cm', '0.1 cm', '100 cm'] },
    ];

    for (let i = 0; i < lengths.length; i++) {
        questions.push(lengths[i]);
    }
    return questions;
};

export default LengthQuiz;