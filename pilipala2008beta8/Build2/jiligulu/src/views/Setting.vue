<template>
  <v-app>
    <v-card rounded class="mx-auto mt-4" style="min-width:320px">
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
            <v-list-item-subtitle>{{pilipala_version}}</v-list-item-subtitle>
          </v-list-item-content>
        </v-list-item>
        <v-list-item>
          <v-list-item-content>
            <v-list-item-title>内核版本</v-list-item-title>
            <v-list-item-subtitle>{{core_version}}</v-list-item-subtitle>
          </v-list-item-content>
        </v-list-item>
        <v-list-item>
          <v-list-item-content>
            <v-list-item-title>加密数据链路验证</v-list-item-title>
            <v-list-item-subtitle>{{auth}}</v-list-item-subtitle>
          </v-list-item-content>
        </v-list-item>
        <v-list-item>
          <v-list-item-content>
            <v-list-item-title>下次验证时间</v-list-item-title>
            <v-list-item-subtitle>{{auth_end_time}}</v-list-item-subtitle>
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
      pilipala_version: null,
      core_version: null,
      auth: null,
      auth_end_time: null,
    };
  },
  mounted() {
    this.$axios({
      method: "post",
      url: this.glob.root_path + "/user/Get_system_info",
      data: qs.stringify({
        Token: this.$encrypt(this.$root.PublicKey, new Date().toISOString()),
      }),
    }).then((response) => {
      this.pilipala_version = response.data.pilipala_version;
      this.core_version = response.data.core_version;
      this.auth = response.data.auth;
      this.auth_end_time = response.data.auth_end_time;
    });
  },
};
</script>