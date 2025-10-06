// services/movieService.js
import axios from "axios";

const API_URL = "https://localhost:7289/api/Movies";

export async function getMovies({ page = 1, pageSize = 5, sort = [], search = {} } = {}) {
  try {

       // Convert boolean sortAsc to "asc"/"desc"
    const sortDto = sort.map(s => ({
      Field: s.sortKey,
      Dir: s.sortAsc ? "asc" : "desc"
    }));

    const res = await axios.post(`${API_URL}/query`, {
      request: {
        page,
        pageSize,
        sort: JSON.stringify(sortDto), // send array as string
        searchColumn: search.searchColumn,
        searchTerm: search.searchTerm
      }
    });

    return res.data;
  } catch (err) {
    console.error("Failed to fetch movies", err);
    return { isSuccess: false, value: [], errors: err };
  }
}


export async function deleteMovie(movie) {
  try {
    // console.log(movie);

    const res = await axios.delete(API_URL, {
      data: { id: movie.id }
    });

    return res.data;
  } catch (err) {
    console.error("Failed to delete movie:", err);
  }
}

export async function createMovie(movie){
  try{
    const res = await axios.post(API_URL, {
        dto: movie
    });
    return res.data;
  }catch(err){
    console.error("Failed to add movie:", err);
    throw err;
  }
}

export async function updateMovie(movie) {
  try {
    const res = await axios.put(`${API_URL}`, {
      id:movie.id,
      dto: movie
    });
    return res.data;
  } catch (err) {
    console.error("Failed to update movie:", err);
    throw err; // rethrow so parent can handle errors
  }
}



