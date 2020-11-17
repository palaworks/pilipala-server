<template>
  <v-app>
    <v-btn to="/Edit" small fab class="mt-2 mx-auto" color="primary">
      <v-icon>mdi-plus</v-icon>
    </v-btn>
    <v-card v-for="item in post_list" :key="item.ID" class="mx-auto mt-2" width="92%">
      <v-list-item>
        <v-list-item-content>
          <p v-if="item.Type=='note'?false:true" class="text-h6">{{ item.Title }}</p>
          <p v-if="item.Type=='note'?false:true" class="text-subtitle-2">{{ item.Summary }}</p>
          <p style="max-width:62vw;max-height:15px" :class="ContentClass(item.Type)">
            <v-icon v-if="item.Type=='note'?true:false" left class="mt-n1">mdi-label</v-icon>
            {{ item.Content }}
          </p>
        </v-list-item-content>
        <v-list-item-action>
          <div>
            <v-btn text class="text--disabled">
              <v-icon left>mdi-folder-outline</v-icon>
              {{item.Archiv}}
            </v-btn>
            <v-btn text class="text--disabled">
              <v-icon left>mdi-code-tags</v-icon>
              {{item.ID}}
            </v-btn>
          </div>
          <v-btn small text class="text--disabled">最后编辑 : {{item.LCT}}</v-btn>
        </v-list-item-action>
      </v-list-item>
      <v-card-actions class="mt-n3 md-n2">
        <v-btn
          :to="{ name: 'Edit', params: { post_id:item.ID}}"
          class="text-caption"
          color="blue"
          text
        >编辑</v-btn>

        <v-btn
          :to="{ name: 'Iteration', params: { post_id:item.ID}}"
          class="text-caption"
          text
        >
          <v-icon color="secondary" left>mdi-source-merge</v-icon>迭代化
        </v-btn>

        <v-tooltip right>
          <template v-slot:activator="{ on, attrs }">
            <v-btn text class="text-caption" v-bind="attrs" v-on="on">详情</v-btn>
          </template>
          创建时间 : {{item.CT}}
          <br />
          MD5 : {{item.MD5}}
        </v-tooltip>

        <v-row class="mr-2" justify="end">
          <v-btn class="text-caption" color="error" text @click="Dispose(item.ID)">删除</v-btn>
        </v-row>
      </v-card-actions>
    </v-card>
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
        return "text--primary text-h6";
      } else {
        return "text--disabled text-caption d-inline-block";
      }
    },
    Dispose: function (ID) {
      this.$axios({
        method: "post",
        url: "https://localhost:44334/system/serv/SysServ.asmx/Dispose",
        data: qs.stringify({
          ID: ID,
        }),
      })
        .then((response) => {
          this.GetData();
          console.log(response);
        })
        .catch(function (error) {
          console.log(error); //请求失败处理
        });
    },
    GetData: function () {
      this.$axios
        .post("https://localhost:44334/system/serv/SysServ.asmx/GetPosts")
        .then((response) => (this.post_list = response.data))
        .catch(function (error) {
          // 请求失败处理
          console.log(error);
        });
    },
  },
  mounted() {
    this.GetData();
  },
};
</script>