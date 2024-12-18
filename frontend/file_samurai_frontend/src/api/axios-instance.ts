import axios, {AxiosError, AxiosInstance, AxiosResponse, InternalAxiosRequestConfig} from 'axios';
import {API_BASE_URL} from "../../constants";

const axiosInstance: AxiosInstance = axios.create({
    baseURL: API_BASE_URL,
    headers: {
        'Accept': 'application/json',
    },
});

axiosInstance.interceptors.request.use(
    (config: InternalAxiosRequestConfig) => {
        const token = getJwtToken();
        if (token) {
            config.headers.Authorization = `Bearer ${token}`;
        }
        return config;
    },
    (error) => {
        return Promise.reject(error);
    }
);

axiosInstance.interceptors.response.use(
    (response: AxiosResponse) => {
        return response;
    },
    (error: AxiosError) => {
        // Extract relevant information from the error
        const status = error.response?.status;
        const data = error.response?.data;

        // Log the error with relevant details
        console.error(`HTTP Request failed with status ${status}: ${error.message}`, {
            url: error.config?.url,
            method: error.config?.method,
            params: error.config?.params,
            data: error.config?.data,
            response: data,
        });
        /*
        // Optionally, you can customize the error handling based on status codes or other criteria
        if (status === 404) {
            console.error('Resource not found.');
        } else if (status === 500) {
            console.error('Internal server error.');
        } else {
            console.error('An unexpected error occurred.');
        }*/

        return Promise.reject();
    }
);


function getJwtToken(): string | null {
    return null;
    //return localStorage.getItem('jwtToken');
}

export default axiosInstance;