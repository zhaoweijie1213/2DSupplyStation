<template>
  <AppHead />
  <ImagesContainer :images="images" />
</template>

<script lang="ts" setup>
import { onMounted, ref } from "vue";
import { useRoute } from "vue-router";
import { ImageInfo, ImagesServiceProxy } from "@/api/api";
import { useDisplay } from "vuetify";
import AppHead from "@/components/AppHead.vue";
import ImagesContainer from "@/components/ImagesContainer.vue";
const { mobile } = useDisplay();
const images = ref<ImageInfo[]>([]);
const route = useRoute();

const auth: string = route.query.auth as string;

onMounted(async () => {
  console.log(mobile.value); // false
  await getImages();
});

async function getImages() {
  var client = new ImagesServiceProxy();
  var res = await client.hid(auth);
  if (res.code == 0 && res.data) images.value = res.data;
}
</script>
<style lang="css" scoped>
@media (min-width: 768px) {
  .img_container {
    width: 80% !important;
  }
}
</style>
