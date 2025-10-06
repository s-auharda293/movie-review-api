// services/movieService.js
import axios from "axios";

const API_URL = "https://localhost:7289/api/Movies/query";
export async function getMovies({ page = 1, pageSize = 5, sort = [], search = {} } = {}) {
  try {

       // Convert boolean sortAsc to "asc"/"desc"
    const sortDto = sort.map(s => ({
      Field: s.sortKey,
      Dir: s.sortAsc ? "asc" : "desc"
    }));

    const res = await axios.post(API_URL, {
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

