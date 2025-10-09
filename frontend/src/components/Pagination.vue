<script setup>
defineProps({
  currentPage: Number,
  pageSize: Number,
  totalCount: Number
})

const emit = defineEmits(['update:currentPage', 'update:pageSize'])

</script>

<template>
  <div class="d-flex justify-content-between align-items-center mt-3">
    <div class="d-flex gap-4">
      <!-- Items per page selector -->
      <div>
        <label for="itemsPerPage">Items Per Page:</label>
        <select
          id="itemsPerPage"
          name="itemsPerPage"
          :value="pageSize"
          @change="emit('update:pageSize', Number($event.target.value))"
        >
            <option value="5">5</option>
            <option value="10">10</option>
            <option value="15">15</option>
            <option value="20">20</option>
        </select>
      </div>

      <!-- Showing X to Y of Z -->
      <div>
        Showing {{ (currentPage - 1) * pageSize + 1 }} to
        {{ Math.min(currentPage * pageSize, totalCount) }} of {{ totalCount }} movies
      </div>
    </div>

    <!-- Pagination buttons -->
    <div class="btn-group">
      <button
        class="btn btn-outline-primary"
        :disabled="currentPage === 1"
        @click="emit('update:currentPage', currentPage - 1)"
      >
        Prev
      </button>
      <button
        class="btn btn-outline-primary"
        :disabled="currentPage >= Math.ceil(totalCount / pageSize)"
        @click="emit('update:currentPage', currentPage + 1)"
      >
        Next
      </button>
    </div>
  </div>
</template>
