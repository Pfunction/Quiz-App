import { apiClient } from './config.ts';
import { API_ENDPOINTS } from './endpoints.ts';
import { Quiz, SubmissionPayload } from "./types.ts";

export class QuizService {
    static async fetchQuizzes(): Promise<Quiz[]> {
        try {
            const response = await apiClient.get<Quiz[]>(API_ENDPOINTS.QUIZZES);
            return response.data;
        } catch (error) {
            console.error('Error fetching quizzes:', error);
            throw error;
        }
    }

    static async submitQuiz(submission: SubmissionPayload): Promise<any> {
        try {
            const response = await apiClient.post(API_ENDPOINTS.SUBMIT_QUIZ, submission);
            return response.data;
        } catch (error) {
            console.error('Failed submission:', error);
            throw error;
        }
    }
}