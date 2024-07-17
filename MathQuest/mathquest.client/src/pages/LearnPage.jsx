import { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import CustomAxios from '../services/customAxios';
import './LearnPage.css';

const LearnPage = () => {
    const [unlockedGrades, setUnlockedGrades] = useState([1]);
    const [currentGrade, setCurrentGrade] = useState(1);
    const [isTestModalOpen, setIsTestModalOpen] = useState(false);
    const [testResult, setTestResult] = useState(null);

    const getGradeSuffix = (grade) => {
        if (grade === 1) return 'st';
        if (grade === 2) return 'nd';
        if (grade === 3) return 'rd';
        return 'th';
    };

    const getGradeName = (grade) => {
        const gradeNames = [
            'first', 'second', 'third', 'fourth', 'fifth',
            'sixth', 'seventh', 'eighth', 'ninth', 'tenth',
            'eleventh', 'twelfth'
        ];
        return gradeNames[grade - 1];
    };

    useEffect(() => {
        CustomAxios.get('/api/account/user/grade')
            .then(response => {
                const grade = response.data.gradeLevel;
                const gradesToUnlock = [];
                for (let i = 1; i <= grade; i++) {
                    gradesToUnlock.push(i);
                }
                setUnlockedGrades(gradesToUnlock);
                setCurrentGrade(grade);
            })
            .catch(error => {
                console.error('Error fetching user grade:', error);
            });
    }, []);

    const handleTestPass = (grade) => {
        const newUnlockedGrades = [...unlockedGrades, grade + 1];
        setUnlockedGrades(newUnlockedGrades);
    };

    const openTestModal = () => {
        setIsTestModalOpen(true);
    };

    const closeTestModal = () => {
        setIsTestModalOpen(false);
    };

    const handleTestSubmit = () => {
        // Simulate test result
        const result = false; // For example, test passed
        setTestResult(result);
        if (result) {
            handleTestPass(currentGrade);
        }
        closeTestModal();
    };

    return (
        <div className="learn-page">
            <div className="grades-sidebar">
                {Array.from({ length: 12 }, (_, i) => i + 1).map((grade) => (
                    <Link
                        key={grade}
                        to={`/learn/${getGradeName(grade)}-grade`}
                        className={`grade-link ${unlockedGrades.includes(grade) ? 'unlocked' : 'locked'}`}
                        onClick={(e) => !unlockedGrades.includes(grade) && e.preventDefault()}
                    >
                        {grade}{getGradeSuffix(grade)} Grade
                    </Link>
                ))}
            </div>
            <div className="grade-content">
                <h1>{currentGrade}{getGradeSuffix(currentGrade)} Grade Content</h1>
                {unlockedGrades.includes(currentGrade) && (
                    <button onClick={openTestModal} className="test-button">Take Test to Unlock Next Grade</button>
                )}
            </div>

            {isTestModalOpen && (
                <div className="test-modal">
                    <div className="test-modal-content">
                        <h2>General Test for {currentGrade + 1}{getGradeSuffix(currentGrade + 1)} Grade</h2>
                        {/* Simulated test content */}
                        <button onClick={handleTestSubmit} className="modal-button">Submit Test</button>
                        <button onClick={closeTestModal} className="modal-button">Cancel</button>
                    </div>
                </div>
            )}
        </div>
    );
};

export default LearnPage;