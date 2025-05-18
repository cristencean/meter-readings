import './App.css';
import AddMeterReadings from './components/AddMeterReadings';
import MeterReadingsList from './components/MeterReadingsList';

function App() {
  return (
    <div className="App min-h-screen bg-gray-100 text-gray-800">
      <header className="bg-gray-800 text-white p-4">
        <h1 className="text-3xl font-semibold">Meter readings application</h1>
      </header>
      <section className="max-w-4xl mx-auto p-4">
        <AddMeterReadings />
        <br />
        <MeterReadingsList />
      </section>
    </div>
  );
}

export default App;
