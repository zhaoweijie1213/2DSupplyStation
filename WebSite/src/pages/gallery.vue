<template>
  <v-app-bar :elevation="2">
    <template v-slot:prepend>
      <v-app-bar-nav-icon></v-app-bar-nav-icon>
    </template>
    <v-app-bar-title></v-app-bar-title>
    <v-spacer></v-spacer>
    <!-- 桌面端菜单按钮 -->
    <template v-if="!mobile">
      <v-btn
        v-for="(menu, index) in menus"
        :key="index"
        :class="{ 'v-btn--active': menu.path === activeTab }"
        @click="clickItem(menu.path)"
      >
        {{ menu.name }}
      </v-btn>
    </template>
  </v-app-bar>
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
  <v-main>
    <ImagesContainer :product="activeTab" :hidden="false" />
  </v-main>
</template>

<script lang="ts" setup>
  import { onMounted, ref, watch } from "vue";
  import { useRoute } from "vue-router";
  import { ImagesServiceProxy, MenuConfig } from "@/api/api";
  import { useDisplay } from "vuetify";
  // import AppHead from "@/components/AppHead.vue";
  import ImagesContainer from "@/components/ImagesContainer.vue";
  import { isLoading } from "@/store/loading"; // 引入全局 loading 状态

  const { mobile } = useDisplay();
  const route = useRoute();
  const menus = ref<MenuConfig[]>([]);
  const activeTab = ref("");
  const drawer = ref(!mobile.value);
  const auth: string = route.query.auth as string;
  onMounted(async () => {
    console.log(mobile.value); // false
    await getMenus();
    if (menus.value.length > 0) {
      clickItem(menus.value[0].path);
    }
  });

  watch(
    () => mobile,
    (newValue) => {
      drawer.value = !newValue.value;
    }
  );

  const clickItem = async (name: string) => {
    if (activeTab.value === name) return;
    isLoading.value = true;
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
