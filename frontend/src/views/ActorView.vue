<script setup>
import { ref, onMounted, watch } from "vue";
import { deleteActor, getActors, createActor, updateActor } from "../services/actorService";
import Pagination from "@/components/Pagination.vue";
import ViewModal from "@/components/ViewModal.vue";
import DeleteModal from "@/components/DeleteModal.vue";
import CreateEditActorModal from "@/components/CreateEditActorModal.vue";

const actors = ref([]);
const totalCount = ref(0);

// Server-side parameters
const currentPage = ref(1);
const pageSize = ref(10);
const sortColumns = ref([]);

const searchColumn = ref("name"); // column to search
const searchTerm = ref("");        // search text

//view
const showViewModal = ref(false);
const actorToView = ref(null);

//create or edit
const showActorModal = ref(false);
const selectedActor = ref(null);
const formMode = ref("create");
const formErrors = ref({});

function addActor() {
  formMode.value = "create";
  selectedActor.value = null;
  showActorModal.value = true;
  formErrors.value = {};
}

function editActor(actor) {
  console.log("Selected actor to edit: ",actor);
  formMode.value = "edit";
  selectedActor.value = actor;
  showActorModal.value = true;
  formErrors.value = {};
}

function closeActorModal() {
  showActorModal.value = false;
}

async function handleActorSubmit(actorData) {
  try{
    formErrors.value = {};
    let data;
    if (formMode.value === "create") {
    // console.log(actorData);

    // call API to create actor
    data = await createActor(actorData);

    //  console.log(data);

    }else if(formMode.value==="edit") {
    // call API to update actor
    data = await updateActor(actorData);
    }

    if(data.isSuccess){
      await fetchActors(); // refresh actor list
      showActorModal.value = false; // close modal
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
const actorToDelete = ref(null);

async function fetchActors() {
  const data = await getActors({
    page: currentPage.value,
    pageSize: pageSize.value,
    sort: sortColumns.value,
    search: {
      searchColumn: searchColumn.value,
      searchTerm: searchTerm.value
    }
  });

  if (data.isSuccess) {
    actors.value = data.value.actors;
    totalCount.value = data.value.totalCount;
  }
}

// Fetch on mount
onMounted(fetchActors);

// Watchers to refetch data on changes
watch([currentPage, pageSize, searchColumn, searchTerm], fetchActors);

watch(sortColumns, fetchActors, { deep: true });

function sortColumn(column) {
  const col = sortColumns.value[0]; // only one column for now
  if (col?.sortKey === column) {
    col.sortAsc = !col.sortAsc; // toggle
  } else {
    sortColumns.value = [{ sortKey: column, sortAsc: true }]; // replace old sort
  }
  fetchActors(); // fetch immediately with new sort
}

function getSortArrow(column) {
  const col = sortColumns.value[0];
  if (!col || col.sortKey !== column) return '';
  return col.sortAsc ? '▲' : '▼';
}

// view modal setup
function openViewModal(actor){
  actorToView.value = actor;
  showViewModal.value = true;
}

function closeViewModal(){
  actorToView.value = null;
  showViewModal.value = false;
}

// delete modal setup
function openDeleteModal(actor) {
  actorToDelete.value = actor;
  showDeleteModal.value = true;
}

function closeDeleteModal() {
  actorToDelete.value = null;
  showDeleteModal.value = false;
}

async function confirmDelete() {
  if (!actorToDelete.value) return;

  try {
    const data = await deleteActor(actorToDelete.value);
    console.log("Deleted actor response:", data);

    await fetchActors(); // refresh the actor list
    closeDeleteModal();   // close the modal
  } catch (err) {
    console.error("Failed to delete actor:", err);
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
  <h1 class="text-gray-500 text-xl mb-4 mt-2">Actors</h1>
   <!-- Search box -->
  <div class="mb-4 flex justify-between">
    <div class="flex gap-2">
      <!-- Column filter -->
      <select
      v-model="searchColumn"
      class=" border-gray-200 rounded-full px-3 py-2 border"
      >
      <option value="id">Actor Id</option>
      <option value="name">Name</option>
      <option value="dateOfBirth">Date of Birth</option>
      <option value="bio">Bio</option>
    </select>

    <input
    type="text"
    v-model="searchTerm"
    placeholder="Search Actors..."
    class="form-control border p-2 w-72 rounded-full border-gray-300"
    />
  </div>

  <button
    class="mt-3 px-3 py-1 rounded-md bg-blue-600 text-white text-md hover:bg-blue-700 mr-10 cursor-pointer"
    @click="addActor"
  >
    Add Actor
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
                  Actor Id
                </span>
              </div>
            </th>
            <th @click="sortColumn('name')" style="cursor:pointer">
              Name
              <span>{{ getSortArrow('name') }}</span>
            </th>
            <th @click="sortColumn('bio')" style="cursor:pointer">
            Bio
            <span>{{ getSortArrow('bio') }}</span>
          </th>
             <th @click="sortColumn('dateOfBirth')" style="cursor:pointer">
                  Date Of Birth
                <span>{{ getSortArrow('dateOfBirth') }}</span>
            </th>
          <th>
            Movies
          </th>
          <th>Actions</th>

          </tr>
        </thead>
        <tbody>
          <tr v-for="(actor, index) in actors" :key="actor.id">
            <th>{{ index + 1 + (currentPage-1)*pageSize }}</th>
            <td  class="text-truncate" style="max-width: 150px;">{{ actor.id }}</td>
            <td  class="text-truncate" style="max-width: 150px;">{{ actor.name }}</td>
            <td  class="text-truncate" style="max-width: 150px;" :title="actor.description">{{ actor.bio }}</td>
            <td>{{ new Date(actor.dateOfBirth).toLocaleDateString() }}</td>
            <td>
              <div class="flex flex-wrap w-28 gap-1">
                <span v-for="(actor,index) in actor.movies" :key="index" :class="`border px-2 ${getRandomColor()} rounded-full italic text-gray-500`">{{ actor.title }}</span>
              </div>
            </td>
            <td>
              <div class="d-flex gap-1 flex-wrap">
                <button
                @click="openViewModal(actor)"
                class="btn btn-success btn-sm flex justify-center items-center hover:bg-opacity hover:bg-[#031b0d] transition-colors"
                style="border-radius:12px"
              >
                <span class="pi pi-eye"></span>
              </button>
                <button
                @click="editActor(actor)"
                 class="btn btn-primary btn-sm flex justify-center items-center hover:bg-[#0a143a] transition-colors"
                 style="border-radius:12px">
                 <span class="pi pi-pencil"></span>
                </button>
                <button
                      @click="openDeleteModal(actor)"
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
      v-bind:show="showViewModal"
      v-bind:item= "actorToView"
      mode = "actor"
      @close="closeViewModal"
   />

   <CreateEditActorModal
      v-if="showActorModal"
      :actor="selectedActor"
      :mode="formMode"
      :errors="formErrors"
      @submit="handleActorSubmit"
      @cancel="closeActorModal"
    />


    <!-- Delete Confirmation Modal -->
    <DeleteModal
      v-bind:show="showDeleteModal"
      v-bind:item="actorToDelete"
      mode="actor"
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

