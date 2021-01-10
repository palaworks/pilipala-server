<template>
  <v-app>
    <v-card class="d-flex flex-row">
      <Navi v-if="this.$root.navi_show" />
      <v-card :class="this.$root.navi_show?'ml-14':''" width="100%">
    
          <router-view class="px-2" />
        
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
