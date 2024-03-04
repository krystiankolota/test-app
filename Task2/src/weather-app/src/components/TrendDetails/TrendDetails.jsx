import React, { useEffect, useState } from 'react';
import { Line } from 'react-chartjs-2';
import axios from 'axios';

const TrendDetails = ({ city, type }) => {
  const [trendData, setTrendData] = useState([]);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const response = await axios.get(`${process.env.REACT_APP_API_URL}/trend/${city}`);
        setTrendData(response.data);
      } catch (error) {
        console.error('Error fetching trend data:', error);
      }
    };

    fetchData();
  }, [city, type]);

  const data = {
    labels: trendData.map(data => new Date(data.lastUpdate).toLocaleTimeString()),
    datasets: [{
      label: type === 'temperature' ? 'Temperature (°C)' : 'Wind Speed (kph)',
      data: trendData.map(data => type === 'temperature' ? data.temperature : data.windSpeed),
      fill: false,
      borderColor: type === 'temperature' ? 'rgb(255, 99, 132)' : 'rgb(54, 162, 235)',
      tension: 0.1
    }]
  };

  return (
    <div>
      <h2>{type === 'temperature' ? 'Temperature Trend' : 'Wind Speed Trend'}</h2>
      <h2>{city}</h2>
      <Line data={data} />
    </div>
  );
};

export default TrendDetails;