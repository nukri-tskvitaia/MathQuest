import { useState } from 'react';
import '../../Quiz.css';
import StorePoints from '../../../../services/StorePoints';

const TimeQuiz = () => {
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
    const times = [
        { question: 'What time is it if the hour hand is on 3 and the minute hand is on 12?', correctAnswer: '3:00', options: ['3:00', '3:30', '12:00', '6:00'] },
        { question: 'What time is it if the hour hand is on 7 and the minute hand is on 6?', correctAnswer: '7:30', options: ['7:00', '7:30', '7:06', '6:30'] },
        { question: 'What time is it if the hour hand is on 12 and the minute hand is on 9?', correctAnswer: '12:45', options: ['12:00', '12:45', '9:00', '6:00'] },
        { question: 'What time is it if the hour hand is on 5 and the minute hand is on 6?', correctAnswer: '5:30', options: ['5:00', '5:30', '6:00', '3:00'] },
        { question: 'What time is it if the hour hand is on 8 and the minute hand is on 12?', correctAnswer: '8:00', options: ['8:00', '8:30', '12:00', '8:15'] },
    ];

    for (let i = 0; i < times.length; i++) {
        questions.push(times[i]);
    }
    return questions;
};

export default TimeQuiz;