<template>
  <v-app>
    <v-card rounded class="mx-2 mt-4">
      <v-list>
        <v-list-item>
          <v-icon left>mdi-desktop-tower</v-icon>
          <v-list-item-content>
            <v-list-item-title>试作型控制台</v-list-item-title>
            <v-list-item-subtitle>jiligulu</v-list-item-subtitle>
          </v-list-item-content>
        </v-list-item>
        <v-divider class="mb-2"></v-divider>
        <v-list-item>
          <v-list-item-content>
            <v-list-item-title>噼里啪啦版本</v-list-item-title>
            <v-list-item-subtitle>BETA8</v-list-item-subtitle>
          </v-list-item-content>
        </v-list-item>
        <v-list-item>
          <v-list-item-content>
            <v-list-item-title>内核版本</v-list-item-title>
            <v-list-item-subtitle>{{wl_core_verison}}</v-list-item-subtitle>
          </v-list-item-content>
        </v-list-item>
      </v-list>
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
      url: this.glob.root_path + "/user/Get_core_version",
      data: qs.stringify({
        Token: this.$encrypt(this.$root.PublicKey, new Date().toISOString()),
      }),
    }).then((response) => {
      this.wl_core_verison = response.data;
    });
  },
};
</script>