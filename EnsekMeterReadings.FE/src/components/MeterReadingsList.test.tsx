import { render, screen, waitFor, fireEvent } from '@testing-library/react';
import MeterReadingsList from './MeterReadingsList';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { METER_READINGS_API_URL } from '../utils/constants';

// Setup mock
const mock = new MockAdapter(axios);

describe('MeterReadingsList', () => {
  afterEach(() => {
    mock.reset();
  });

  it('renders meter readings list when API call succeeds', async () => {
    const mockData = [
      { id: 12, accountId: 6473, meterReadValue: '00647', dateTime: '2025-01-01T09:27:00Z' },
      { id: 13, accountId: 2239, meterReadValue: '02039', dateTime: '2025-04-02T10:12:00Z' }
    ];

    mock.onGet(METER_READINGS_API_URL).reply(200, mockData);

    render(<MeterReadingsList />);

    expect(await screen.findByText('00647')).toBeInTheDocument();
    expect(screen.getByText('02039')).toBeInTheDocument();
  });

  it('shows error message when no meter readings are returned', async () => {
    mock.onGet(METER_READINGS_API_URL).reply(200, []);

    render(<MeterReadingsList />);

    expect(await screen.findByText(/no meter readings available/i)).toBeInTheDocument();
  });

  it('reloads meter readings list when button is clicked', async () => {
    const firstLoad = [
      { id: 2, accountId: 1001, meterReadValue: '00123', dateTime: '2025-01-01T09:00:00Z' }
    ];
    const secondLoad = [
      { id: 5, accountId: 1002, meterReadValue: '00124', dateTime: '2025-01-02T09:00:00Z' }
    ];

    mock.onGet(METER_READINGS_API_URL)
      .replyOnce(200, firstLoad)
      .onGet(METER_READINGS_API_URL)
      .replyOnce(200, secondLoad);

    render(<MeterReadingsList />);

    expect(await screen.findByText('00123')).toBeInTheDocument();

    fireEvent.click(screen.getByText(/reload meter readings/i));

    await waitFor(() => {
      expect(screen.getByText('00124')).toBeInTheDocument();
    });
  });
});
