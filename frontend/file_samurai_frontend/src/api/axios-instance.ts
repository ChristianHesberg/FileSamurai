import axios, {AxiosError, AxiosInstance, AxiosResponse, InternalAxiosRequestConfig} from 'axios';
import {API_BASE_URL} from "../constants";
import {NotFoundError} from "../errors/not-found.error";
import {ForbiddenError} from "../errors/forbidden.error";
import {UnauthorizedError} from "../errors/unauthorized.error";
import {InternalServerError} from "../errors/internal-server.error";
import {HttpError} from "../errors/http.error";
import {BadRequestError} from "../errors/bad-request.error";

const axiosInstance: AxiosInstance = axios.create({
    baseURL: API_BASE_URL,
    headers: {
        'Accept': 'application/json'
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
        const status = error.response?.status;
        const data = error.response?.data;

        console.error(`HTTP Request failed with status ${status}: ${error.message}`, {
            url: error.config?.url,
            method: error.config?.method,
            params: error.config?.params,
            data: error.config?.data,
            response: data,
        });

        switch (status) {
            case 400: {
                const error = new BadRequestError(data);
                return rejectError(error);
            }
            case 401:{
                const error = new UnauthorizedError(data);
                return rejectError(error);
            }
            case 403:{
                const error = new ForbiddenError(data);
                return rejectError(error);
            }
            case 404:{
                const error = new NotFoundError(data);
                return rejectError(error);
            }
            case 500:{
                const error = new InternalServerError(data);
                return rejectError(error);
            }
            default:{
                const error = new HttpError('An unexpected error occurred', status, data);
                return rejectError(error);
            }
        }
    }
);

function rejectError(error: HttpError): Promise<never> {
    console.log(error.message);
    return Promise.reject(error);
}

function getJwtToken(): string | null {
    return null;
    //return localStorage.getItem('jwtToken');
}

export default axiosInstance;