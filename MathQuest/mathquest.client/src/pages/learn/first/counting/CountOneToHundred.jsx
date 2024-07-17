import { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import './CountOneToHundred.css';
import '../../QuizSection.css';

const getColor = (number) => {
    const colors = ['#FF6633', '#FFB399', '#FF33FF', '#FFFF99', '#00B3E6',
        '#E6B333', '#3366E6', '#999966', '#99FF99', '#B34D4D',
        '#80B300', '#809900', '#E6B3B3', '#6680B3', '#66991A',
        '#FF99E6', '#CCFF1A', '#FF1A66', '#E6331A', '#33FFCC',
        '#66994D', '#B366CC', '#4D8000', '#B33300', '#CC80CC',
        '#66664D', '#991AFF', '#E666FF', '#4DB3FF', '#1AB399',
        '#E666B3', '#33991A', '#CC9999', '#B3B31A', '#00E680',
        '#4D8066', '#809980', '#E6FF80', '#1AFF33', '#999933',
        '#FF3380', '#CCCC00', '#66E64D', '#4D80CC', '#9900B3',
        '#E64D66', '#4DB380', '#FF4D4D', '#99E6E6', '#6666FF'];
    return colors[number % colors.length];
};

const CountingPage = () => {
    const [selectedNumber, setSelectedNumber] = useState(1);

    useEffect(() => {
        const handleKeyDown = (event) => {
            if (event.key === 'ArrowRight' && selectedNumber < 100) {
                setSelectedNumber(selectedNumber + 1);
            } else if (event.key === 'ArrowLeft' && selectedNumber > 1) {
                setSelectedNumber(selectedNumber - 1);
            }
        };

        window.addEventListener('keydown', handleKeyDown);
        return () => {
            window.removeEventListener('keydown', handleKeyDown);
        };
    }, [selectedNumber]);

    const numberWords = [
        "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine",
        "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen",
        "Seventeen", "Eighteen", "Nineteen", "Twenty", "Twenty-One", "Twenty-Two",
        "Twenty-Three", "Twenty-Four", "Twenty-Five", "Twenty-Six", "Twenty-Seven",
        "Twenty-Eight", "Twenty-Nine", "Thirty", "Thirty-One", "Thirty-Two",
        "Thirty-Three", "Thirty-Four", "Thirty-Five", "Thirty-Six", "Thirty-Seven",
        "Thirty-Eight", "Thirty-Nine", "Forty", "Forty-One", "Forty-Two",
        "Forty-Three", "Forty-Four", "Forty-Five", "Forty-Six", "Forty-Seven",
        "Forty-Eight", "Forty-Nine", "Fifty", "Fifty-One", "Fifty-Two",
        "Fifty-Three", "Fifty-Four", "Fifty-Five", "Fifty-Six", "Fifty-Seven",
        "Fifty-Eight", "Fifty-Nine", "Sixty", "Sixty-One", "Sixty-Two",
        "Sixty-Three", "Sixty-Four", "Sixty-Five", "Sixty-Six", "Sixty-Seven",
        "Sixty-Eight", "Sixty-Nine", "Seventy", "Seventy-One", "Seventy-Two",
        "Seventy-Three", "Seventy-Four", "Seventy-Five", "Seventy-Six",
        "Seventy-Seven", "Seventy-Eight", "Seventy-Nine", "Eighty", "Eighty-One",
        "Eighty-Two", "Eighty-Three", "Eighty-Four", "Eighty-Five", "Eighty-Six",
        "Eighty-Seven", "Eighty-Eight", "Eighty-Nine", "Ninety", "Ninety-One",
        "Ninety-Two", "Ninety-Three", "Ninety-Four", "Ninety-Five", "Ninety-Six",
        "Ninety-Seven", "Ninety-Eight", "Ninety-Nine", "One Hundred"
    ];

    return (
        <div className="counting">
            <h1>Counting 1 to 100</h1>
            <div className="numbers-section">
                {Array.from({ length: 10 }, (_, i) => (
                    <div key={i} className="number-row">
                        {Array.from({ length: 10 }, (_, j) => {
                            const number = i * 10 + j + 1;
                            return (
                                <div key={number} className="number-container">
                                    <div
                                        className="number"
                                        style={{ backgroundColor: getColor(number) }}
                                        onClick={() => setSelectedNumber(number)}
                                    >
                                        {number}
                                    </div>
                                    <div className="number-word">
                                        {numberWords[number]}
                                    </div>
                                </div>
                            );
                        })}
                    </div>
                ))}
            </div>
            <div className="number-words">
                {selectedNumber !== null && (
                    <>
                        <h2>{selectedNumber} - {numberWords[selectedNumber]}</h2>
                        <p>Examples:</p>
                        <p>{`${selectedNumber} apple${selectedNumber !== 1 ? 's' : ''}`}</p>
                        <div className="apples">
                            {Array.from({ length: selectedNumber }, (_, i) => (
                                <span key={i} role="img" aria-label="apple">🍎</span>
                            )).reduce((acc, curr, idx) => {
                                const lineIdx = Math.floor(idx / 10);
                                if (!acc[lineIdx]) acc[lineIdx] = [];
                                acc[lineIdx].push(curr);
                                return acc;
                            }, []).map((line, idx) => (
                                <div key={idx} className="apple-line">
                                    {line}
                                </div>
                            ))}
                        </div>
                    </>
                )}
            </div>
            <div className="interactive-graph">
                <h2>Interactive Number Line</h2>
                <p>Use the slider or the <b>&lt;</b> and <b>&gt;</b> arrow keys to navigate through numbers.</p>
                <input
                    type="range"
                    min="1"
                    max="100"
                    value={selectedNumber}
                    onChange={(e) => setSelectedNumber(parseInt(e.target.value))}
                />
            </div>
            <div className="quiz-section">
                <div className="quiz-title">Quizzes</div>
                <div className="quiz-links">
                    <Link className="quiz-link" to="/learn/first-grade/count/counting-quiz">Practice Counting 1 to 100</Link>
                    <Link className="quiz-link" to="/learn/first-grade/count/before-after-quiz">Learn Which numbers comes before and which after</Link>
                </div>
            </div>
        </div>
    );
};

export default CountingPage;