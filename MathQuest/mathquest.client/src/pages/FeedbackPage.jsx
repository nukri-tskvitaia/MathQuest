import { useState } from 'react';
import customAxios from '../services/customAxios';
import './FeedbackPage.css';
import { useEffect } from 'react';

const FeedbackPage = () => {
    const [feedbackDescription, setFeedbackDescription] = useState('');
    const [feedbackText, setFeedbackText] = useState('');
    const [successMessage, setSuccessMessage] = useState('');
    const [errorMessage, setErrorMessage] = useState('');
    const [currentUserId, setCurrentUserId] = useState(null);

    const validateInput = (description, text) => {
        const descriptionPattern = /^[a-zA-Z0-9 .,'!?]{3,}$/;
        const textPattern = /^[a-zA-Z0-9 .,!'?]{10,}$/;

        if (!descriptionPattern.test(description)) {
            return 'Feedback subject must contain at least 3 characters and only English letters and digits.';
        }

        if (!textPattern.test(text)) {
            return 'Feedback text must contain at least 10 characters and only English letters and digits.';
        }

        return '';
    };

    useEffect(() => {
        fetchCurrentUserId();
    }, []);

    const handleSubmit = async (e) => {
        e.preventDefault();
        setSuccessMessage('');
        setErrorMessage('');

        const feedback = {
            feedbackDescription,
            userId: currentUserId,
            feedbackText,
            feedbackDate: new Date(),
        };

        const validationError = validateInput(feedbackDescription, feedbackText);
        if (validationError) {
            setErrorMessage(validationError);
            return;
        }

        try {
            console.log(feedback);
            await customAxios.post('/api/feedback', feedback);
            setSuccessMessage('Feedback submitted successfully!');
            setFeedbackDescription('');
            setFeedbackText('');
        } catch (error) {
            setErrorMessage('Failed to submit feedback. Please try again later.', error);
        }
    };

    const fetchCurrentUserId = async () => {
        try {
            const response = await customAxios.get('/api/authorization/user-id');
            setCurrentUserId(response.data.userId);
        } catch (error) {
            console.error('Error fetching user ID:', error);
        }
    };

    return (
        <div className="feedback-page">
            <h1>Feedback</h1>
            <form onSubmit={handleSubmit} className="feedback-form">
                <div className="form-group">
                    <label htmlFor="description">*Feedback Subject:</label>
                    <input
                        type="text"
                        id="description"
                        value={feedbackDescription}
                        onChange={(e) => setFeedbackDescription(e.target.value)}
                        required
                    />
                    <small>Tell us in 2-3 words what is your feedback about</small>
                </div>
                <div className="form-group">
                    <label htmlFor="text">*Feedback Text:</label>
                    <textarea
                        id="text"
                        value={feedbackText}
                        onChange={(e) => setFeedbackText(e.target.value)}
                        required
                    ></textarea>
                    <small>Describe your feedback in detail</small>
                </div>
                <button type="submit">Submit Feedback</button>
                {successMessage && <p className="success-message">{successMessage}</p>}
                {errorMessage && <p className="error-message">{errorMessage}</p>}
            </form>
        </div>
    );
};

export default FeedbackPage;