import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { 
  Container, 
  Paper, 
  Typography, 
  Table, 
  TableBody, 
  TableCell, 
  TableContainer, 
  TableHead, 
  TableRow 
} from '@mui/material';

const HighScoresPage = () => {
  const [highScores, setHighScores] = useState([]);

  useEffect(() => {
    axios.get('https://localhost:5001/api/quizzes/highscores')
      .then((response) => {
        setHighScores(response.data);
      });
  }, []);

  const medalColors = ['#FFD700', '#C0C0C0', '#CD7F32'];

  return (
    <Container maxWidth="md">
      <Paper elevation={3} sx={{ p: 3, mt: 4 }}>
        <Typography variant="h4" gutterBottom>
          High Scores
        </Typography>
        
        <TableContainer>
          <Table>
            <TableHead>
              <TableRow>
                <TableCell>Position</TableCell>
                <TableCell>Email</TableCell>
                <TableCell align="right">Score</TableCell>
                <TableCell align="right">Date</TableCell>
              </TableRow>
            </TableHead>
            <TableBody>
              {highScores.map((entry, index) => (
                <TableRow 
                  key={entry.id}
                  sx={{ 
                    backgroundColor: index < 3 
                      ? `${medalColors[index]}20` 
                      : 'inherit' 
                  }}
                >
                  <TableCell 
                    sx={{ 
                      fontWeight: 'bold', 
                      color: index < 3 ? medalColors[index] : 'inherit' 
                    }}
                  >
                    {index + 1}
                  </TableCell>
                  <TableCell>{entry.email}</TableCell>
                  <TableCell align="right">{entry.score} pts</TableCell>
                  <TableCell align="right">
                    {new Date(entry.dateTime).toLocaleString()}
                  </TableCell>
                </TableRow>
              ))}
            </TableBody>
          </Table>
        </TableContainer>
      </Paper>
    </Container>
  );
};

export default HighScoresPage;