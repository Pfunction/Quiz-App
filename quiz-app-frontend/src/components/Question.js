import React from 'react'
import {TextField, FormControl, FormControlLabel, RadioGroup, Radio, Checkbox} from '@mui/material'


const Question = ({question, handleChange}) => {
    const renderOptions = () => {
        switch (question.type) {
            case 'radio':
                return (
                    <RadioGroup name ={`question-${question.id}`} onChange={(e) => handleChange(question.id, [e.target.value])}>
                        {question.answers.map((answer) => (
                            <FormControlLabel  key={answer.id} value={answer.id} control={<Radio />} label={answer.text}/>
                        ))}

                    </RadioGroup>
                );
            case 'checkbox':
              return question.answers.map((answer) => (
                <FormControlLabel
                  key={answer.id}
                  control={<Checkbox onChange={(e) => handleChange(question.id, e.target.checked ? [...(question.selectedAnswers || []), answer.id] : (question.selectedAnswers || []).filter(id => id !== answer.id))} />}
                  label={answer.text}
                />
              ));
            case 'textbox':
              return <TextField label="Your Answer" variant="outlined" onChange={(e) => handleChange(question.id, [e.target.value])} />;
            default:
              return null
        }
    }
    return (
        <FormControl component='fieldset'>
            <h3>{question.text}</h3>
            {renderOptions()}
        </FormControl>
    );
};

export default Question