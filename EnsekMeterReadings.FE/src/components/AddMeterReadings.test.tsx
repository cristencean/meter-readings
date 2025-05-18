import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import AddMeterReadings from './AddMeterReadings';
import axios from 'axios';

jest.mock('axios');
const mockedAxios = axios as jest.Mocked<typeof axios>;

describe('AddMeterReadings component', () => {
  it('renders correctly', () => {
    render(<AddMeterReadings />);
    expect(screen.getByText(/Meter Readings CSV File/i)).toBeInTheDocument();
    expect(screen.getByLabelText(/Add CSV File/i)).toBeInTheDocument();
    expect(screen.getByRole('button', { name: /submit/i })).toBeInTheDocument();
  });

  it('shows error when wrong file format is uploaded', () => {
    render(<AddMeterReadings />);
    const fileInput = screen.getByLabelText(/Add CSV File/i);
    const invalidFile = new File(['dummy'], 'text.txt', { type: 'text/plain' });

    fireEvent.change(fileInput, { target: { files: [invalidFile] } });

    expect(screen.getByText(/Please upload a valid CSV file/i)).toBeInTheDocument();
  });

  it('calls the API and displays success message on valid submission', async () => {
    mockedAxios.post.mockResolvedValue({
      data: {
        validEntries: 15,
        invalidEntries: 7,
      },
    });

    render(<AddMeterReadings />);

    const fileInput = screen.getByLabelText(/Add CSV File/i);
    const submitButton = screen.getByRole('button', { name: /submit/i });

    const csvFile = new File(['AccountId,MeterReadingDateTime,MeterReadValue\n2344,22/04/2025 12:24,03874'], 'meterReadings.csv', {
      type: 'text/csv',
    });

    await waitFor(() => {
      fireEvent.change(fileInput, { target: { files: [csvFile] } });
    });

    await waitFor(() => {
      fireEvent.click(submitButton);
    });

    await waitFor(() => {
      expect(screen.getByText('Meter readings file uploaded successfully!')).toBeInTheDocument();
      expect(screen.getByText('Valid entries')).toBeInTheDocument();
      expect(screen.getByText('Invalid entries')).toBeInTheDocument();
    });
  });

  it('shows an error if could not process the file', async () => {
    mockedAxios.post.mockRejectedValue(new Error('Process failed'));

    render(<AddMeterReadings />);

    const fileInput = screen.getByLabelText(/Add CSV File/i);
    const submitButton = screen.getByRole('button', { name: /submit/i });

    const csvFile = new File(['test'], 'meterReadings.csv', { type: 'text/csv' });

    fireEvent.change(fileInput, { target: { files: [csvFile] } });
    fireEvent.click(submitButton);

    await waitFor(() => {
      expect(screen.getByText(/Error uploading meter readings/i)).toBeInTheDocument();
    });
  });
});
