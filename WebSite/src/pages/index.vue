<template>
  <v-container fluid class="pa-0">
    <v-img
      :src="bgImage"
      cover
      height="100vh"
      width="100%"
      class="d-flex align-center justify-center"
    >
      <div class="overlay d-flex justify-center align-center">
        <v-btn
          color="primary"
          class="elevation-12"
          @click="goToGallery"
          style="width: 90vw; max-width: 500px; height: 80px; font-size: 24px; border-radius: 12px;"
        >
          进入图库
        </v-btn>
      </div>
    </v-img>
  </v-container>
</template>

<script setup lang="ts">
import { useRouter } from 'vue-router'
import bgImage from '@/assets/bg.png' // 替换成你自己的背景图路径

const router = useRouter()
const goToGallery = () => {
  router.push('/gallery')
}
</script>

<style scoped>
.overlay {
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background: linear-gradient(to bottom, rgba(0, 0, 0, 0.2), rgba(0, 0, 0, 0.7));
}
</style>
