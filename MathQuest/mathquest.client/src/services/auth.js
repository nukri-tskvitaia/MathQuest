import customAxios from "./customAxios";

const register = async (formData) => {
    try {
        const response = await fetch(`/api/authorization/register`, {
            method: 'POST',
            body: formData,
            credentials: 'include'
        });

        if (!response.ok) {
            // Get the error message from the response if available
            const errorText = await response.text();
            throw new Error(`Registration failed: ${errorText}`);
        }

        const responseData = await response.json();
        console.log(responseData);

        const { message, errors } = responseData;

        return { success: true, message, errors };
    }
    catch (error) {
        console.error('Error registering:', error);
        return false; // Or handle error in some other way
    }
};

const confirmRegistration = async (email, confirmationToken) => {
    try {

        const data = {
            Email: email,
            ConfirmationToken: confirmationToken
        };
        const response = await customAxios.post('https://localhost:5001/api/authorization/register/confirm', data);

        if (response.status !== 200) {
            throw new Error('Failed to confirm registration');
        }

        return { message: response.data.message };
    } catch (error) {
        console.error('Error confirming registration:', error);
        throw error;
    }
};

const resendConfirmationEmail = async (email) => {
    try {
        const response = await customAxios.post('https://localhost:5001/api/authorization/register/resend-confirmation', email);

        if (response.status == 400) {
            throw new Error('Email already confirmed');
        }

        return { message: response.data.message, confirmationToken: response.data.confirmationToken };
    } catch (error) {
        console.error('Error confirming registration:', error);
        throw error;
    }
};

const login = async (email, password, rememberMe) => {

    try {
        const response = await fetch(`/api/authorization/login`,{
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ email, password, rememberMe }),
            credentials: 'include'
        });

        if (response.ok) {

            const responseData = await response.json();
            console.log(responseData);

            const { message, requires2FA, userEmail, roles } = responseData;

            return { success: true, message, requires2FA, userEmail, roles };
        }

        // Get the error message from the response if available
        const errorText = await response.json();
        const { message } = errorText;
        throw new Error(`Login failed: ${message}`);


    } catch (error) {
        console.error(error);
        return {success: false, message: error.message };
    }
};

const verifyTwoFactor = async ({ verificationCode, email, rememberMe, rememberTwoFactor }) => {
    try {
        const response = await fetch(`/api/authorization/verify-2fa`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ verificationCode, email, rememberMe, rememberTwoFactor }),
            credentials: 'include'
        });

        const responseData = await response.json();
        console.log(responseData);

        const { Message } = responseData;

        if (response.ok) {

            return { Success: true, Message };
        }
        else {
            
            return { Success: false, Message };
        }
    } catch (error) {
        console.error('Error verifying 2FA:', error);
        return { Success: false, Message: error.message };
    }
};

export { register, confirmRegistration, login, verifyTwoFactor, resendConfirmationEmail };