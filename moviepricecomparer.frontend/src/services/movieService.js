import axios from "axios";

const API_URL = process.env.REACT_APP_API_URL;
export const getAllMovies = async () => {
  const res = await axios.get(`${API_URL}/movies`);
  return res.data;
};

export const getMovieDetails = async (id) => {
  const res = await axios.get(`${API_URL}/movies/${id}`);
  return res.data;
};
