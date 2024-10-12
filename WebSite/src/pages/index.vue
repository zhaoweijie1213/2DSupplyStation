<template>
  <AppHead />
  <v-navigation-drawer v-model="drawer">
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

  <ImagesContainer :product="activeTab" :hidden="false" />
</template>

<script lang="ts" setup>
import { onMounted, ref, watch } from "vue";
import { useRoute } from "vue-router";
import { ImagesServiceProxy, MenuConfig } from "@/api/api";
import { useDisplay } from "vuetify";
import AppHead from "@/components/AppHead.vue";
import ImagesContainer from "@/components/ImagesContainer.vue";

const { mobile } = useDisplay();
const route = useRoute();
const menus = ref<MenuConfig[]>([]);
const activeTab = ref("");
const drawer = ref(!mobile.value);
const auth: string = route.query.auth as string;
onMounted(async () => {
  console.log(mobile.value); // false
  await getMenus();
  clickItem(menus.value[0].path);
});

watch(
  () => mobile,
  (newValue) => {
    drawer.value = !newValue.value;
  }
);

const clickItem = async (name: string) => {
  activeTab.value = name;
};

async function getMenus() {
  var client = new ImagesServiceProxy();
  var res = await client.menus(auth);
  if (res.code == 0 && res.data) {
    menus.value = res.data;
  }
}
</script>
<style lang="css" scoped>
@media (min-width: 768px) {
  .img_container {
    width: 80% !important;
  }
}
</style>
