import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';

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

const QuizPage = () => {
  const navigate = useNavigate();
  const [quiz, setQuiz] = useState(null);
  const [email, setEmail] = useState('');
  const [answers, setAnswers] = useState({});
  const [currentPage, setCurrentPage] = useState(-1);

  useEffect(() => {
    const fetchQuiz = async () => {
      try {
        const response = await axios.get('https://localhost:5001/api/quizzes', { 
          withCredentials: true, 
          headers: { 'Content-Type': 'application/json' } 
        });

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

  const handleChange = (questionId, answer) => {
    setAnswers(prev => ({ ...prev, [questionId]: answer }));
  };

  const handleSubmit = async () => {
    try {
      const submission = {
        quizId: quiz.id,
        email,
        userId: 0,
        submissionDate: new Date().toISOString(),
        answers: quiz.questions.map((question) => {
          const answer = answers[question.id] || [];
          return {
            questionId: question.id,
            answerIds: question.type === 'textbox' ? [] : (answer || []),
            text: question.type === 'textbox' ? (answer[0] || '') : null
          };
        })
      };

      console.log('Submission payload:', JSON.stringify(submission, null, 2));
      
      const response = await axios.post('https://localhost:5001/api/quizzes/submit', submission);
      navigate('/highscores');
    } catch (error) {
      console.error('Failed submission payload:', error.response?.data);
      alert('Failed to submit quiz. Please try again.');
    }
  };

  const renderQuestionType = (question) => {
    switch (question.type) {
      case 'radio':
        return (
          <RadioGroup 
            name={`question-${question.id}`} 
            value={answers[question.id]?.[0]?.toString() || ''}
            onChange={(e) => {
              const selectedValue = parseInt(e.target.value, 10);
              handleChange(question.id, [selectedValue]);
            }}
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
      case 'checkbox':
        return question.answers.map((answer) => (
          <FormControlLabel 
            key={answer.id}
            control={
              <Checkbox 
                checked={answers[question.id]?.includes(answer.id) || false}
                onChange={(e) => {
                  const currentAnswers = answers[question.id] || [];
                  const newAnswers = e.target.checked 
                    ? [...currentAnswers, answer.id]
                    : currentAnswers.filter(id => id !== answer.id);
                  handleChange(question.id, newAnswers);
                }}
              />
            }
            label={answer.text} 
          />
        ));
        case 'textbox':
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
      <Container maxWidth="md">
        <Paper elevation={3} sx={{ p: 4, mt: 4, textAlign: 'center' }}>
          <Typography variant="h3" gutterBottom>
            {quiz.title}
          </Typography>
          
          <Typography variant="body1" paragraph sx={{ mb: 3 }}>
            Welcome to the quiz! This test consists of {quiz.questions.length} questions 
            across different formats including single answer, multiple choice, and text input. 
            You'll need to provide an email to track your score.
          </Typography>

          <TextField 
            fullWidth 
            label="Your Email" 
            variant="outlined" 
            margin="normal"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            sx={{ mb: 3 }}
          />

          <Button 
            variant="contained" 
            color="primary" 
            size="large"
            disabled={!email}
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
    
    <Container maxWidth="md">
      
      <Paper elevation={3} sx={{ p: 3, mt: 4 }}>
        <Typography variant="h4" gutterBottom>
          {quiz.title}
        </Typography>

        <Box sx={{ 
          display: 'flex', 
          flexDirection: 'column', 
          gap: 3 
        }}>
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

        <Divider sx={{ my: 3 }} />

        <Box sx={{ 
          display: 'flex', 
          justifyContent: 'space-between', 
          alignItems: 'center' 
        }}>
          <Button 
            variant="outlined" 
            disabled={currentPage === 0}
            onClick={() => setCurrentPage(Math.max(0, currentPage - 1))}
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
              if (currentPage < totalPages - 1) {
                setCurrentPage(currentPage + 1);
              } else {
                handleSubmit();
              }
            }}
          >
            {currentPage < totalPages - 1 ? 'Next' : 'Submit'}
            
          </Button>
        </Box>
      </Paper>
    </Container>
  );
};

export default QuizPage;