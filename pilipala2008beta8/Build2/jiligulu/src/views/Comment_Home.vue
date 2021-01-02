<template>
  <v-app>
    <v-chip
      v-if="this.commented_post_list.length==0"
      style="margin:auto;letter-spacing:1px"
      color="accent"
    >
      <v-icon left>mdi-sleep</v-icon>现在还没有评论呢......
    </v-chip>
    <div class="d-flex justify-center flex-wrap">
      <v-card
        v-for="item in commented_post_list"
        :key="item.CommentID"
        class="mx-2 mt-4 flex-grow-1"
      >
        <v-card-title>
          <p v-if="item.Title" class="text-h6">{{ item.Title }}</p>
          <p v-if="!item.Title" class="text-subtitle-1">
            <v-icon left style="margin-top:-3px">mdi-label</v-icon>
            {{ item.Content }}
          </p>
        </v-card-title>
        <v-card-subtitle>
          <v-row>
            <v-chip
              label
              color="transparent"
              class="text--disabled"
            >最后评论时间 : {{item.LatestCommentTime}}</v-chip>
          </v-row>
          <v-chip label color="transparent" class="text--disabled">最近一月 : {{item.MonthCommentCount}}</v-chip>
          <v-chip label color="transparent" class="text--disabled">最近一周 : {{item.WeekCommentCount}}</v-chip>
        </v-card-subtitle>

        <v-card-actions class="mt-n4">
          <v-btn
            text
            color="primary"
            class="mx-2"
            :to="{ name: 'Comment_List', params: { ID:item.ID}}"
          >查看</v-btn>
          <v-chip outlined label small class="text-subtitle-2 text--secondary">{{item.CommentCount}}</v-chip>
          <v-row justify="end">
            <v-btn text class="text--disabled mr-4">
              <v-icon left style="margin-top:1px">mdi-code-tags</v-icon>
              {{item.ID}}
            </v-btn>
          </v-row>
        </v-card-actions>
      </v-card>
    </div>
  </v-app>
</template>
<script>
import qs from "qs";

export default {
  name: "Comment_Home",
  data() {
    return {
      commented_post_list: {},
    };
  },
  methods: {
    Get_commented_posts: function () {
      this.$axios({
        method: "post",
        url: this.glob.root_path + "/user/Get_commented_posts",
        data: qs.stringify({
          Token: this.$encrypt(this.$root.PublicKey, new Date().toISOString()),
        }),
      }).then((response) => {
        this.commented_post_list = response.data;
        console.log(this.commented_post_list);
      });
    },
  },
  mounted() {
    this.Get_commented_posts();
  },
};
</script>