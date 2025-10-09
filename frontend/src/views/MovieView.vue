<script setup>
import { ref, onMounted, watch } from "vue";
import { deleteMovie, getMovies, createMovie, updateMovie } from "../services/movieService";
import Pagination from "@/components/Pagination.vue";
import ViewModal from "@/components/ViewModal.vue";
import DeleteModal from "@/components/DeleteModal.vue";
import CreateEditMovieModal from "@/components/CreateEditMovieModal.vue";

const movies = ref([]);
const totalCount = ref(0);

// Server-side parameters
const currentPage = ref(1);
const pageSize = ref(10);
const sortColumns = ref([]);

const searchColumn = ref("title"); // column to search
const searchTerm = ref("");        // search text

//view
const showViewModal = ref(false);
const movieToView = ref(null);

//create or edit
const showMovieModal = ref(false);
const selectedMovie = ref(null);
const formMode = ref("create");
const formErrors = ref({});

function addMovie() {
  formMode.value = "create";
  selectedMovie.value = null;
  showMovieModal.value = true;
  formErrors.value = {};
}

function editMovie(movie) {
  formMode.value = "edit";
  selectedMovie.value = movie;
  showMovieModal.value = true;
  formErrors.value = {};
}

function closeMovieModal() {
  showMovieModal.value = false;
}

async function handleMovieSubmit(movieData) {
  try{
    formErrors.value = {};
    let data;
    if (formMode.value === "create") {
    // console.log(movieData);

    // call API to create movie
    data = await createMovie(movieData);

    //  console.log(data);

    }else if(formMode.value==="edit") {
    // call API to update movie
    data = await updateMovie(movieData);
    }

    if(data.isSuccess){
      await fetchMovies(); // refresh movie list
      showMovieModal.value = false; // close modal
    }
  }catch(err){
     if (err.response?.data?.errors) {
      // convert backend error format to key:value
      const errorMap = {};
      err.response.data.errors.forEach((e) => {
        const field = e.code.split(".")[1].toLowerCase();
        errorMap[field] = e.description;
      });
      formErrors.value = errorMap; // send to modal
    }
  }
}


// delete
const showDeleteModal = ref(false);
const movieToDelete = ref(null);

async function fetchMovies() {
  const data = await getMovies({
    page: currentPage.value,
    pageSize: pageSize.value,
    sort: sortColumns.value,
    search: {
      searchColumn: searchColumn.value,
      searchTerm: searchTerm.value
    }
  });

  if (data.isSuccess) {
    movies.value = data.value.movies;
    totalCount.value = data.value.totalCount;
  }
}

// Fetch on mount
onMounted(fetchMovies);

// Watchers to refetch data on changes
watch([currentPage, pageSize, searchColumn, searchTerm], fetchMovies);

watch(sortColumns, fetchMovies, { deep: true });

function sortColumn(column) {
  const col = sortColumns.value[0]; // only one column for now
  if (col?.sortKey === column) {
    col.sortAsc = !col.sortAsc; // toggle
  } else {
    sortColumns.value = [{ sortKey: column, sortAsc: true }]; // replace old sort
  }
  fetchMovies(); // fetch immediately with new sort
}

function getSortArrow(column) {
  const col = sortColumns.value[0];
  if (!col || col.sortKey !== column) return '';
  return col.sortAsc ? '▲' : '▼';
}

// view modal setup
function openViewModal(movie){
  movieToView.value = movie;
  showViewModal.value = true;
}

function closeViewModal(){
  movieToView.value = null;
  showViewModal.value = false;
}

// delete modal setup
function openDeleteModal(movie) {
  movieToDelete.value = movie;
  showDeleteModal.value = true;
}

function closeDeleteModal() {
  movieToDelete.value = null;
  showDeleteModal.value = false;
}

async function confirmDelete() {
  if (!movieToDelete.value) return;

  try {
    const data = await deleteMovie(movieToDelete.value);
    console.log("Deleted movie response:", data);

    await fetchMovies(); // refresh the movie list
    closeDeleteModal();   // close the modal
  } catch (err) {
    console.error("Failed to delete movie:", err);
  }
}

const colors = [
  "bg-green-100",
  "bg-blue-100",
  "bg-red-100",
  "bg-yellow-100",
  "bg-purple-100",
  "bg-pink-100",
  "bg-indigo-100"
];

function getRandomColor() {
  return colors[Math.floor(Math.random() * colors.length)];
}

</script>


<template>
  <h1 class="text-gray-500 text-xl mb-4 mt-2">Movies</h1>

  <!-- Search box -->
  <div class="mb-4 flex justify-between">
    <div class="flex gap-2">
      <!-- Column filter -->
      <select
      v-model="searchColumn"
      class=" border-gray-200 rounded-full px-3 py-2 border"
      >
      <option value="id">Movie Id</option>
      <option value="title">Title</option>
      <option value="description">Description</option>
      <option value="releaseDate">Release Date</option>
      <option value="durationMinutes">Duration</option>
      <option value="rating">Rating</option>
    </select>

    <input
    type="text"
    v-model="searchTerm"
    placeholder="Search movies..."
    class="form-control border p-2 w-72 rounded-full border-gray-300"
    />
  </div>

  <button
    class="mt-3 px-3 py-1 rounded-md bg-blue-600 text-white text-md hover:bg-blue-700 mr-10 cursor-pointer"
    @click="addMovie"
  >
    Add Movie
  </button>


  </div>

  <div class="bootstrap-table">
    <div class="table-responsive">
      <table class="table table-hover custom-ant-table">
        <thead>
          <tr>
            <th>S.N</th>
            <th>
              <div>
                <span>
                  Movie Id
                </span>
              </div>
            </th>
            <th @click="sortColumn('title')" style="cursor:pointer">
              Title
              <span>{{ getSortArrow('title') }}</span>
            </th>
             <th @click="sortColumn('description')" style="cursor:pointer">
                  Description
                <span>{{ getSortArrow('description') }}</span>
            </th>
           <th @click="sortColumn('releaseDate')" style="cursor:pointer">
            Release Date
            <span>{{ getSortArrow('releaseDate') }}</span>
          </th>
          <th @click="sortColumn('durationMinutes')" style="cursor:pointer">
            Duration(min)
            <span>{{ getSortArrow('durationMinutes') }}</span>
          </th>
          <th @click="sortColumn('rating')" style="cursor:pointer">
            Rating
            <span>{{ getSortArrow('rating') }}</span>
          </th>
          <th>
            Actors
          </th>
          <th>Actions</th>

          </tr>
        </thead>
        <tbody>
          <tr v-for="(movie, index) in movies" :key="movie.id">
            <th>{{ index + 1 + (currentPage-1)*pageSize }}</th>
            <td  class="text-truncate" style="max-width: 150px;">{{ movie.id }}</td>
            <td  class="text-truncate" style="max-width: 150px;">{{ movie.title }}</td>
            <td  class="text-truncate" style="max-width: 150px;" :title="movie.description">{{ movie.description }}</td>
            <td>{{ new Date(movie.releaseDate).toLocaleDateString() }}</td>
            <td>{{ movie.durationMinutes }}</td>
            <td>{{ movie.rating }}</td>
            <td>
              <div class="flex flex-wrap w-28 gap-1">
                <span v-for="(actor,index) in movie.actors" :key="index" :class="`border px-3 ${getRandomColor()} rounded-full italic text-gray-500`">{{ actor.name }}</span>
              </div>
            </td>
            <td>
              <div class="d-flex gap-1 flex-wrap">
                <button
                @click="openViewModal(movie)"
                class="btn btn-success btn-sm flex justify-center items-center hover:bg-opacity hover:bg-[#031b0d] transition-colors"
                style="border-radius:12px"
              >
                <span class="pi pi-eye"></span>
              </button>
                <button
                @click="editMovie(movie)"
                 class="btn btn-primary btn-sm flex justify-center items-center hover:bg-[#0a143a] transition-colors"
                 style="border-radius:12px">
                 <span class="pi pi-pencil"></span>
                </button>
                <button
                      @click="openDeleteModal(movie)"
                      class="btn btn-danger btn-sm flex justify-center items-center hover:bg-[#3f0000] transition-colors"
                      style="border-radius:12px">
                      <span class="pi pi-trash"></span>
                </button>
              </div>
            </td>
          </tr>
        </tbody>
      </table>
    </div>

    <!-- Pagination -->
    <Pagination v-model:currentPage="currentPage" v-model:pageSize="pageSize" v-bind:totalCount="totalCount"/>
  </div>

  <!-- View Modal -->
   <ViewModal
      v-bind:show ="showViewModal"
      v-bind:item = "movieToView"
      mode = "movie"
      @close="closeViewModal"
   />

   <CreateEditMovieModal
      v-if="showMovieModal"
      :movie="selectedMovie"
      :mode="formMode"
      :errors="formErrors"
      @submit="handleMovieSubmit"
      @cancel="closeMovieModal"
    />


    <!-- Delete Confirmation Modal -->
    <DeleteModal
      v-bind:show="showDeleteModal"
      v-bind:item="movieToDelete"
      mode="movie"
      @close="closeDeleteModal"
      @confirm="confirmDelete"
    />

</template>



<style scoped>
/* table */
/* Global Ant Design–like table styles for Bootstrap */
table.custom-ant-table {
  border-collapse: separate;
  border-spacing: 0;
  width: 100%;
  border-radius: 8px;
  overflow: hidden; /* keeps rounded corners */
  background-color: #fff;
}

/* Header */
table.custom-ant-table thead th {
  background-color: #fafafa !important; /* subtle gray like Ant Design */
  font-weight: 600;
  font-size: 14px;
  color: #555;
  border-bottom: 1px solid #e0e0e0; /* thin header border */
  padding: 12px;
}

/* Body rows */
table.custom-ant-table tbody td,
table.custom-ant-table tbody th {
  border-bottom: 1px solid #e0e0e0; /* thin row border */
  padding: 12px;
  font-size: 14px;
  color: #444;
}

/* Remove border-bottom for the last row */
table.custom-ant-table tbody tr:last-child td,
table.custom-ant-table tbody tr:last-child th {
  border-bottom: none;
}

/* Optional: hover effect */
table.custom-ant-table tbody tr:hover {
  background-color: #f5f5f5 !important;
  transition: background 0.2s ease;
}

/* Remove right border on last column (actions) for cleaner look */
table.custom-ant-table tbody td:last-child,
table.custom-ant-table thead th:last-child {
  border-right: none;
}

</style>
