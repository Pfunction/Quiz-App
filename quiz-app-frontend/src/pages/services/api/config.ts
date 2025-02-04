import axios from 'axios';
import { API_ENDPOINTS } from './endpoints.ts';

export const apiClient = axios.create({
    baseURL: API_ENDPOINTS.BASE_URL,
    withCredentials: true,
    headers: {
        'Content-Type': 'application/json'
    }
});