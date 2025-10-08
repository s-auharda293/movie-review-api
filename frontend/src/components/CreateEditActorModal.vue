<script setup>
import { ref, watch} from "vue";
import MovieMultiSelect from "./MovieMultiSelect.vue";

const props = defineProps({
  actor: Object, // existing actor for edit
  mode: { type: String, default: "create" }, // "create" or "edit"
  errors: {type:Object, default: ()=>({})}
});

const emit = defineEmits(["submit", "cancel"]);

const form = ref({
  name: "",
  bio: "",
  dateOfBirth: ""
});

const selectedMovies = ref([]);
const editSelectedMovies = ref([]);

const errors = ref({});

//watch for any errors
watch(
  () => props.errors, // run this watcher when the props.errors changes
  (newErrors) => {
    errors.value = newErrors || {};
  },
  { immediate: true } // run this watcher once when the component is created/mounted
);

// Prefill form when editing
watch(
  () => props.actor,
  (newActor) => {
    if (newActor) {
      form.value = {
        name: newActor.name,
        bio: newActor.bio,
        dateOfBirth: formatDateForInput(newActor.dateOfBirth)
      };

       if(props.mode==="edit" && newActor?.movies){
         editSelectedMovies.value = newActor.movies;
      }
    }
  },
  { immediate: true }
);

function formatDateForInput(isoString) {
  if (!isoString) return "";
  const date = new Date(isoString);
  const yyyy = date.getFullYear();
  const mm = String(date.getMonth() + 1).padStart(2, "0"); // months are 0-indexed
  const dd = String(date.getDate()).padStart(2, "0");
  return `${yyyy}-${mm}-${dd}`;
}


function handleSubmit() {
  const formValue = { ...form.value, movieIds: selectedMovies.value.map((movie)=>movie.id), id: props.mode==="edit"? props.actor?.id:""}
  emit("submit", formValue );
}

function handleCancel() {
  emit("cancel");
}
</script>

<template>
  <!-- Modal overlay -->
  <div class="fixed inset-0 flex justify-center items-center z-[1000]">
    <div class="fixed inset-0 bg-[rgba(0,0,0,0.5)]" @click="handleCancel"></div>

    <!-- Modal content -->
    <div class="relative bg-white rounded-lg shadow-lg w-full max-w-4xl p-6 z-10">
      <h2 class="text-xl font-semibold mb-6">{{ mode === 'edit' ? 'Edit Actor' : 'Create Actor' }}</h2>

      <form class="grid sm:grid-cols-2 gap-8">
        <!-- Name -->
        <div class="relative">
          <input
            type="text"
            placeholder="Name"
            v-model="form.name"
            class="px-2 py-3 bg-white text-slate-900 w-full text-sm border-b-2 border-gray-200 focus:border-[#007bff] outline-none"
          />
          <!-- ✅ show name error -->
          <p v-if="errors.name" class="absolute -bottom-5 px-2 text-xs text-red-500">
            {{ errors.name }}
          </p>
        </div>

              <!-- Bio -->
        <div class="relative sm:col-span-2">
          <textarea
            placeholder="Bio"
            v-model="form.bio"
            class="px-2 py-3 bg-white text-slate-900 w-full text-sm border-b-2 border-gray-200 focus:border-[#007bff] outline-none"
          ></textarea>
          <!-- show bio error -->
          <p v-if="errors.bio" class="absolute -bottom-5 px-2 text-xs text-red-500">
            {{ errors.bio }}
          </p>
        </div>

        <!-- Date of Birth -->
        <div class="relative">
          <input
            type="date"
            v-model="form.dateOfBirth"
            class="px-2 py-3 bg-white text-slate-900 w-full text-sm border-b-2 border-gray-200 focus:border-[#007bff] outline-none mt-9"
          />
          <!-- show dateOfBirth error -->
          <p v-if="errors.dateofbirth" class="absolute -bottom-5 px-2 text-xs text-red-500">
            {{ errors.dateofbirth }}
          </p>
        </div>

         <!-- Movie Selection -->
      <div class="relative mb-5">
        <MovieMultiSelect v-model:selectedMovies="selectedMovies" v-if="mode==='create'"/>
        <MovieMultiSelect v-model:selectedMovies="selectedMovies" :editSelectedMovies="editSelectedMovies" v-if="mode==='edit'" :mode="mode"/>

        <!-- show movie error -->
        <p v-if="errors.actorIds" class="absolute -bottom-5 px-2 text-xs text-red-500">
          {{ errors.movieIds }}
        </p>
      </div>

      </form>

      <!-- Buttons -->
      <div class="flex justify-end gap-4 mt-8">
        <button
          @click="handleCancel"
          type="button"
          class="px-6 py-2 rounded-sm font-medium text-sm bg-gray-200 text-slate-900 hover:bg-gray-300"
        >
          Cancel
        </button>
        <button
          @click="handleSubmit"
          type="button"
          class="px-6 py-2 rounded-sm font-medium text-sm bg-[#007bff] text-white hover:bg-[#006bff]"
        >
          {{ mode === 'edit' ? 'Update' : 'Create' }}
        </button>
      </div>
    </div>
  </div>
</template>
