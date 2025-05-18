import { render, screen } from '@testing-library/react';
import App from './App';

test('enders app title', () => {
  render(<App />);
  const titleElement = screen.getByText('Meter readings application');
  expect(titleElement).toBeInTheDocument();
});
