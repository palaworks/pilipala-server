<template>
  <v-app>
    <div class="d-flex justify-center flex-wrap mt-4">
      <v-card
        v-for="(value,key) in preview_list"
        :key="key"
        elevation="2"
        outlined
        class="ma-2 flex-grow-1"
        min-width="120px"
      >
        <v-card-title class="text-subtitle-1">{{key}}</v-card-title>
        <v-divider></v-divider>
        <v-card-actions>
          <v-btn text color="blue">{{value}}</v-btn>
        </v-card-actions>
      </v-card>
    </div>
  </v-app>
</template>


<script>
export default {
  name: "Home",
  data() {
    return {
      preview_list: {
        文章总计: null,
        备份数: null,
        隐藏: null,
        展示: null,
        归档: null,
        计划: null,
        评论: null,
      },
      temp: null,
    };
  },
  mounted() {
    this.$axios
      .post("https://localhost:44334/system/serv/user.asmx/Get_counts")
      .then((response) => {
        this.preview_list.文章总计 = response.data.PostCount;
        this.preview_list.备份数 = response.data.CopyCount;
        this.preview_list.隐藏 = response.data.HiddenCount;
        this.preview_list.展示 = response.data.OnDisplayCount;
        this.preview_list.归档 = response.data.ArchivedCount;
        this.preview_list.计划 = response.data.ScheduledCount;
        this.preview_list.评论 = response.data.CommentCount;
      })
      .catch(function (error) {
        // 请求失败处理
        console.log(error);
      });
  },
};
</script>