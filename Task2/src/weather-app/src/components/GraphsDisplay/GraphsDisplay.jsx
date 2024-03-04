import React, { useEffect, useState } from "react";
import { Line } from "react-chartjs-2";
import { Chart, registerables } from "chart.js";
import axios from "axios";

Chart.register(...registerables);

const GraphsDisplay = ({ onCityClick }) => {
  const [weatherData, setWeatherData] = useState({
    minTemperature: [],
    maxWindSpeed: [],
  });

  useEffect(() => {
    const fetchData = async () => {
      try {
        const response = await axios.get(
          `${process.env.REACT_APP_API_URL}/summary`
        );
        setWeatherData(response.data);
      } catch (error) {
        console.error("Error fetching weather data:", error);
      }
    };

    fetchData();
    const intervalId = setInterval(fetchData, 60000);

    return () => clearInterval(intervalId);
  }, []);

  const temperatureData = {
    labels: weatherData.minTemperature.map(
      (data) => `${data.city}, ${data.country}`
    ),
    datasets: [
      {
        label: "Min Temperature (°C)",
        data: weatherData.minTemperature.map((data) => data.temperature),
        fill: false,
        borderColor: "rgb(255, 99, 132)",
        tension: 0.1,
      },
    ],
  };

  const windSpeedData = {
    labels: weatherData.maxWindSpeed.map(
      (data) => `${data.city}, ${data.country}`
    ),
    datasets: [
      {
        label: "Max Wind Speed (kph)",
        data: weatherData.maxWindSpeed.map((data) => data.windSpeed),
        fill: false,
        borderColor: "rgb(54, 162, 235)",
        tension: 0.1,
      },
    ],
  };

  return (
    <div>
      <h2>Min Temperature</h2>
      <Line
        data={temperatureData}
        options={{
          onClick: (event) => {
            onCityClick(event, "temperature");
          },
        }}
      />
      <h2>Highest Wind Speed</h2>
      <Line
        data={windSpeedData}
        options={{
          onClick: (event) => {
            onCityClick(event, "windSpeed");
          },
        }}
      />
    </div>
  );
};

export default GraphsDisplay;
