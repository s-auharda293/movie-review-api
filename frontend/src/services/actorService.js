import api from "@/api";

export async function getActors({ page = 1, pageSize = 5, sort = [], search = {} } = {}) {
  try {
       // Convert boolean sortAsc to "asc"/"desc"
    const sortDto = sort.map(s => ({
      Field: s.sortKey,
      Dir: s.sortAsc ? "asc" : "desc"
    }));

    const res = await api.post(`/Actors/query`, {
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
    console.error("Failed to fetch actors", err);
    return { isSuccess: false, value: [], errors: err };
  }
}

export async function getAllActors() {
  try {
    const res = await api.get('/Actors');
    return res.data;
  } catch (err) {
    console.error("Failed to fetch actors", err);
    return [];
  }
}


export async function deleteActor(actor) {
  try {
    // console.log(Actor);

    const res = await api.delete('/Actors', {
      data: { id: actor.id }
    });

    return res.data;
  } catch (err) {
    console.error("Failed to delete actor:", err);
  }
}

export async function createActor(actor){
  try{
    const res = await api.post('/Actors', {
        dto: actor
    });
    return res.data;
  }catch(err){
    console.error("Failed to add actor:", err);
    throw err;
  }
}

export async function updateActor(actor) {
  try {
    const res = await api.put('/Actors', {
      id: actor.id,
      dto: actor
    });
    return res.data;
  } catch (err) {
    console.error("Failed to update actor:", err);
    throw err; // rethrow so parent can handle errors
  }
}



