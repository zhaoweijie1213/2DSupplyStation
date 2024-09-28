<template>
  <v-app-bar :elevation="2">
    <template v-slot:prepend>
      <v-app-bar-nav-icon></v-app-bar-nav-icon>
    </template>
    <v-app-bar-title>二次元补给站</v-app-bar-title>
  </v-app-bar>
  <v-bottom-navigation v-if="mobile" grow>
    <v-btn value="favorites" @click="clickItem('HomkalStarRail3d')">
      <v-icon>mdi-heart</v-icon>
      <span>崩坏:星穹铁道</span>
    </v-btn>
    <v-btn value="nearby" @click="clickItem('GenshinImpact3d')">
      <v-icon>mdi-map-marker</v-icon>
      <span>原神</span>
    </v-btn>
    <v-btn value="recent" @click="clickItem('Uncategorlzed')">
      <v-icon>mdi-history</v-icon>
      <span>不分类</span>
    </v-btn>
  </v-bottom-navigation>
</template>

<script lang="ts" setup>
import { ImagesServiceProxy } from "@/api/api";
import { onMounted } from "vue";
import { useDisplay } from "vuetify";
const { mobile } = useDisplay();
onMounted(() => {
  console.log(mobile.value); // false
});

const clickItem = async (name: string) => {
  var res = await getImages(name);
};

async function getImages(name: string) {
  var client = new ImagesServiceProxy();
  var res = await client.list(name);
}
</script>
