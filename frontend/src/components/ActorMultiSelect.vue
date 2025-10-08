<script setup>
import { onMounted, ref,computed } from 'vue'
import Multiselect from 'vue-multiselect'
import { getAllActors } from '@/services/actorService';

const props = defineProps({
  selectedActors: {type: Array, default:[]}
})

const emit = defineEmits(['update:selectedActors']);

let options = ref([]);

onMounted(async () => {
  try {
    const response = await getAllActors();
    if (response?.value) {
      options.value = response.value.map(actor => ({ id: actor.id, name: actor.name }));
    }
  } catch (err) {
    console.error("Failed to load actors", err);
  }
});


const internalValue = computed({
  get: ()=>props.selectedActors,
  set:(val)=>emit('update:selectedActors',val)
})



</script>

<style src="vue-multiselect/dist/vue-multiselect.min.css"></style>

<template>
  <div class="mt-5"><label class="typo__label text-gray-400 font-light text-sm">Select Actors</label>
    <multiselect v-if="options.length" id="multiselect" v-model="internalValue" :options="options" :multiple="true" :close-on-select="false" :clear-on-select="false"
                 :preserve-search="true" placeholder="Actors" label="name" track-by="id" :preselect-first="true">
      <template #selection="{ values, isOpen }">
        <span class="multiselect__single"
              v-if="values.length"
              v-show="!isOpen">{{ values.length }} options selected</span>
      </template>
    </multiselect>
    <!-- <pre class="language-json"><code>{{ value }}</code></pre> -->
  </div>
</template>

