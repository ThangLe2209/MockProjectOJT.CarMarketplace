import axios from "axios";

const instance = axios.create({
  baseURL: process.env.REACT_APP_CAR_API_URL || "http://localhost:5281/api", // Adjust port as needed
  // baseURL: process.env.REACT_APP_CAR_API_URL || "http://localhost:5002/api", // Adjust port as needed
});

instance.interceptors.request.use((config) => {
  const token = localStorage.getItem("token");
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

export default instance;
