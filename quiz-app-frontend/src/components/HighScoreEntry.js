import React from 'react';
import PropTypes from 'prop-types';
import { Box, Typography, Card, CardContent } from '@mui/material';

const HighScoreEntry = ({ entry, index }) => {
  const medalColors = ['#FFD700', '#C0C0C0', '#CD7F32'];

  return (
    <Card
      variant="outlined"
      sx={{
        mb: 2,
        borderColor: index < 3 ? medalColors[index] : 'grey.300',
        backgroundColor: index < 3 ? `${medalColors[index]}20` : 'background.paper',
      }}
      aria-label={`High score entry for ${entry.email}`}
    >
      <CardContent>
        <Box display="flex" justifyContent="space-between" alignItems="center">
          <Typography
            variant="h6"
            sx={{
              color: index < 3 ? medalColors[index] : 'text.primary',
              fontWeight: index < 3 ? 'bold' : 'normal',
            }}
          >
            {index + 1}.
          </Typography>

          <Typography variant="body1" sx={{ flexGrow: 1, mx: 2 }}>
            {entry.email}
          </Typography>

          <Typography variant="body1" sx={{ fontWeight: 'bold' }}>
            {entry.score} pts
          </Typography>

          <Typography variant="body2" color="text.secondary" sx={{ ml: 2 }}>
            {new Intl.DateTimeFormat('default', {
              dateStyle: 'medium',
              timeStyle: 'short',
            }).format(new Date(entry.dateTime))}
          </Typography>
        </Box>
      </CardContent>
    </Card>
  );
};

HighScoreEntry.propTypes = {
  entry: PropTypes.shape({
    email: PropTypes.string.isRequired,
    score: PropTypes.number.isRequired,
    dateTime: PropTypes.string.isRequired,
  }).isRequired,
  index: PropTypes.number.isRequired,
};

export default HighScoreEntry;
