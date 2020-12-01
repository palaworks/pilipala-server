<template>
  <v-app>
    <div class="mb-11 mx-auto" style="width:98%">
      <v-list>
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
              <v-btn small class="text-caption mr-4" color="error" text @click="Dispose(item.ID)">删除</v-btn>
            </v-row>
          </v-card-actions>
        </v-card>
      </v-list>
    </div>
  </v-app>
</template>
<script>
//import qs from "qs";

export default {
  name: "Comment",
  data() {
    return {
      comment_list: [
        {
          CommentID: 30001,
          PostID: 12384,
          User: "Thaumy",
          Email: "eml@eml.eml",
          Content: "我的评论",
          WebSite: "http://www.thaumy.cn/",
          HEAD: "",
          Time: "2017-08-05 13:30:00",
          Floor: "2",
        },
        {
          CommentID: 30002,
          PostID: 12384,
          User: "Pinn",
          Email: "eml@eml.eml",
          Content:
            "小品的评论小品的评论小品的评论小品的评论小品的评论小品的评论小品的评论小品的评论小品的评论小品的评论小品的评论小品的评论小品的评论小品的评论小品的评论小品的评论小品的评论小品的评论小品的评论小品的评论小品的评论小品的评论小品的评论小品的评论",
          WebSite: "http://www.thaumy.cn/",
          HEAD: "",
          Time: "2017-08-05 13:30:00",
          Floor: "3",
        },
        {
          CommentID: 30003,
          PostID: 12384,
          User: "JSS",
          Email: "eml@eml.eml",
          Content: "我看你是欠日",
          WebSite: "http://www.thaumy.cn/",
          HEAD: "30001",
          Time: "2017-08-05 13:30:00",
          Floor: "4",
        },
      ],
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
  },
  mounted() {},
};
</script>