import React from "react";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import MovieList from "./pages/MovieList";

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<MovieList />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
