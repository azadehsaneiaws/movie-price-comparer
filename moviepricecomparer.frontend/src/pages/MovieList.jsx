// src/pages/MovieList.jsx
import React, { useEffect, useState } from "react";
import { getAllMovies } from "../services/movieService";
import MovieCard from "../components/MovieCard";
import {
  Container,
  Typography,
  CircularProgress,
  TextField,
  Box,
} from "@mui/material";

const MovieList = () => {
  const [movies, setMovies] = useState([]);
  const [searchTerm, setSearchTerm] = useState("");
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    getAllMovies()
      .then((data) => setMovies(data))
      .catch((err) => setError(err.message || "Unknown error"))
      .finally(() => setLoading(false));
  }, []);

  const filtered = movies.filter((m) =>
    m.title.toLowerCase().includes(searchTerm.toLowerCase())
  );

  return (
    <Container sx={{ mt: 4 }}>
      <Typography variant="h3" gutterBottom align="center">
        Movie Price Compare
      </Typography>
      <Typography variant="subtitle1" align="center" color="textSecondary" gutterBottom>
        Compare prices from Cinemaworld and Filmworld
      </Typography>

      <Box mt={4} mb={2}>
        <TextField
          fullWidth
          label="Search by title"
          value={searchTerm}
          onChange={(e) => setSearchTerm(e.target.value)}
        />
      </Box>

      {loading && (
        <Box mt={6} textAlign="center">
          <CircularProgress />
        </Box>
      )}

      {error && !loading && (
        <Box mt={4} textAlign="center">
          <Typography color="error" variant="h6">
             Failed to load movies
          </Typography>
          <Typography variant="body2" color="textSecondary">
            Please check your connection or try again later.
          </Typography>
        </Box>
      )}

      {!loading && !error && filtered.length === 0 && (
        <Box mt={4} textAlign="center">
          <Typography variant="body1">No movies found for your search.</Typography>
        </Box>
      )}

      {!loading && !error && (
        <Box mt={3}>
          {filtered.map((m) => (
            <MovieCard key={m.id} movie={m} />
          ))}
        </Box>
      )}
    </Container>
  );
};

export default MovieList;