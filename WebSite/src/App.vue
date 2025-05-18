<template>
  <v-app>
    <!-- 全局 Loading 遮罩 -->
    <v-overlay :model-value="loading" persistent class="global-loading" z-index="9999">
      <div class="loading-container d-flex align-center justify-center">
        <v-progress-circular
          indeterminate
          color="primary"
          size="64"
          width="6"
        />
      </div>
    </v-overlay>
      <router-view />
  </v-app>
</template>

<script lang="ts" setup>
  import { isLoading } from '@/store/loading'
  import { computed } from 'vue'
  const loading = computed(() => isLoading.value)
</script>
<style scoped>
  .global-loading {
    backdrop-filter: blur(4px);
    background-color: rgba(255, 255, 255, 0.5);
  }

  .loading-container {
    min-height: 100vh;
    min-width: 100vw;
  }


</style>
