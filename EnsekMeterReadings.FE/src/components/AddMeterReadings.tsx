import React, { useState } from 'react';
import axios from 'axios';
import { UPLOAD_API_URL } from '../utils/constants';

interface MeterReadingsResults {
    validEntries: number;
    invalidEntries: number;
};

const AddMeterReadings: React.FC = () => {
    const [file, setFile] = useState<File | null>(null);
    const [errorMessage, setErrorMessage] = useState<string | null>(null);
    const [successMessage, setSuccessMessage] = useState<string | null>(null);
    const [resultStatus, setResultStatus] = useState<MeterReadingsResults>({
        validEntries: 0, invalidEntries: 0
    });

    const handleFileUpdate = (event: React.ChangeEvent<HTMLInputElement>) => {
        setSuccessMessage(null);
        setErrorMessage(null);
        const selectedFile = event.target.files?.[0];
        if (selectedFile && selectedFile.type === 'text/csv') {
            setFile(selectedFile);
        } else {
            setErrorMessage('Please upload a valid CSV file');
            setFile(null);
        }
    };

    const handleSubmit = async (event: React.FormEvent) => {
        event.preventDefault();

        if (!file) {
            setErrorMessage('Please add a CSV file');
            return;
        }

        const formData = new FormData();
        formData.append('file', file);

        try {
            setSuccessMessage(null);
            setErrorMessage(null);
            const response = await axios.post(UPLOAD_API_URL, formData, {
                headers: {
                    'Content-Type': 'multipart/form-data',
                },
            });

            setResultStatus({
                validEntries: response.data.validEntries,
                invalidEntries: response.data.invalidEntries,
            });
            setSuccessMessage('Meter readings file uploaded successfully!');
        } catch (err) {
            setErrorMessage('Error uploading meter readings. Please try again.');
        }
    };

    return (
        <div className="p-6 bg-white shadow-md rounded-lg max-w-xl mx-auto">
            <h2 className="text-2xl font-semibold mb-4">Meter Readings CSV File</h2>
            <form className="space-y-4" onSubmit={handleSubmit}>
                <div>
                    <label className="block text-lg font-medium text-gray-700 mb-2" htmlFor="csv-file-input">Add CSV File:</label>
                    <input
                        className="block w-full text-sm text-gray-800 border border-gray-300 rounded-lg p-2"
                        id="csv-file-input"
                        type="file"
                        accept=".csv"
                        onChange={handleFileUpdate}
                    />
                </div>
                {errorMessage && <p className="text-red-500 text-sm mt-2">{errorMessage}</p>}
                {successMessage && <p className="text-green-500 text-sm mt-2">{successMessage}</p>}
                {successMessage &&
                    (
                        <>
                            <p className="text-sm text-gray-800"><strong>Valid entries</strong>: {resultStatus.validEntries}</p>
                            <p className="text-sm text-gray-800"><strong>Invalid entries</strong>: {resultStatus.invalidEntries}</p>
                        </>
                    )}
                <button className="w-full py-2 bg-blue-600 text-white font-medium rounded-lg hover:bg-blue-700 disabled:opacity-50" type="submit" disabled={!file}>
                    Submit
                </button>
            </form>
        </div>
    );
};

export default AddMeterReadings;