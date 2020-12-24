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
import qs from "qs";

export default {
  name: "Setting",
  data() {
    return {
      wl_core_verison: null,
    };
  },
  mounted() {
    this.$axios({
      method: "post",
      url: this.glob.root_path + "/guest/Get_core_version",
      data: qs.stringify({
        Token: this.$encrypt(this.$root.PublicKey, new Date().toISOString()),
      }),
    }).then((response) => {
      this.wl_core_verison = response.data;
    });
  },
};
</script>