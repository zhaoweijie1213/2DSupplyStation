<template>
  <v-app-bar :elevation="2">
    <template v-slot:prepend>
      <v-app-bar-nav-icon></v-app-bar-nav-icon>
    </template>
    <v-app-bar-title></v-app-bar-title>
    <v-spacer></v-spacer>
    <template v-if="!mobile">
      <v-btn
        v-for="(menu, index) in menus"
        :key="index"
        :class="{ 'v-btn--active': menu.path === activeTab }"
        @click="emit('change-tab', menu.path)"
      >
        {{ menu.name }}
      </v-btn>
    </template>
  </v-app-bar>

  <v-bottom-navigation v-model="activeTabLocal" v-if="mobile" grow>
    <v-btn
      v-for="(menu, index) in menus"
      :key="index"
      :value="menu.path"
      @click="emit('change-tab', menu.path)"
    >
      <v-icon>mdi-heart</v-icon>
      <span>{{ menu.name }}</span>
    </v-btn>
  </v-bottom-navigation>
</template>

<script setup lang="ts">
  import { ref, watch } from "vue";
  import { useDisplay } from "vuetify";

  const props = defineProps<{ menus: any[]; activeTab: string }>();
  const emit = defineEmits(["change-tab"]);
  const { mobile } = useDisplay();

  const activeTabLocal = ref(props.activeTab);

  watch(
    () => props.activeTab,
    (newVal) => {
      activeTabLocal.value = newVal;
    }
  );

  watch(activeTabLocal, (newVal) => {
    emit("change-tab", newVal);
  });
</script>
