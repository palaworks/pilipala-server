<template>
  <v-app>
    <v-navigation-drawer fixed expand-on-hover v-model="drawer" width="280" floating permanent>
      <v-list-item class="px-2">
        <v-list-item-avatar size="40">
          <v-img :src="this.$root.UserAvatar"></v-img>
        </v-list-item-avatar>
        <v-list-item-content>{{this.$root.UserName}}</v-list-item-content>
        <v-list-item-action>
          <v-switch
            append-icon="mdi-moon-waning-crescent"
            inset
            dense
            v-model="$vuetify.theme.dark"
          ></v-switch>
        </v-list-item-action>
      </v-list-item>
      <v-divider></v-divider>

      <v-list>
        <v-list-item
          link
          v-for="item in items"
          :key="item.title"
          :disabled="item.is_disabled"
          :to="item.src"
        >
          <v-list-item-icon>
            <v-icon>{{ item.icon }}</v-icon>
          </v-list-item-icon>
          <v-list-item-content>
            <v-list-item-title>{{ item.title }}</v-list-item-title>
          </v-list-item-content>
        </v-list-item>
      </v-list>
    </v-navigation-drawer>
  </v-app>
</template>

<script>
export default {
  name: "Navi",
  data() {
    return {};
  },
};
</script>
<script>
export default {
  data() {
    return {
      drawer: true,
      items: require("../static/navi_menu.json"),
    };
  },
};
</script>
<style>
.router-link-active {
  text-decoration: none;
}
a {
  text-decoration: none;
}
</style>