<template>
  <v-app>
    <v-card style="width:30%;max-width:280px;min-width:240px" class="mt-4 mx-auto">
      <v-img :src="this.Avatar"></v-img>
      <v-card-title>{{this.Name}}</v-card-title>

      <v-divider></v-divider>

      <v-list>
        <v-list-item>
          <v-icon left small>mdi-card-account-details-outline</v-icon>
          <v-list-item-content>
            <v-list-item-title>账号</v-list-item-title>
            <v-list-item-subtitle>{{this.Account}}</v-list-item-subtitle>
          </v-list-item-content>
        </v-list-item>
        <v-list-item>
          <v-icon left small>mdi-text-account</v-icon>
          <v-list-item-content>
            <v-list-item-title>个人简介</v-list-item-title>
            <v-list-item-subtitle>{{this.Bio}}</v-list-item-subtitle>
          </v-list-item-content>
        </v-list-item>
        <v-list-item>
          <v-icon left small>mdi-account-supervisor-circle</v-icon>
          <v-list-item-content>
            <v-list-item-title>组别</v-list-item-title>
            <v-list-item-subtitle>{{this.GroupType}}</v-list-item-subtitle>
          </v-list-item-content>
        </v-list-item>
        <v-list-item>
          <v-icon left small>mdi-email-outline</v-icon>
          <v-list-item-content>
            <v-list-item-title>邮箱</v-list-item-title>
            <v-list-item-subtitle>{{this.Email}}</v-list-item-subtitle>
          </v-list-item-content>
        </v-list-item>
      </v-list>
    </v-card>
  </v-app>
</template>
<script>
import qs from "qs";

export default {
  name: "User",
  data() {
    return {
      Account: "",
      Name: "",
      Bio: "",
      GroupType: "",
      Email: "",
      Avatar: "",
    };
  },
  mounted() {
    this.$axios({
      method: "post",
      url: this.glob.root_path + "/user/Get_user_data",
      data: qs.stringify({
        Token: this.$encrypt(this.$root.PublicKey, new Date().toISOString()),
      }),
    }).then((response) => {
      this.Account = response.data.Account;
      this.Name = response.data.Name;
      this.Bio = response.data.Bio;
      this.GroupType = response.data.GroupType;
      this.Email = response.data.Email;
      this.Avatar = response.data.Avatar;
    });
  },
};
</script>