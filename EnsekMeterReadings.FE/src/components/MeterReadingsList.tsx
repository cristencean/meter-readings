import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { METER_READINGS_API_URL } from '../utils/constants';

interface MeterReadings {
    id: number;
    accountId: number;
    meterReadValue: string;
    dateTime: string;
}

const MeterReadingsList: React.FC = () => {
    const [meterReadings, setMeterReadings] = useState<MeterReadings[]>([]);

    useEffect(() => {
        getMeterReadings();
    }, []);

    const getMeterReadings = async () => {
        try {
            const response = await axios.get(METER_READINGS_API_URL, {
                headers: {
                    'Content-Type': 'application/json',
                },
            });

            if (response.status === 200) {
                setMeterReadings(response.data);
            }
            
        } catch (error) {
            console.error('Failed to load meter readings: ', error);
        }
    };

    return <div className="p-6 bg-white shadow-md rounded-lg max-w-3xl mx-auto">
        <h2 className="text-2xl font-semibold mb-4">Meter Readings list</h2>
        <button className="mb-4 px-4 py-2 bg-blue-600 text-white font-medium rounded-lg hover:bg-blue-700" onClick={getMeterReadings}>Reload Meter Readings</button>
        {meterReadings.length === 0 && (
            <div className="text-center text-gray-500">
                <p>No meter readings available.</p>
            </div>
        )}
        {meterReadings.length > 0 && (<table className="min-w-full table-auto border-collapse border border-gray-300">
            <thead className="bg-gray-100">
                <tr>
                    <th className="px-4 py-2 border-b text-left text-sm font-semibold text-gray-700">Account ID</th>
                    <th className="px-4 py-2 border-b text-left text-sm font-semibold text-gray-700">Meter Read Value</th>
                    <th className="px-4 py-2 border-b text-left text-sm font-semibold text-gray-700">Date Time</th>
                </tr>
            </thead>
            <tbody>
                {meterReadings && meterReadings.map((reading) => (
                    <tr key={reading.id} className="hover:bg-gray-50">
                        <td className="px-4 py-2 border-b text-sm text-gray-800">{reading.accountId}</td>
                        <td className="px-4 py-2 border-b text-sm text-gray-800">{reading.meterReadValue}</td>
                        <td className="px-4 py-2 border-b text-sm text-gray-800">{reading.dateTime}</td>
                    </tr>
                ))}
            </tbody>
        </table>)}
    </div>;
}
export default MeterReadingsList;