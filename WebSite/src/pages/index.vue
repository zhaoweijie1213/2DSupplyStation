<template>
  <v-app-bar :elevation="2">
    <template v-slot:prepend>
      <v-app-bar-nav-icon></v-app-bar-nav-icon>
    </template>
    <v-app-bar-title>二次元补给站</v-app-bar-title>
  </v-app-bar>
  <v-bottom-navigation v-model="value" v-if="mobile" grow>
    <v-btn value="HomkalStarRail3d" @click="clickItem('HomkalStarRail3d')">
      <v-icon>mdi-heart</v-icon>
      <span>崩坏:星穹铁道</span>
    </v-btn>
    <v-btn value="GenshinImpact3d" @click="clickItem('GenshinImpact3d')">
      <v-icon>mdi-map-marker</v-icon>
      <span>原神</span>
    </v-btn>
    <v-btn value="Uncategorlzed" @click="clickItem('Uncategorlzed')">
      <v-icon>mdi-history</v-icon>
      <span>不分类</span>
    </v-btn>
  </v-bottom-navigation>
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
          <v-img :src="image.filePath" class="align-end" cover> </v-img>
          <v-card-title class="text-white text-center">{{
            image.fileName
          }}</v-card-title>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script lang="ts" setup>
import { onMounted, ref } from "vue";
import { ImageInfo, ImagesServiceProxy } from "@/api/api";
import { useDisplay } from "vuetify";
const { mobile } = useDisplay();
const images = ref<ImageInfo[]>([]);
const value = ref("HomkalStarRail3d");
onMounted(() => {
  console.log(mobile.value); // false
  clickItem("HomkalStarRail3d");
});

const clickItem = async (name: string) => {
  var res = await getImages(name);
  if (res.code == 0 && res.data) {
    images.value = res.data;
  }
};

async function getImages(name: string) {
  var client = new ImagesServiceProxy();
  var res = await client.list(name);
  return res;
}
</script>
<style lang="css" scoped>
@media (min-width: 768px) {
  .img_container {
    width: 80% !important;
  }
}
</style>
