<template>
  <v-app>
    <v-card class="mx-auto mt-2" rounded>
      <v-card-title>
        <v-icon left class="mt-n1">mdi-desktop-tower</v-icon>试作型控制台(JILIGULU)
      </v-card-title>
      <v-card-subtitle>PILIPALA BETA8</v-card-subtitle>
      <v-card-text>WL内核信息:{{wl_core_verison}}</v-card-text>
    </v-card>
  </v-app>
</template>
<script>
export default {
  name: "Setting",
  data() {
    return {
      wl_core_verison: null,
    };
  },

  mounted() {
    /* 得到拷贝列表 */
    this.$axios
      .get("https://localhost:44334/system/serv/sysserv.asmx/Get_core_version")
      .then((response) => (this.wl_core_verison = response.data))
      .catch(function (error) {
        // 请求失败处理
        console.log(error);
      });
  },
};
</script>