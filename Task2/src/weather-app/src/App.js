import React, { useState } from 'react';
import GraphsDisplay from './components/GraphsDisplay/GraphsDisplay';
import TrendDetails from './components/TrendDetails/TrendDetails';

function App() {
  const [selectedCity, setSelectedCity] = useState(null);
  const [selectedType, setSelectedType] = useState(null);

  const handleCityClick = (event, type) => {
    const chart = event.chart;
    const elements = chart.getElementsAtEventForMode(
      event,
      "nearest",
      { intersect: true },
      true
    );

    if (elements.length) {
      const firstElement = elements[0];
      const dataIndex = firstElement.index;
      const label = chart.data.labels[dataIndex];
      const city = label.split(",")[0].trim();

      setSelectedCity(city);
      setSelectedType(type);
    }
  };

  return (
    <div>
      <GraphsDisplay onCityClick={handleCityClick} />
      {selectedCity && <TrendDetails city={selectedCity} type={selectedType} />}
    </div>
  );
}

export default App;
