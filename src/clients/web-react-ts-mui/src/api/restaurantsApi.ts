import axios from "axios";

const restaurantsApiClient = axios.create({
  baseURL: import.meta.env.VITE_RESTAURANTS_API_URL,
  headers: {
    "Content-Type": "application/json",
  }
});

export default restaurantsApiClient;