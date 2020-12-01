<template>
  <v-app>
    <div>
      <v-card class="d-flex flex-row flex-wrap ma-2" flat>
        <v-card
          v-for="(k,v) in preview_list"
          :key="k"
          elevation="2"
          outlined
          width="190"
          class="ma-2"
        >
          <v-card-title class="text-subtitle-1">{{v}}</v-card-title>
          <v-divider></v-divider>
          <v-card-actions>
            <v-btn text color="blue">{{k}}</v-btn>
          </v-card-actions>
        </v-card>
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
      .post("https://localhost:44334/system/serv/user.asmx/Get_Count_DataList")
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