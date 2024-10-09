<template>
  <AppHead />
  <v-navigation-drawer v-show="!mobile">
    <v-list>
      <v-list-item
        v-for="(menu, index) in menus"
        :key="index"
        :title="menu.name"
        :active="menu.path == activeTab"
        @click="clickItem(menu.path)"
      ></v-list-item>
    </v-list>
  </v-navigation-drawer>
  <v-bottom-navigation v-model="activeTab" v-if="mobile" grow>
    <v-btn
      v-for="(menu, index) in menus"
      :key="index"
      :value="menu.path"
      @click="clickItem(menu.path)"
    >
      <v-icon>mdi-heart</v-icon>
      <span>{{ menu.name }}</span>
    </v-btn>
  </v-bottom-navigation>
  <ImagesContainer :images="images" />
</template>

<script lang="ts" setup>
import { onMounted, ref } from "vue";
import { useRoute } from "vue-router";
import { ImageInfo, ImagesServiceProxy, MenuConfig } from "@/api/api";
import { useDisplay } from "vuetify";
import AppHead from "@/components/AppHead.vue";
import ImagesContainer from "@/components/ImagesContainer.vue";
const { mobile } = useDisplay();
const route = useRoute();
const images = ref<ImageInfo[]>([]);
const menus = ref<MenuConfig[]>([]);
const activeTab = ref("");
const auth: string = route.query.auth as string;
onMounted(async () => {
  console.log(mobile.value); // false
  await getMenus();
  activeTab.value = menus.value[0].path;
  clickItem(menus.value[0].path);
});

const clickItem = async (name: string) => {
  var res = await getImages(name);
  if (res.code == 0 && res.data) {
    images.value = res.data;
  }
};

async function getMenus() {
  var client = new ImagesServiceProxy();
  var res = await client.menus(auth);
  if (res.code == 0 && res.data) {
    menus.value = res.data;
  }
}

async function getImages(name: string) {
  var client = new ImagesServiceProxy();
  var res = await client.list(name, auth);
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
