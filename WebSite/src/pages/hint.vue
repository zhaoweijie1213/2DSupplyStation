<template>
  <v-container class="fill-height d-flex flex-column justify-center align-center text-center">
    <v-card class="pa-6" max-width="500" elevation="10" rounded="xl">
      <v-card-title class="text-h6 mb-4">
        请使用系统浏览器访问本站
      </v-card-title>

      <v-card-text>
        <div class="text-body-1 mb-6">
          检测到您正在使用 <strong>微信或 QQ 内置浏览器</strong>，为确保功能正常使用，
          <br><br>
          <strong>请复制本站地址</strong>，在手机系统自带浏览器（如 Safari、Chrome）中打开。
          <br><br>
          不支持在微信或 QQ 中直接访问。
        </div>

        <v-divider class="my-4"></v-divider>

        <div class="text-subtitle-2" style="color: #666;">
          操作方式：点击右上角“…” → <strong>选择「在浏览器中打开」</strong><br>
          或返回你获取链接的地方，<strong>长按复制地址</strong> → 粘贴到浏览器
        </div>
      </v-card-text>
    </v-card>
  </v-container>
</template>

<script setup lang="ts">
// 无需任何逻辑
</script>
