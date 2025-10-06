<script setup>
import { defineProps, defineEmits } from "vue";

const props = defineProps({
  show: Boolean,
  movie: Object
});

const emit = defineEmits(["close"]);

function handleClose() {
  emit("close");
}
</script>

<template>
  <div v-if="show" class="fixed inset-0 flex justify-center items-center w-full h-full z-[1000] overflow-auto">
    <!-- Overlay -->
    <div
      class="fixed inset-0 w-full h-full bg-[rgba(0,0,0,0.5)]"
      @click="handleClose"
    ></div>

    <!-- Modal content -->
    <div class="relative w-full max-w-lg bg-white shadow-lg rounded-lg p-6 z-10">
      <!-- Header -->
      <div class="flex items-center pb-3 border-b border-gray-300">
        <h3 class="text-slate-900 text-xl font-semibold flex-1">{{ movie?.title || 'Movie Details' }}</h3>
        <svg
          @click="handleClose"
          xmlns="http://www.w3.org/2000/svg"
          class="w-3.5 h-3.5 ml-2 cursor-pointer shrink-0 fill-gray-400 hover:fill-red-500"
          viewBox="0 0 320.591 320.591"
        >
          <path d="M30.391 318.583a30.37 30.37 0 0 1-21.56-7.288c-11.774-11.844-11.774-30.973 0-42.817L266.643 10.665c12.246-11.459 31.462-10.822 42.921 1.424 10.362 11.074 10.966 28.095 1.414 39.875L51.647 311.295a30.366 30.366 0 0 1-21.256 7.288z"></path>
          <path d="M287.9 318.583a30.37 30.37 0 0 1-21.257-8.806L8.83 51.963C-2.078 39.225-.595 20.055 12.143 9.146c11.369-9.736 28.136-9.736 39.504 0l259.331 257.813c12.243 11.462 12.876 30.679 1.414 42.922-.456.487-.927.958-1.414 1.414a30.368 30.368 0 0 1-23.078 7.288z"></path>
        </svg>
      </div>

      <!-- Body -->
      <div class="my-6 text-slate-600 text-sm leading-relaxed">
        <p><strong>Id:</strong> {{ movie?.id }}</p>
        <p  class="mt-2"><strong>Title:</strong> {{ movie?.title }}</p>
        <p class="mt-2"><strong>Description:</strong> {{ movie?.description }}</p>
        <p class="mt-2"><strong>Release Date:</strong> {{ movie?.releaseDate ? new Date(movie.releaseDate).toLocaleDateString() : '-' }}</p>
        <p class="mt-2"><strong>Duration:</strong> {{ movie?.durationMinutes }} min</p>
        <p class="mt-2"><strong>Rating:</strong> {{ movie?.rating }}</p>
      </div>

      <!-- Footer -->
      <div class="border-t border-gray-300 pt-6 flex justify-end gap-4">
        <button
          @click="handleClose"
          class="px-4 py-2 rounded-lg text-slate-900 text-sm font-medium bg-gray-200 hover:bg-gray-300"
        >
          Close
        </button>
      </div>
    </div>
  </div>
</template>
