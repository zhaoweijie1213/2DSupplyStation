<template>
  <v-container class="fill-height d-flex flex-column justify-center align-center text-center">
    <v-card class="pa-6" max-width="500" elevation="10" rounded="xl">
      <v-card-title class="text-h6 mb-4">
        请使用系统浏览器访问本站
      </v-card-title>

      <v-card-text>
        <!-- 原始链接展示区 -->
        <!-- 改为 readonly textarea，移动端友好选中 -->
        <v-textarea
          ref="urlEl"
          v-model="originalUrl"
          readonly
          color="#f5f5f5"
          variant="outlined"
          class="url-display mb-4"
          rows="1"
          auto-grow
          @click="selectOriginal"
        ></v-textarea>

        <!-- 复制按钮 -->
        <v-btn
          color="primary"
          class="mb-4"
          @click="copyOriginal"
          rounded
        >
          <v-icon left>mdi-content-copy</v-icon>
          复制原始链接
        </v-btn>

        <v-divider class="my-4"></v-divider>

        <!-- 说明文案 -->
        <!-- 使用 Vuetify 文本排版类 -->
      <div class="text-center">
        <p class="text-body-2 mb-2">
          检测到您正在使用 <strong>{{ source }}</strong> 内置浏览器，功能受限。
        </p>
        <p class="text-body-2 mb-2">
          请点击上方链接或 “复制原始链接” 按钮复制链接，<br>
          然后打开手机系统浏览器(如 Safari、Chrome),在地址栏粘贴并访问该链接。
        </p>
      </div>
      </v-card-text>
    </v-card>

    <!-- 成功复制提示 -->
    <v-snackbar
      v-model="copied"
      color="success"
      rounded="lg"
      timeout="2000"
      location="bottom"
      multi-line
      class="text-center ma-2"
    >
      链接已复制到剪贴板
      <template v-slot:actions>
        <v-btn variant="text" @click="copied = false">知道了</v-btn>
      </template>
    </v-snackbar>
  </v-container>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue'
import { useRoute } from 'vue-router'

const route = useRoute()
// 拿到原始目标路径
const redirectPath = (route.query.redirect as string) || '/'
const originalUrl = ref(window.location.origin + redirectPath)

const copied = ref(false)
async function copyOriginal() {
  const text = originalUrl.value
  // 优先尝试 Clipboard API
  if (navigator.clipboard && navigator.clipboard.writeText) {
    try {
      await navigator.clipboard.writeText(text)
      copied.value = true
      return
    } catch (e) {
      // Clipboard API 不可用或失败，继续走后面的 execCommand
    }
  }
  // fallback: execCommand
  const el = urlEl.value!
  el.focus()
  el.select()
  try {
    const successful = document.execCommand('copy')
    if (successful) {
      copied.value = true
    } else {
      throw new Error('execCommand 复制失败')
    }
  } catch {
    alert('复制失败，请手动长按复制上方链接')
  }
}

// 选中 textarea 内容
const urlEl = ref<HTMLTextAreaElement>()
function selectOriginal() {
  const el = urlEl.value
  if (!el) return
  el.focus()
  el.select()
}

// UA 检测
const ua = navigator.userAgent.toLowerCase()
const isWeChat = ua.includes('micromessenger')
const isQQ     = ua.includes(' qq') || ua.includes('qqbrowser')
const isTaobao = ua.includes('aliapp(tb/') || ua.includes('aliapp(tm/')
const isTmall  = ua.includes('aliapp(tm/')
const isAlipay = ua.includes('alipayclient')

const source = computed(() => {
  if (isWeChat) return '微信'
  if (isQQ)     return 'QQ'
  if (isTaobao) return '淘宝'
  if (isTmall)  return '天猫'
  if (isAlipay) return '支付宝'
  return '内置浏览器'
})
</script>

<style scoped>
.url-display {
  word-break: break-all;
  font-family: 'Courier New', Courier, monospace;
  user-select: all;
  background-color: #f5f5f5;
  color: black;
  border-radius: 4px;
  overflow-x: auto;
}
</style>
