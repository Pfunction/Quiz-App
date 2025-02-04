export interface Answer {
    id: number;
    text: string;
}

export interface Question {
    id: number;
    text: string;
    type: 'Radio' | 'Checkbox' | 'Textbox';
    answers: Answer[];
}

export interface Quiz {
    id: number;
    title: string;
    questions: Question[];
}

export interface SubmissionPayload {
    quizId: number;
    email: string;
    userId: number;
    submissionDate: string;
    answers: {
        questionId: number;
        answerIds: number[];
        text: string | null;
    }[];
}