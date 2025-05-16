/**
 * router/index.ts
 *
 * Automatic routes for `./src/pages/*.vue`
 */

// Composables
import { createRouter, createWebHistory } from 'vue-router/auto'
import { routes } from 'vue-router/auto-routes'

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
  const ua = navigator.userAgent.toLowerCase();
  const isWeChat = ua.includes('micromessenger');
  const isQQ = ua.includes('qq/') || ua.includes('qqbrowser');
  const isTaobao = ua.includes('aliapp(tb/') || ua.includes('aliapp(tm/')
  const isAlipay = ua.includes('alipayclient') // 如需拦截支付宝可加

  const isInnerBrowser = isWeChat || isQQ || isTaobao || isAlipay;

  if (isInnerBrowser) {
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


export default router
