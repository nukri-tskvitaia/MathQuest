import { useState } from 'react';
import '../../Quiz.css';
import StorePoints from '../../../../services/StorePoints';

const Adding1sAnd10sQuiz = () => {
    const [questions] = useState(generateAdding1sAnd10sQuestions());
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

const generateAdding1sAnd10sQuestions = () => {
    const questions = [];
    for (let i = 0; i < 5; i++) {
        const baseNumber = Math.floor(Math.random() * 90) + 1;
        const addValue = Math.random() < 0.5 ? 1 : 10;
        const correctAnswer = baseNumber + addValue;
        const options = generateOptions(correctAnswer);

        shuffleArray(options);

        const question = `What is ${baseNumber} + ${addValue}?`;
        questions.push({ question, options, correctAnswer });
    }
    return questions;
};

const generateOptions = (correctAnswer) => {
    const options = [];
    options.push(correctAnswer);

    while (options.length < 4) {
        const randomOption = Math.floor(Math.random() * 100) + 1;
        if (!options.includes(randomOption)) {
            options.push(randomOption);
        }
    }

    return options;
};

const shuffleArray = (array) => {
    for (let i = array.length - 1; i > 0; i--) {
        const j = Math.floor(Math.random() * (i + 1));
        [array[i], array[j]] = [array[j], array[i]];
    }
};

export default Adding1sAnd10sQuiz;
