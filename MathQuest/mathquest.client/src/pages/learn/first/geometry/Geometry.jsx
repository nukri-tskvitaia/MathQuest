import { Link } from 'react-router-dom';
import './Geometry.css';

const Geometry = () => {
    return (
        <div className="geometry-page">
            <h1>Measurement and Geometry</h1>

            <div className="topic">
                <h2>Length</h2>
                <p>
                    Length is a measure of distance. In the European system, the basic unit of length is the meter (m).
                    Smaller lengths are measured in centimeters (cm) and millimeters (mm).
                </p>
                <div className="ruler-container">
                    <svg width="500" height="60" className="ruler">
                        {/* Ruler background */}
                        <rect width="500" height="60" fill="#f0e68c" />
                        {/* Centimeter and millimeter marks */}
                        {Array.from({ length: 101 }).map((_, i) => {
                            const x = i * 5;
                            return (
                                <line key={i} x1={x} y1={0} x2={x} y2={i % 10 === 0 ? 40 : 20} stroke="black" strokeWidth="2" />
                            );
                        })}
                        {/* Numbers */}
                        {Array.from({ length: 11 }).map((_, i) => {
                            const x = i * 50;
                            return (
                                <text key={i} x={x} y={50} fontSize="12" textAnchor="middle">{i * 10}</text>
                            );
                        })}
                    </svg>
                </div>
                <p>
                    <b>1 meter (m)</b> = 100 centimeters (cm)<br />
                    <b>1 centimeter (cm)</b> = 10 millimeters (mm)
                </p>
            </div>

            <div className="topic">
                <h2>Time</h2>
                <p>
                    Time is measured in seconds, minutes, and hours. Here are the conversions:
                </p>
                <p>
                    <b>1 hour</b> = 60 minutes<br />
                    <b>1 minute</b> = 60 seconds
                </p>
                <div className="clock-container">
                    <svg width="200" height="200" viewBox="0 0 200 200" className="clock">
                        <circle cx="100" cy="100" r="95" stroke="black" strokeWidth="5" fill="white" />
                        <line x1="100" y1="100" x2="100" y2="30" stroke="black" strokeWidth="5" />
                        <line x1="100" y1="100" x2="150" y2="100" stroke="black" strokeWidth="3" />
                        <circle cx="100" cy="100" r="3" fill="black" />
                        {Array.from({ length: 12 }).map((_, i) => {
                            const angle = (i * 30) * (Math.PI / 180);
                            const x1 = 100 + 85 * Math.cos(angle);
                            const y1 = 100 + 85 * Math.sin(angle);
                            const x2 = 100 + 95 * Math.cos(angle);
                            const y2 = 100 + 95 * Math.sin(angle);
                            return (
                                <line key={i} x1={x1} y1={y1} x2={x2} y2={y2} stroke="black" strokeWidth="2" />
                            );
                        })}
                    </svg>
                </div>
                <p>
                    We use clocks and watches to measure time. A day has 24 hours, and each hour is divided into 60 minutes.
                </p>
            </div>

            <div className="topic">
                <h2>Basic Geometric Shapes</h2>
                <p>
                    Geometry is the study of shapes. Here are some basic shapes:
                </p>
                <div className="shapes">
                    <div className="shape">
                        <svg width="100" height="100">
                            <circle cx="50" cy="50" r="40" stroke="black" strokeWidth="3" fill="red" />
                        </svg>
                        <p>Circle</p>
                    </div>
                    <div className="shape">
                        <svg width="100" height="100">
                            <rect width="80" height="80" stroke="black" strokeWidth="3" fill="blue" />
                        </svg>
                        <p>Square</p>
                    </div>
                    <div className="shape">
                        <svg width="120" height="100">
                            <rect width="100" height="60" stroke="black" strokeWidth="3" fill="green" />
                        </svg>
                        <p>Rectangle</p>
                    </div>
                    <div className="shape">
                        <svg width="100" height="100">
                            <polygon points="50,15 90,85 10,85" stroke="black" strokeWidth="3" fill="yellow" />
                        </svg>
                        <p>Triangle</p>
                    </div>
                </div>
            </div>
            <div className="quiz-section">
                <div className="quiz-title">Quizzes</div>
                <div className="quiz-links">
                    <Link className="quiz-link" to="/learn/first-grade/geometry/length-quiz">Length</Link>
                    <Link className="quiz-link" to="/learn/first-grade/geometry/time-quiz">Time</Link>
                    <Link className="quiz-link" to="/learn/first-grade/geometry/shapes-quiz">Shapes</Link>
                </div>
            </div>
        </div>
    );
};

export default Geometry;