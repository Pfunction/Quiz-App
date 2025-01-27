import React from 'react';
import { Box, Container, Stepper, Step, StepLabel } from '@mui/material';
import CustomStepIcon from './CustomStepIcon';
import { StepConnector } from '@mui/material';
import { display, margin, styled } from '@mui/system';

const CustomConnector = styled(StepConnector)(({ theme }) => ({
  '& .MuiStepConnector-line': {
    borderColor: 'gray',
    borderWidth: 1,
    marginTop: 2

    
  },
}));

const QuizStepper = ({ currentPage, totalPages }) => {
  return (
    <Box sx={{ 
      position: 'fixed',
      bottom: 0,
      left: 0,
      right: 0,
      bgcolor: 'background.paper',
      p: 2,
      zIndex: 1000,
      boxShadow: 3
    }}>
      <Container maxWidth="md">

        <Box sx={{ 
          display: { xs: 'none', md: 'block' }
        }}>
         <Stepper
        activeStep={currentPage}
        alternativeLabel
        connector={<CustomConnector />}
      >
        {Array.from({ length: totalPages }).map((_, index) => (
          <Step key={index}>
            <StepLabel
              StepIconComponent={(props) => (
                <CustomStepIcon
                  {...props}
                  completed={currentPage > index}
                />
              )}
              sx={{
                '& .MuiStepLabel-label': {
                  display: 'none',
                },
              }}
            />
          </Step>
        ))}
      </Stepper>

        </Box>


        <Box sx={{ 
          display: { xs: 'block', md: 'none' }
        }}>
          <Stepper 
            activeStep={currentPage} 
            alternativeLabel
  
           

          >
            {(() => {
              let stepsToShow = [];
              if (currentPage === 0) {
                stepsToShow = [0, 1, 2];
              } else if (currentPage === totalPages - 1) {
                stepsToShow = [totalPages - 3, totalPages - 2, totalPages - 1];
              } else {
                stepsToShow = [currentPage - 1, currentPage, currentPage + 1];
              }
              return stepsToShow
                .filter(step => step >= 0 && step < totalPages)
                .map((stepIndex) => (
                  <Step key={stepIndex}>
                    <StepLabel 
                      StepIconComponent={(props) => (
                        <CustomStepIcon 
                          {...props}
                          icon={stepIndex + 1}
                          active={currentPage === stepIndex}
                          completed={currentPage > stepIndex}
                        />
                      )}
                      sx={{
                        '& .MuiStepLabel-label': {
                          display: 'none'
                        }
                      }}
                    />
                  </Step>
                ));
            })()}
          </Stepper>
        </Box>
      </Container>
    </Box>
  );
};

export default QuizStepper;