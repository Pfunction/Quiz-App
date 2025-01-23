
import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { Button, TextField } from '@mui/material';
import Question from '../components/Question';

const QuizPage = () => {
  const [quiz, setQuiz] = useState(null);
  const [email, setEmail] = useState('');
  const [answers, setAnswers] = useState({});

  useEffect(() => {
    axios.get('/api/quizzes').then((response) => {
      setQuiz(response.data[0]);
    });
  }, []);

  const handleChange = (questionId, answer) => {
    setAnswers({ ...answers, [questionId]: answer });
  };

  const handleSubmit = () => {
    const submission = {
      quizId: quiz.id,
      email,
      answers: Object.keys(answers).map((questionId) => ({
        questionId: parseInt(questionId),
        answerIds: answers[questionId].filter((ans) => typeof ans === 'number'),
        text: answers[questionId].find((ans) => typeof ans === 'string') || null,
      })),
    };

    axios.post('/api/quizzes/submit', submission).then((response) => {
      alert(`Your score: ${response.data}`);
    });
  };

  if (!quiz) return <div>Loading...</div>;

  return (
    <div>
      <h1>{quiz.title}</h1>
      <TextField label="Email" variant="outlined" fullWidth margin="normal" onChange={(e) => setEmail(e.target.value)} />
      {quiz.questions.map((question) => (
        <Question key={question.id} question={question} handleChange={handleChange} />
      ))}
      <Button variant="contained" color="primary" onClick={handleSubmit}>Submit</Button>
    </div>
  );
};

export default QuizPage;
