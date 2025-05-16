/**
 * router/index.ts
 *
 * Automatic routes for `./src/pages/*.vue`
 */

// Composables
import { createRouter, createWebHistory } from 'vue-router/auto'
import { routes } from 'vue-router/auto-routes'
import { isLoading } from '@/store/loading'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes,
})

// Workaround for https://github.com/vitejs/vite/issues/11804
router.onError((err, to) => {
  if (err?.message?.includes?.('Failed to fetch dynamically imported module')) {
    if (!localStorage.getItem('vuetify:dynamic-reload')) {
      console.log('Reloading page to fix dynamic import error')
      localStorage.setItem('vuetify:dynamic-reload', 'true')
      location.assign(to.fullPath)
    } else {
      console.error('Dynamic import error, reloading page did not fix it', err)
    }
  } else {
    console.error(err)
  }
})

router.isReady().then(() => {
  localStorage.removeItem('vuetify:dynamic-reload')
})

router.beforeEach((to, from, next) => {


 isLoading.value = true

  const ua = navigator.userAgent.toLowerCase();

  const isWeChat = ua.includes('micromessenger');
  const isQQ = ua.includes('qq/') || ua.includes('qqbrowser');
  const isInnerBrowser = isWeChat || isQQ;

  const isTencentBot = ua.includes('tencent') || ua.includes('qcloud') || ua.includes('qqbrowser') || ua.includes('tencenttraveler') || ua.includes('security-crawler');
  const isSearchEngineBot = ua.includes('spider') || ua.includes('googlebot') || ua.includes('baiduspider') || ua.includes('bingbot') || ua.includes('sogou') || ua.includes('360spider');

  const isBotVisitor = isTencentBot || isSearchEngineBot;

  const requireExternalBrowser = to.meta.requireExternalBrowser;

  if ((isInnerBrowser || isBotVisitor) && requireExternalBrowser) {
    // 避免死循环跳转
    if (to.name !== 'hint') {
      next({ name: 'hint' });
    } else {
      next();
    }
  } else {
    next();
  }
});

// 加载完成
router.afterEach(() => {
  setTimeout(() => {
    isLoading.value = false
  }, 200) // 延迟一点避免闪烁
})

export default router
