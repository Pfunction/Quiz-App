import React from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import QuizPage from './pages/QuizPage';
import HighScoresPage from './pages/HighScoresPage'

const App = () => {
  return (
    
    <Router>
      <Routes>
        <Route path="/" element={<QuizPage />} />

        <Route path="/highscores" element={<HighScoresPage />}/>
      </Routes>
    </Router>
  );
}

export default App;
