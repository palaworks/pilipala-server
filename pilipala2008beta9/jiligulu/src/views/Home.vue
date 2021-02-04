<template>
  <v-app>
    <div class="d-flex justify-center flex-wrap">
      <v-card
        v-for="(k,v) in this.preview_list"
        :key="v.name"
        elevation="2"
        outlined
        class="mx-2 mt-4 flex-grow-1"
      >
        <v-list-item>
          <v-list-item-content>
            <v-card-title class="text-subtitle-1">{{k.name}}</v-card-title>
            <v-card-subtitle>{{k.summary}}</v-card-subtitle>
          </v-list-item-content>
          <v-list-item-action>
            <v-btn x-large style="font-size:32px" rounded text color="blue">{{k.count}}</v-btn>
          </v-list-item-action>
        </v-list-item>
      </v-card>
    </div>
  </v-app>
</template>


<script>
import qs from "qs";

export default {
  name: "Home",
  data() {
    return {
      preview_list: {
        PostCount: {
          name: "文章",
          summary: "你在噼里啪啦的所有资产",
          count: null,
        },
        CopyCount: {
          name: "备份",
          summary: "文章迭代栈内的备份数量",
          count: null,
        },
        HiddenCount: { name: "隐藏", summary: "不会被展示的文章", count: null },
        OnDisplayCount: {
          name: "展示",
          summary: "大家可以浏览的文章",
          count: null,
        },
        ArchivedCount: {
          name: "归档",
          summary: "被纳入归档状态的文章",
          count: null,
        },
        ScheduledCount: {
          name: "计划",
          summary: "被纳入计划状态的文章",
          count: null,
        },
        CommentCount: { name: "评论", summary: "所有的留言数", count: null },
      },
      temp: null,
    };
  },
  mounted() {
    this.$axios({
      method: "post",
      url: this.glob.root_path + "/user/Get_counts",
      data: qs.stringify({
        Token: this.$encrypt(this.$root.PublicKey, new Date().toISOString()),
      }),
    }).then((response) => {
      this.preview_list["PostCount"].count = response.data.PostCount;
      this.preview_list.CopyCount.count = response.data.CopyCount;
      this.preview_list.HiddenCount.count = response.data.HiddenCount;
      this.preview_list.OnDisplayCount.count = response.data.OnDisplayCount;
      this.preview_list.ArchivedCount.count = response.data.ArchivedCount;
      this.preview_list.ScheduledCount.count = response.data.ScheduledCount;
      this.preview_list.CommentCount.count = response.data.CommentCount;
      console.log(this.preview_list);
    });
  },
};
</script>