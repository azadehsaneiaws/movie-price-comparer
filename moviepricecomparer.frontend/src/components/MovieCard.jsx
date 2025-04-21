import React from "react";
import { Card, CardContent, Typography } from "@mui/material";

const MovieCard = ({ movie }) => (
  <Card sx={{ marginBottom: 2 }}>
    <CardContent>
      <Typography variant="h6">{movie.title || "Unknown Title"}</Typography>
      <Typography variant="body2">Year: {movie.year || "N/A"}</Typography>
      <Typography variant="body2">
        Cheapest: {movie.provider || "Unknown"} - ${movie.price ?? "?"}
      </Typography>
    </CardContent>
  </Card>
);

export default MovieCard;
