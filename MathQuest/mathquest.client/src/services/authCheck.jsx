import customAxios from "./customAxios";

export const checkAuth = async (setIsAuthenticated, setRoles) => {
    try {
        const response = await customAxios.get(`/api/authorization/check-auth`);
        setIsAuthenticated(true);
        setRoles(response.data.roles);
    } catch (error) {
        setIsAuthenticated(false);
        setRoles([]);
    }
}