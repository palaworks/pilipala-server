<template>
  <v-app>
    <v-card class="d-flex flex-row" color="grey lighten-2" flat tile>
      <Navi />
      <v-card class="ml-14" width="100%" flat tile>
        <router-view />
      </v-card>
    </v-card>
  </v-app>
</template>

<script>
import Navi from "./components/Navi";

export default {
  name: "App",

  components: {
    Navi,
  },

  data() {
    return {};
  },
};
</script>
