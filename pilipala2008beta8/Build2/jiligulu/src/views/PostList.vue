<template>
  <v-app class="pb-16">
    <v-btn
      to="/Edit"
      color="primary"
      fixed
      bottom
      elevation="4"
      style="left:50%;transform:translateX(-50%);z-index:1"
    >
      <v-icon>mdi-plus</v-icon>
    </v-btn>
    <v-lazy
      min-height="100"
      transition="fade-transition"
      class="mx-2 mt-2"
      v-for="item in post_list"
      :key="item.ID"
    >
      <v-card>
        <v-list-item>
          <v-list-item-content>
            <p
              style="overflow:hidden;text-overflow:ellipsis;white-space:nowrap;"
              v-if="item.Type=='note'?false:true"
              class="text-h6"
            >{{ item.Title }}</p>
            <p v-if="item.Type=='note'?false:true" class="text-subtitle-2">{{ item.Summary }}</p>
            <p
              style="max-height:56px;max-width:60vw;display:-webkit-box;overflow:hidden;text-overflow:ellipsis;word-wrap:break-word;white-space:normal;-webkit-line-clamp:2;-webkit-box-orient:vertical;"
              :class="ContentClass(item.Type)"
            >
              <v-icon v-if="item.Type=='note'?true:false" left class="mt-n1">mdi-label</v-icon>
              {{ item.Content }}
            </p>
          </v-list-item-content>
          <v-list-item-action>
            <div>
              <v-chip
                v-if="item.Mode==''?false:true"
                text-color="white"
                small
                :color="item.Mode=='archived'?'amber accent-4':item.Mode=='scheduled'?'blue accent-5':item.Mode=='hidden'?'grey':null"
              >{{item.Mode=='archived'?'已归档':item.Mode=='scheduled'?'计划':item.Mode=='hidden'?'隐藏':null}}</v-chip>
              <v-btn disabled text class="text--disabled">
                <v-icon left>mdi-folder-outline</v-icon>
                {{item.Archiv}}
              </v-btn>
              <v-btn text class="text--disabled">{{item.ID}}</v-btn>
            </div>
            <v-btn small text class="text--disabled">
              <v-icon small>mdi-account-edit</v-icon>
              {{item.User}}
              <v-icon small class="ml-2">mdi-format-quote-open</v-icon>
              {{item.PropertyContainer.CommentCount}}
              <v-icon small class="ml-2">mdi-star-outline</v-icon>
              {{item.StarCount}}
              <v-icon small class="ml-2">mdi-account-multiple-outline</v-icon>
              {{item.UVCount}}
            </v-btn>
            <v-btn disabled small text>最后编辑 : {{item.LCT}}</v-btn>
          </v-list-item-action>
        </v-list-item>
        <v-card-actions class="mt-n7">
          <v-btn
            :to="{ name: 'Edit', params: { post_id:item.ID}}"
            class="text-caption"
            color="blue"
            text
          >编辑</v-btn>

          <v-btn :to="{ name: 'Iteration', params: { post_id:item.ID}}" class="text-caption" text>
            <v-icon color="secondary" left>mdi-source-merge</v-icon>迭代化
          </v-btn>

          <v-tooltip right>
            <template v-slot:activator="{ on, attrs }">
              <v-btn text class="text-caption" v-bind="attrs" v-on="on">详情</v-btn>
            </template>
            创建时间 : {{item.CT}}
            <br />
            MD5 : {{item.PropertyContainer.MD5}}
          </v-tooltip>

          <v-row justify="end">
            <v-btn small class="text-caption mr-4" color="error" text @click="Dispose(item.ID)">删除</v-btn>
          </v-row>
        </v-card-actions>
      </v-card>
    </v-lazy>
  </v-app>
</template>

<script>
import qs from "qs";

export default {
  name: "PostList",
  data() {
    return {
      drawer: true,
      mini: true,
      post_list: null,
      post_id: -1,
    };
  },
  methods: {
    ContentClass: function (Type) {
      if (Type == "note") {
        return "text--primary text-subtitle-1";
      } else {
        return "text--disabled text-caption d-inline-block";
      }
    },
    Dispose: function (ID) {
      this.$axios({
        method: "post",
        url: this.glob.root_path + "/user/Dispose_post_by_PostID",
        data: qs.stringify({
          Token: this.$encrypt(this.$root.PublicKey, new Date().toISOString()),
          PostID: ID,
        }),
      }).then(() => {
        this.GetData();
      });
    },
    GetData: function () {
      this.$axios({
        method: "post",
        url: this.glob.root_path + "/user/Get_posts",
        data: qs.stringify({
          Token: this.$encrypt(this.$root.PublicKey, new Date().toISOString()),
        }),
      }).then((response) => {
        this.post_list = response.data;
      });
    },
  },
  mounted() {
    this.GetData();
  },
};
</script>