<template>
  <v-app>
    <div class="mb-11 mx-auto" style="width:98%">
      <v-card class="mt-2" v-for="comment in comment_list" :key="comment.CommentID">
        <v-list-item>
          <v-list-item-content>
            <v-list-item-title>
              <v-chip outlined label small class="text-subtitle-2">{{comment.Floor}}F</v-chip>
              <v-chip label color="transparent" class="text-h6 mx-1">{{comment.User}}</v-chip>
              <v-chip v-if="comment.HEAD" class="text-button" color="orange" text-color="white">
                <v-icon left>mdi-arrow-right</v-icon>
                {{getFloor(comment.HEAD)}}F
              </v-chip>
              <v-chip label color="transparent" class="text-h6 ml-1">{{getUser(comment.HEAD)}}</v-chip>
            </v-list-item-title>
          </v-list-item-content>
          <v-list-item-action>
            <v-btn text class="text--disabled">
              <v-icon left>mdi-code-tags</v-icon>
              {{comment.CommentID}}
            </v-btn>
          </v-list-item-action>
        </v-list-item>

        <v-card-text class="my-n5">{{comment.Content}}</v-card-text>
        <v-card-actions>
          <v-chip
            label
            color="transparent"
            class="text--disabled"
            v-if="comment.Time"
          >{{comment.Time}}</v-chip>
          <v-chip
            label
            color="transparent"
            class="text--disabled"
            v-if="comment.WebSite"
          >{{comment.WebSite}}</v-chip>
          <v-chip
            label
            color="transparent"
            class="text--disabled"
            v-if="comment.Email"
          >{{comment.Email}}</v-chip>

          <v-row justify="end">
            <v-btn
              small
              class="text-caption mr-4"
              color="error"
              text
              @click="Delete_comment_by_CommentID(comment.CommentID)"
            >删除</v-btn>
          </v-row>
        </v-card-actions>
      </v-card>
    </div>
  </v-app>
</template>
<script>
import qs from "qs";

export default {
  name: "Comment_List",
  data() {
    return {
      comment_list: [],
    };
  },
  methods: {
    getFloor: function (HEAD) {
      var Floor;
      this.comment_list.forEach((element) => {
        if (element.CommentID == HEAD) {
          Floor = element.Floor;
        }
      });
      return Floor;
    },
    getUser: function (HEAD) {
      var User;
      this.comment_list.forEach((element) => {
        if (element.CommentID == HEAD) {
          User = element.User;
        }
      });
      return User;
    },
    Get_comments_by_PostID: function () {
      this.$axios({
        method: "post",
        url: this.glob.root_path + "/user/Get_comments_by_PostID",
        data: qs.stringify({
           Token: this.$encrypt(this.$root.PublicKey, new Date().toISOString()),
          PostID: this.$route.params.ID,
        }),
      })
        .then((response) => {
          this.comment_list = response.data;
          console.log(response);
        })
        .catch(function (error) {
          console.log(error); //请求失败处理
        });
    },
    Delete_comment_by_CommentID: function (CommentID) {
      this.$axios({
        method: "post",
        url: this.glob.root_path + "/user/Delete_comment_by_CommentID",
        data: qs.stringify({
          Token: this.$encrypt(this.$root.PublicKey, new Date().toISOString()),
          CommentID: CommentID,
        }),
      })
        .then((response) => {
          this.Get_comments_by_PostID();
          console.log(response);
        })
        .catch(function (error) {
          console.log(error); //请求失败处理
        });
    },
  },
  mounted() {
    this.Get_comments_by_PostID();
  },
};
</script>