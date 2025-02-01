import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';

import QuizStepper from '../components/QuizStepper';

import { 
  Container, 
  Paper, 
  Typography, 
  TextField, 
  Radio, 
  RadioGroup, 
  FormControlLabel, 
  Checkbox,
  Button, 
  Box,
  Divider
} from '@mui/material';


interface Answer {
  id: number;
  text: string;
}

interface Question {
  id: number;
  text: string;
  type: 'Radio' | 'Checkbox' | 'Textbox';
  answers: Answer[];
}

interface Quiz {
  id: number;
  title: string;
  questions: Question[];
}

interface SubmissionPayload {
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

const QuizPage: React.FC = () => {
  const navigate = useNavigate();
  const [quiz, setQuiz] = useState<Quiz | null>(null);
  const [email, setEmail] = useState<string>('');
  const [answers, setAnswers] = useState<Record<number, string[]>>({});
  const [currentPage, setCurrentPage] = useState<number>(-1);

  const isValidEmail = (email: string) => {
    const emailRegex = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
    return emailRegex.test(email);
  };
  

  useEffect(() => {
    const fetchQuiz = async () => {
      try {
        const response = await axios.get<Quiz[]>('https://localhost:5001/api/quizzes', { 
          withCredentials: true, 
          headers: { 'Content-Type': 'application/json' } 
        });

        console.log('Fetch Quiz Response:', response.data);
        if (response.data.length > 0) {
          setQuiz(response.data[0]);
        } else {
          console.error('No quizzes found');
        }
      } catch (error) {
        console.error('Error fetching quizzes:', error);
      }
    };

    fetchQuiz();
  }, []);

  useEffect(() => {
    const handleKeyPress = (e: KeyboardEvent) => {
      if (e.key === 'Enter') {
        e.preventDefault(); 
  
        if (currentPage === -1) {

          if (email && isValidEmail(email)) {
            setCurrentPage(0); 
          }
        } else {
        
          if (validateAnswer()) {
            if (currentPage < Math.ceil(quiz?.questions.length || 0) - 1) {
              setCurrentPage(prev => prev + 1);
            } else {
              handleSubmit();
            }
          } else {
            alert('Please answer the question before proceeding.');
          }
        }
      }
    };
  
    window.addEventListener('keydown', handleKeyPress);
  
    return () => {
      window.removeEventListener('keydown', handleKeyPress);
    };
  }, [currentPage, quiz, answers, email]); 

  const handleChange = (questionId: number, answer: string[]) => {
    setAnswers(prev => ({ ...prev, [questionId]: answer }));
  };

  const validateAnswer = (): boolean => {
    const currentQuestion = quiz?.questions[currentPage];
    if (!currentQuestion) return false;
  
    const currentAnswer = answers[currentQuestion.id];
    if (currentQuestion.type === 'Radio') { 
      return currentAnswer && currentAnswer.length > 0;
    }
    if (currentQuestion.type === 'Checkbox') { 
      return currentAnswer && currentAnswer.length > 0;
    }
    if (currentQuestion.type === 'Textbox') {
      return currentAnswer && currentAnswer[0].trim() !== '';
    }
    return false;
  };

  const handleSubmit = async () => {
    if (!quiz) return;
  
    const submission: SubmissionPayload = {
      quizId: quiz.id,
      email,
      userId: 0, 
      submissionDate: new Date().toISOString(),
      answers: quiz.questions.map((question) => {
        const answer = answers[question.id] || [];
        return {
          questionId: question.id,
          answerIds: question.type === 'Textbox' ? [] : answer.map(Number),
          text: question.type === 'Textbox' ? answer[0] || '' : null, 
        };
      }),
    };
  
    try {
      console.log('Submission payload:', JSON.stringify(submission, null, 2)); 
      const response = await axios.post('https://localhost:5001/api/quizzes/submit', submission);
      console.log('Submission response:', response.data); 
      navigate('/highscores');
    } catch (error: any) {
      console.error('Failed submission payload:', error.response?.data); 
      alert('Failed to submit quiz. Please try again.');
    }
  };

  const renderQuestionType = (question: Question) => {
    switch (question.type) {
      case 'Radio':
        return (
          <RadioGroup 
            name={`question-${question.id}`} 
            value={answers[question.id]?.[0] || ''}
            onChange={(e) => handleChange(question.id, [e.target.value])}
          >
            {question.answers.map((answer) => (
              <FormControlLabel 
                key={answer.id} 
                value={answer.id.toString()} 
                control={<Radio />} 
                label={answer.text}
              />
            ))}
          </RadioGroup>
        );
      case 'Checkbox':
        return question.answers.map((answer) => (
          <FormControlLabel 
            key={answer.id}
            control={
              <Checkbox 
                checked={answers[question.id]?.includes(answer.id.toString()) || false}
                onChange={(e) => {
                  const currentAnswers = answers[question.id] || [];
                  const newAnswers = e.target.checked 
                    ? [...currentAnswers, answer.id.toString()]
                    : currentAnswers.filter(id => id !== answer.id.toString());
                  handleChange(question.id, newAnswers);
                }}
              />
            }
            label={answer.text} 
          />
        ));
      case 'Textbox':
        return (
          <TextField 
            fullWidth 
            variant="outlined" 
            label="Your Answer"
            value={answers[question.id]?.[0] || ''}
            onChange={(e) => handleChange(question.id, [e.target.value])}
          />
        );
      default:
        return null;
    }
  };

  if (!quiz) return <Typography>Loading...</Typography>;

  const QUESTIONS_PER_PAGE = 1;
  const totalPages = Math.ceil(quiz.questions.length / QUESTIONS_PER_PAGE);

  if (currentPage === -1) {
    return (
      <Container maxWidth="md" sx={{ pb: 8 }}>
        <Paper elevation={3} sx={{ p: 4, mt: 4, textAlign: 'center' }}>
          <Typography variant="h3" gutterBottom>{quiz.title}</Typography>
          <Typography variant="body1" paragraph sx={{ mb: 3 }}>
            Welcome to the quiz! This test consists of {quiz.questions.length} questions that test your general knowledge! Good luck!
          </Typography>
                Type your Email to track your score.
          <TextField
            fullWidth
            label="Your Email"
            variant="outlined"
            margin="normal"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            sx={{ mb: 3 }}
            error={!isValidEmail(email) && email.length > 0}
            helperText={isValidEmail(email) || email.length === 0 ? '' : 'Invalid email address'}
          />
          <Button
            variant="contained"
            color="primary"
            size="large"
            disabled={!email || !isValidEmail(email)}
            onClick={() => setCurrentPage(0)}
          >
            Start Quiz
          </Button>
        </Paper>
      </Container>
    );
  }

  const startIndex = currentPage * QUESTIONS_PER_PAGE;
  const endIndex = startIndex + QUESTIONS_PER_PAGE;
  const currentQuestions = quiz.questions.slice(startIndex, endIndex);

  return (
    <Box sx={{ 
      minHeight: '100vh',
      display: 'flex',
      flexDirection: 'column',
      pb: '80px',
      position: 'relative'
    }}>
      <Container maxWidth="md" sx={{ flex: 1 }}>
        <Paper elevation={3} sx={{ p: 3, mt: 4 }}>
          <Typography variant="h4" gutterBottom>{quiz.title}</Typography>
          
          <Box sx={{ display: 'flex', flexDirection: 'column', gap: 3 }}>
            {currentQuestions.map((question, index) => (
              <Box
                key={question.id}
                sx={{
                  border: '1px solid #e0e0e0',
                  borderRadius: 2,
                  p: 2,
                  backgroundColor: 'white'
                }}
              >
                <Typography variant="h6" sx={{ mb: 2 }}>
                  {startIndex + index + 1}. {question.text}
                </Typography>
                {renderQuestionType(question)}
              </Box>
            ))}
          </Box>

          <Divider
              sx={{
                my: 3,
                width: '0',
                maxWidth: '300px', 
                marginX: 'auto',
              }}
            />


          <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
            <Button
              variant="outlined"
              disabled={currentPage === 0}
              onClick={() => setCurrentPage(prev => Math.max(0, prev - 1))}
            >
              Previous
            </Button>
            <Typography>
              Page {currentPage + 1} of {totalPages}
            </Typography>
            <Button
              variant="contained"
              color="primary"
              onClick={() => {
                if (validateAnswer()) {
                  if (currentPage < totalPages - 1) {
                    setCurrentPage(prev => prev + 1);
                  } else {
                    handleSubmit();
                  }
                } else {
                  alert('Please answer the question before proceeding.');
                }
              }}
            >
              {currentPage < totalPages - 1 ? 'Next' : 'Submit'}
            </Button>
          </Box>
        </Paper>
      </Container>
  

      <QuizStepper currentPage={currentPage} totalPages={totalPages} />
    </Box>
  );
};

export default QuizPage;
