
import React, { useState, useEffect } from 'react';
import axios from 'axios';
import HighScoreEntry from '../components/HighScoreEntry';

const HighScoresPage = () => {
  const [highScores, setHighScores] = useState([]);

  useEffect(() => {
    axios.get('/api/quizzes/highscores').then((response) => {
      setHighScores(response.data);
    });
  }, []);

  return (
    <div>
      <h1>High Scores</h1>
      {highScores.map((entry, index) => (
        <HighScoreEntry key={entry.id} entry={entry} index={index} />
      ))}
    </div>
  );
};

export default HighScoresPage;
