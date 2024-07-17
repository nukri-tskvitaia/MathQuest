import { useEffect, useState, useRef } from 'react';
import { useLocation } from 'react-router-dom';
import { confirmRegistration } from '../services/auth';

const ConfirmEmail = () => {
    const [message, setMessage] = useState('');
    const location = useLocation();
    const isConfirmed = useRef(false); // useRef to keep track of confirmation state

    useEffect(() => {
        const searchParams = new URLSearchParams(location.search);
        const email = searchParams.get('email');
        const confirmationToken = searchParams.get('confirmationToken');

        console.log('Component mounted:', { searchParams, email, confirmationToken });

        // Perform confirmation only if it hasn't been confirmed yet
        const confirmRegistrationRequest = async () => {
            if (!isConfirmed.current) {
                isConfirmed.current = true;  // Set the flag to true before making the request
                try {
                    console.log('Sending confirmation request');
                    const response = await confirmRegistration(email, confirmationToken);
                    console.log('Received response:', response);
                    setMessage(response.message);
                } catch (error) {
                    console.error('Error confirming registration:', error);
                    setMessage('Failed to confirm registration');
                    isConfirmed.current = false;  // Reset the flag if there was an error
                }
            } else {
                console.log('Confirmation request already sent, skipping...');
            }
        };

        confirmRegistrationRequest();
    }, [location]);

    return (
        <div className="email-confirmation-section">
            <h1>Email Confirmation</h1>
            <p>{message}</p>
        </div>
    );
};

export default ConfirmEmail;