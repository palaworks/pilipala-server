import Vue from 'vue'
import App from './App.vue'
import vuetify from './plugins/vuetify'
import router from './router'
import axios from 'axios'
import glob from './glob'

Vue.prototype.glob = glob

Vue.config.productionTip = false
Vue.prototype.$axios = axios

new Vue({
  axios,
  vuetify,
  router,
  render: h => h(App)
}).$mount('#app')