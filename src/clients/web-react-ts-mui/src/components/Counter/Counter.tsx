import { Box, Button, Stack } from '@mui/material';
import { decrement, incrementAsync } from '@/features/counterSlice';
import { AppDispatch, RootState } from '@/app/store';
import { useDispatch, useSelector } from "react-redux";

const Counter = () => {
  const dispatch = useDispatch<AppDispatch>();
  const count = useSelector((state: RootState) => state.counter.value);

  return (
    <Stack
      sx={{
        width: '100%',
        backgroundColor: 'grey.100',
        boxShadow: 1,
        mt: 2,
        p: 2,
        borderRadius: 8,
        fontSize: 20,
        button: {
          fontSize: 20,
        },
      }}
      gap={2}
      direction="row"
      alignItems="center"
      justifyContent="center"
    >
      <Button variant="contained" aria-label="Decrement value" onClick={() => dispatch(decrement(1))}>
        -
      </Button>
      <Box
        sx={{
          borderRadius: 1,
          backgroundColor: 'grey.200',
          boxShadow: 3,
          p: 1,
          height: 47,
          minWidth: 47,
          display: 'flex',
          alignItems: 'center',
          justifyContent: 'center',
        }}
      >
        {count}{' '}
      </Box>
      <Button
        variant="contained"
        sx={{ backgroundColor: 'primary.main' }}
        aria-label="Increment value"
        onClick={() => dispatch(incrementAsync(1))}
      >
        +
      </Button>
    </Stack>
  );
};

export default Counter;
