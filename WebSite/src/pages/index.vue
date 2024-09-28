<template>
  <v-app-bar :elevation="2">
    <template v-slot:prepend>
      <v-app-bar-nav-icon></v-app-bar-nav-icon>
    </template>
    <v-app-bar-title>Application Bar</v-app-bar-title>
  </v-app-bar>
  <v-bottom-navigation>
    <v-btn value="recent">
      <v-icon>mdi-history</v-icon>

      <span>鸣潮</span>
    </v-btn>

    <v-btn value="favorites">
      <v-icon>mdi-heart</v-icon>

      <span>崩坏:星穹铁道</span>
    </v-btn>

    <v-btn value="nearby">
      <v-icon>mdi-map-marker</v-icon>

      <span>原神</span>
    </v-btn>
  </v-bottom-navigation>
  <HelloWorld />
</template>

<script lang="ts" setup>
import { onMounted } from "vue";
import { useDisplay } from "vuetify";
const { mobile } = useDisplay();
onMounted(() => {
  console.log(mobile.value); // false
});
</script>
