import customAxios from "./customAxios";

const RefreshToken = async () => {
    try {
        const response = await customAxios.post('/api/authorization/refresh');

        if (response.status == 200) {
            const { message, accessToken, roles } = response.data;
            return { success: true, status: 200, message, accessToken, roles };
        }
        else if (response.status == 204) {
            return { success: true, status: 204, message: 'Token still valid' };
        }

        const errorText = response.data?.message || 'Unknown error';
        throw new Error(`Refreshing token failed: ${errorText}`);
    }
    catch (error) {
        console.error('Error registering:', error);
        return { success: false, message: error.message };
    }
}

export default RefreshToken;