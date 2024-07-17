import customAxios from "./customAxios";

const StorePoints = async (points) => {
    try {
        await customAxios.post('api/leaderboard/user/store-points', { Points: points });
    } catch (error) {
        console.error('error occured while adding user score', error);
    }
};

export default StorePoints;