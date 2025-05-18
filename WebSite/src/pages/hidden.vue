<template>
  <AppHead />
  <ImagesContainer :product="''" :hidden="true" />
</template>

<script lang="ts" setup>
  import { onMounted } from "vue";
  import { useDisplay } from "vuetify";
  import AppHead from "@/components/AppHead.vue";
  import ImagesContainer from "@/components/ImagesContainer.vue";
  const { mobile } = useDisplay();

  onMounted(async () => {
    console.log(mobile.value); // false
  });
</script>
<style lang="css" scoped>
  @media (min-width: 768px) {
    .img_container {
      width: 80% !important;
    }
  }
</style>
