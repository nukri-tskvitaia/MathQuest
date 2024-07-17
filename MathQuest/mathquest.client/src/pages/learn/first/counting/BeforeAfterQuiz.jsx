import { useState } from 'react';
import '../../Quiz.css';
import StorePoints from '../../../../services/StorePoints';

const BeforeAfterQuiz = () => {
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

            console.log(quizPoints + pointsToAdd);
            StorePoints(quizPoints + pointsToAdd);

            // Notify parent component (CountingQuizPage) that quiz is completed
            //onCompleteQuiz(quizPoints);
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
    for (let i = 0; i < 5; i++) {
        const number = Math.floor(Math.random() * 100) + 1;
        const beforeNumber = number - 1;
        const afterNumber = number + 1;

        const correctAnswer = `${beforeNumber} and ${afterNumber}`;
        const options = generateOptions(correctAnswer);

        shuffleArray(options);

        const question = `What comes before and after ${number}?`;
        questions.push({ question, options, correctAnswer });
    }
    return questions;
};

const generateOptions = (correctAnswer) => {
    const options = [];
    options.push(correctAnswer);

    // Generate three incorrect options
    while (options.length < 4) {
        const randomOption = generateRandomOption(correctAnswer);
        if (!options.includes(randomOption)) {
            options.push(randomOption);
        }
    }

    return options;
};

const generateRandomOption = (correctAnswer) => {
    // Generate a random number different from correctAnswer
    let randomOption;
    do {
        randomOption = `${Math.floor(Math.random() * 100) + 1} and ${Math.floor(Math.random() * 100) + 1}`;
    } while (randomOption === correctAnswer);

    return randomOption;
};

const shuffleArray = (array) => {
    for (let i = array.length - 1; i > 0; i--) {
        const j = Math.floor(Math.random() * (i + 1));
        [array[i], array[j]] = [array[j], array[i]];
    }
};

export default BeforeAfterQuiz;