<script setup>
import { ref, watch, defineProps, defineEmits } from "vue";

const props = defineProps({
  movie: Object, // existing movie for edit
  mode: { type: String, default: "create" }, // "create" or "edit"
  errors: {type:Object, default: ()=>({})}
});

const emit = defineEmits(["submit", "cancel"]);

const form = ref({
  title: "",
  description: "",
  releaseDate: "",
  durationMinutes: "",
  rating: ""
});

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
  () => props.movie,
  (newMovie) => {
    if (newMovie) {
      form.value = {
        title: newMovie.title,
        description: newMovie.description,
        releaseDate: formatDateForInput(newMovie.releaseDate),
        durationMinutes: newMovie.durationMinutes,
        rating: newMovie.rating
      };
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
  emit("submit", { ...form.value, id: props.mode==="edit"? props.movie?.id:""});
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
      <h2 class="text-xl font-semibold mb-6">{{ mode === 'edit' ? 'Edit Movie' : 'Create Movie' }}</h2>

      <form class="grid sm:grid-cols-2 gap-8">
        <!-- Title -->
        <div class="relative">
          <input
            type="text"
            placeholder="Title"
            v-model="form.title"
            class="px-2 py-3 bg-white text-slate-900 w-full text-sm border-b-2 border-gray-200 focus:border-[#007bff] outline-none"
          />
          <!-- ✅ show title error -->
          <p v-if="errors.title" class="absolute -bottom-5 px-2 text-xs text-red-500">
            {{ errors.title }}
          </p>
        </div>

              <!-- Description -->
        <div class="relative sm:col-span-2">
          <textarea
            placeholder="Description"
            v-model="form.description"
            class="px-2 py-3 bg-white text-slate-900 w-full text-sm border-b-2 border-gray-200 focus:border-[#007bff] outline-none"
          ></textarea>
          <!-- show description error -->
          <p v-if="errors.description" class="absolute -bottom-5 px-2 text-xs text-red-500">
            {{ errors.description }}
          </p>
        </div>

        <!-- Release Date -->
        <div class="relative">
          <input
            type="date"
            v-model="form.releaseDate"
            class="px-2 py-3 bg-white text-slate-900 w-full text-sm border-b-2 border-gray-200 focus:border-[#007bff] outline-none"
          />
          <!-- show releaseDate error -->
          <p v-if="errors.releasedate" class="absolute -bottom-5 px-2 text-xs text-red-500">
            {{ errors.releasedate }}
          </p>
        </div>

        <!-- Duration -->
        <div class="relative">
          <input
            type="number"
            placeholder="Duration (minutes)"
            v-model="form.durationMinutes"
            class="px-2 py-3 bg-white text-slate-900 w-full text-sm border-b-2 border-gray-200 focus:border-[#007bff] outline-none"
          />
          <!-- show durationMinutes error -->
          <p v-if="errors.durationminutes" class="absolute -bottom-5 px-2 text-xs text-red-500">
            {{ errors.durationminutes }}
          </p>
        </div>

        <!-- Rating -->
        <div class="relative">
          <input
            type="number"
            step="0.1"
            placeholder="Rating"
            v-model="form.rating"
            class="px-2 py-3 bg-white text-slate-900 w-full text-sm border-b-2 border-gray-200 focus:border-[#007bff] outline-none"
          />
          <!-- show rating error -->
          <p v-if="errors.rating" class="absolute -bottom-5 px-2 text-xs text-red-500">
            {{ errors.rating }}
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
