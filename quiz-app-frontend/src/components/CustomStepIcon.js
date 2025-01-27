import React from 'react';
import { Box, Typography } from '@mui/material';
import { Check } from 'lucide-react';

const CustomStepIcon = ({ active, completed, icon }) => {
  return (
    <Box
      sx={{
        width: 32,
        height: 32,
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'center',
        borderRadius: '50%',
        bgcolor: active ? 'primary.main' : completed ? 'success.main' : 'grey.400',
        color: '#fff',
        transition: 'all 0.3s ease',
        position: 'relative',
        '& > div': {
          position: 'absolute',
          width: '100%',
          height: '100%',
          display: 'flex',
          alignItems: 'center',
          justifyContent: 'center',
          transition: 'all 0.3s ease',
        }
      }}
    >
      <Box
        sx={{
          opacity: completed ? 0 : 1,
          transform: completed ? 'scale(0)' : 'scale(1)',
        }}
      >
        <Typography 
          variant="body2" 
          sx={{ 
            fontSize: '14px',
            fontWeight: 500
          }}
        >
          {icon}
        </Typography>
      </Box>
      <Box
        sx={{
          opacity: completed ? 1 : 0,
          transform: completed ? 'scale(1)' : 'scale(0)',
        }}
      >
        <Check size={18} />
      </Box>
    </Box>
  );
};

export default CustomStepIcon;