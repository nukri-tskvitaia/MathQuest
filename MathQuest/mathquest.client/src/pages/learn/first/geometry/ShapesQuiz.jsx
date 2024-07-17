import { useState } from 'react';
import '../../Quiz.css';
import StorePoints from '../../../../services/StorePoints';

const ShapesQuiz = () => {
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
    const shapes = [
        { question: 'What shape has 3 sides?', correctAnswer: 'Triangle', options: ['Square', 'Rectangle', 'Triangle', 'Circle'] },
        { question: 'What shape has 4 equal sides and 4 right angles?', correctAnswer: 'Square', options: ['Triangle', 'Rectangle', 'Square', 'Pentagon'] },
        { question: 'What shape is round and has no sides?', correctAnswer: 'Circle', options: ['Square', 'Triangle', 'Circle', 'Rectangle'] },
        { question: 'What shape has 4 sides but only opposite sides are equal?', correctAnswer: 'Rectangle', options: ['Square', 'Triangle', 'Rectangle', 'Pentagon'] },
        { question: 'What shape has no angles?', correctAnswer: 'Circle', options: ['Square', 'Triangle', 'Circle', 'Rectangle'] },
    ];

    for (let i = 0; i < shapes.length; i++) {
        questions.push(shapes[i]);
    }
    return questions;
};

export default ShapesQuiz;