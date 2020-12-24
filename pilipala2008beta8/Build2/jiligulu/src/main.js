import Vue from 'vue'
import App from './App.vue'
import vuetify from './plugins/vuetify'
import router from './router'
import axios from 'axios'
import glob from './glob'
import JSEncrypt from 'jsencrypt'

/* 全局常量 */
Vue.prototype.glob = glob

Vue.config.productionTip = false
Vue.prototype.$axios = axios

/* jsencrypt加密 */
Vue.prototype.$encrypt = function (pub, plain_text) {
  var encrypt = new JSEncrypt();
  encrypt.setPublicKey(pub);
  return encrypt.encrypt(plain_text);
}
/* jsencrypt解密 */
Vue.prototype.$decrypt = function (pri, cipher_text) {
  var decrypt = new JSEncrypt();
  decrypt.setPrivateKey(pri);
  return decrypt.decrypt(cipher_text);
}

new Vue({
  axios,
  vuetify,
  router,
  JSEncrypt,
  data() {
    return {
      navi_show: false,

      UserName: "1",
      UserAvatar: "",
      PublicKey: "",
    }

  },
  render: h => h(App)
}).$mount('#app')