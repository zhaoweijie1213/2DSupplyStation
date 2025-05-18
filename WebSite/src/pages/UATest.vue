<template>
  <v-container fluid class="pa-4">
    <v-card elevation="4" rounded="lg">
      <v-card-title class="text-h6">UA & 环境检测测试</v-card-title>
      <v-card-text>
        <v-list dense>
          <v-list-item>
            <v-list-item-content>
              <v-list-item-title class="font-weight-bold"
                >完整 UA：</v-list-item-title
              >
              <v-list-item-subtitle class="monospace">{{
                ua
              }}</v-list-item-subtitle>
            </v-list-item-content>
            <v-list-item-action>
              <v-btn icon @click="copyUA" :title="'复制 UA'">
                <v-icon>mdi-content-copy</v-icon>
              </v-btn>
            </v-list-item-action>
          </v-list-item>

          <v-divider class="my-2"></v-divider>

          <v-list-item>
            <v-list-item-content>
              <v-list-item-title class="font-weight-bold"
                >检测结果：</v-list-item-title
              >
              <v-list-item-subtitle>
                <div>
                  是否微信内置浏览器：<strong>{{ isWeChat }}</strong>
                </div>
                <div>
                  是否QQ内置浏览器：<strong>{{ isQQ }}</strong>
                </div>
                <div>
                  是否淘宝内置浏览器：<strong>{{ isTaobao }}</strong>
                </div>
                <div>
                  是否天猫内置浏览器：<strong>{{ isTmall }}</strong>
                </div>
                <div>
                  是否支付宝内置浏览器：<strong>{{ isAlipay }}</strong>
                </div>
              </v-list-item-subtitle>
            </v-list-item-content>
          </v-list-item>

          <v-divider class="my-2"></v-divider>

          <v-list-item>
            <v-list-item-content>
              <v-list-item-title class="font-weight-bold"
                >当前环境：</v-list-item-title
              >
              <v-list-item-subtitle>{{ source }}</v-list-item-subtitle>
            </v-list-item-content>
          </v-list-item>
        </v-list>
      </v-card-text>
    </v-card>

    <v-snackbar v-model="copied" timeout="2000">UA 已复制到剪贴板</v-snackbar>
  </v-container>
</template>

<script setup lang="ts">
  import { ref, computed } from "vue";

  const ua = navigator.userAgent.toLowerCase();

  const isWeChat = computed(() => ua.includes("micromessenger"));
  const isQQ = computed(() => ua.includes(" qq") || ua.includes("qqbrowser"));
  const isTaobao = computed(
    () => ua.includes("aliapp(tb/") || ua.includes("aliapp(tm/")
  );
  const isTmall = computed(() => ua.includes("aliapp(tm/"));
  const isAlipay = computed(() => ua.includes("alipayclient"));

  const source = computed(() => {
    if (isWeChat.value) return "微信内置浏览器";
    if (isQQ.value) return "QQ 内置浏览器";
    if (isTaobao.value) return "淘宝内置浏览器";
    if (isTmall.value) return "天猫内置浏览器";
    if (isAlipay.value) return "支付宝内置浏览器";
    return "系统/第三方浏览器";
  });

  const copied = ref(false);
  const copyUA = async () => {
    try {
      await navigator.clipboard.writeText(ua);
      copied.value = true;
    } catch (e) {
      alert("复制失败，请手动复制 UA");
    }
  };
</script>

<style scoped>
  .monospace {
    font-family: "Courier New", Courier, monospace;
    font-size: 0.85rem;
    word-break: break-all;
  }
</style>
