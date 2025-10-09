<script setup>
import { onMounted, ref,computed, watch } from 'vue'
import Multiselect from 'vue-multiselect'
import { getAllMovies } from '@/services/movieService';

const props = defineProps({
  selectedMovies: {type: Array, default:[]},
  editSelectedMovies:{type:Array, default:[]},
  mode:{type:String,default:"create"}
})

const emit = defineEmits(['update:selectedMovies']);

const localMovies = ref([]);

watch(
  ()=>props.mode,
  ()=>{
    if(props.mode === 'edit'){
      localMovies.value = [...props.editSelectedMovies];
    }
    if(props.mode === 'create'){
      console.log(props.selectedMovies);
      localMovies.value = [...props.selectedMovies];
    }
  },
 {immediate:true}
)

let options = ref([]);

onMounted(async () => {
  try {
    const response = await getAllMovies();
    if (response?.value) {
      options.value = response.value.map(movie => ({ id: movie.id, name: movie.title }));
    }
  } catch (err) {
    console.error("Failed to load movies", err);
  }
});


const internalValue = computed({
  get: ()=>
  {
    console.log(localMovies.value)
    return localMovies.value;
  },
  set:(val)=>{
    localMovies.value=val;
    emit('update:selectedMovies',val)
  }
})


</script>

<style src="vue-multiselect/dist/vue-multiselect.min.css"></style>

<template>
  <div class="mt-5"><label class="typo__label text-gray-400 font-light text-sm">Select Movies</label>
    <multiselect v-if="options.length" id="multiselect" v-model="internalValue" :options="options" :multiple="true" :close-on-select="false" :clear-on-select="false"
                 :preserve-search="true" placeholder="Movies" label="name" track-by="id" :preselect-first="true">
      <template #selection="{ values, isOpen }">
        <span class="multiselect__single"
              v-if="values.length"
              v-show="!isOpen">{{ values.length }} options selected</span>
      </template>
    </multiselect>
    <!-- <pre class="language-json"><code>{{ value }}</code></pre> -->
  </div>
</template>

