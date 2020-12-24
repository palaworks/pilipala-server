<template>
  <v-app>
    <v-card style="margin:auto;width:50%;" :loading="this.login_data.is_loading">
      <v-card-title>
        <div style="font-size:24px;margin-top:10px" class="mx-auto">欢迎来到叽里咕噜</div>
      </v-card-title>
      <v-card-text>
        <v-text-field v-model="login_data.UserAccount" label="用户名" hint="UserName" auto-grow></v-text-field>
        <v-text-field
          v-model="login_data.UserPWD"
          label="密码"
          hint="UserPWD"
          type="password"
          auto-grow
        ></v-text-field>
      </v-card-text>
      <v-card-actions>
        <v-btn
          class="mx-auto"
          style="margin-bottom:10px;"
          width="300px"
          color="primary"
          @click="Login()"
        >Login</v-btn>
      </v-card-actions>
    </v-card>
  </v-app>
</template>
<script>
import qs from "qs";

export default {
  name: "Login",
  data() {
    return {
      login_data: {
        UserAccount: "",
        UserPWD: "",
        is_loading: false,
      },
    };
  },
  methods: {
    Login: function () {
      this.is_loading = true;
      this.$axios({
        method: "post",
        url: this.glob.root_path + "/user/Login",
        data: qs.stringify({
          UserAccount: this.login_data.UserAccount,
          UserPWD: this.login_data.UserPWD,
        }),
      }).then((response) => {
        if (response.data != null) {
          this.is_loading = false;
          this.$root.navi_show = true;
          this.$root.UserAccount = this.login_data.UserAccount;
          this.$root.PublicKey = response.data;

          this.$router.push({ name: "Home" });
        }
      });
    },
  },
};
</script>