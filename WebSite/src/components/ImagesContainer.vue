<template>
  <v-container class="img_container" fluid>
    <v-row dense>
      <v-col
        v-for="(image, index) in images"
        :key="index"
        cols="6"
        sm="4"
        md="4"
        lg="3"
        xl="2"
        xxl="2"
      >
        <v-card>
          <img :src="image.filePath" class="align-end img-custom" cover />
          <v-card-title class="text-white text-center"
            >编号：{{ image.fileName }}</v-card-title
          >
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>
<script setup lang="ts">
import { ImageInfo } from "@/api/api";

defineProps<{
  images: ImageInfo[];
}>();
</script>
<style scoped>
.img-custom {
  width: 100%;
}
</style>
