import React from 'react';

const HighScoreEntry = ({ entry, index }) => {
  const medalColors = ['#FFD700', '#C0C0C0', '#CD7F32'];
  const style = index < 3 ? { color: medalColors[index] } : {};

  return (
    <div style={style}>
      <p>{index + 1}. {entry.email} - {entry.score} points ({new Date(entry.dateTime).toLocaleString()})</p>
    </div>
  );
};

export default HighScoreEntry