<template>
  <v-app>
    <div class="mt-2 mb-11 mx-auto" style="width:98%">
      <v-expansion-panels popout>
        <v-expansion-panel v-for="item in copy_list" :key="item.GUID">
          <v-expansion-panel-header>
            <div
              class="text-subtitle-1"
              :class="item.Title==''?'text--disabled':'text--secondary'"
            >{{item.Title==''?'无标题':item.Title}}</div>
            <v-row justify="end" class="mr-4">
              <v-chip
                :outlined="!item.GUID==active_guid"
                :color="item.GUID==active_guid?'success':'transparent'"
              >{{item.LCT}}</v-chip>
            </v-row>
          </v-expansion-panel-header>
          <v-expansion-panel-content>
            <v-list-item-title class="text-h6">
              <v-btn
                :disabled="item.GUID==active_guid"
                class="mb-1"
                color="primary"
                small
                rounded
                @click="Apply(item.GUID)"
              >应用</v-btn>
              {{item.GUID}}
            </v-list-item-title>
            <v-chip label small class="mr-4">
              <v-icon small left>mdi-cube-outline</v-icon>
              {{item.PropertyContainer.MD5}}
            </v-chip>
            <v-chip
              v-if="item.Mode==''?false:true"
              text-color="white"
              :color="item.Mode=='archived'?'amber accent-4':item.Mode=='scheduled'?'blue accent-5':item.Mode=='hidden'?'grey':null"
            >{{item.Mode=='archived'?'已归档':item.Mode=='scheduled'?'计划':item.Mode=='hidden'?'隐藏':null}}</v-chip>
            <v-chip class="ml-1">
              <v-icon small left>mdi-folder-outline</v-icon>
              {{item.Archiv}}
            </v-chip>
            <span v-if="item.Label==''?false:true">
              <v-chip v-for="el in item.Label.split('$')" :key="el" class="ml-1">{{el}}</v-chip>
            </span>
            <v-list-item-subtitle
              class="text-subtitle-1 my-2"
              :class="item.Summary==''?'text--disabled':'text--secondary'"
            >{{item.Summary==''?'无概要':item.Summary}}</v-list-item-subtitle>
            <v-list-item-subtitle
              style="max-width: 78vw;"
              class="ml-3 text--disabled"
            >{{ item.Content }}</v-list-item-subtitle>
            <v-row justify="end">
              <v-btn
                :disabled="item.GUID==active_guid"
                color="error"
                small
                text
                @click="Delete(item.GUID)"
              >删除</v-btn>
            </v-row>
          </v-expansion-panel-content>
        </v-expansion-panel>
      </v-expansion-panels>
    </div>

    <div style="left:50%;transform:translateX(-50%);z-index:1;bottom:0;position:fixed;">
      <v-btn
        :disabled="copy_list.length==1?true:false"
        small
        class="ma-1 mb-2"
        color="primary"
        @click="Rollback()"
      >
        <v-icon left>mdi-restart-alert</v-icon>回滚到上一次更改(Rollback)
      </v-btn>
      <v-btn
        :disabled="copy_list.length==1?true:false"
        small
        class="ma-1 mb-2"
        color="error"
        @click="Release()"
      >
        <v-icon left>mdi-delete-alert</v-icon>释放冗余(Release)
      </v-btn>
    </div>
  </v-app>
</template>
<script>
import qs from "qs";

export default {
  name: "Iteration",
  data() {
    return {
      copy_list: [],
      active_guid: "",
    };
  },
  methods: {
    Rollback: function () {
      this.$axios({
        method: "post",
        url: this.glob.root_path + "/user/Rollback_post_by_PostID",
        data: qs.stringify({
          Token: this.$encrypt(this.$root.PublicKey, new Date().toISOString()),
          PostID: this.$route.params.post_id,
        }),
      })
        .then((response) => {
          this.GetActiveGUID();
          this.GetData();
          console.log(response);
        })
    },
    Release: function () {
      this.$axios({
        method: "post",
        url: this.glob.root_path + "/user/Release_post_by_PostID",
        data: qs.stringify({
          Token: this.$encrypt(this.$root.PublicKey, new Date().toISOString()),
          PostID: this.$route.params.post_id,
        }),
      })
        .then((response) => {
          this.GetActiveGUID();
          this.GetData();
          console.log(response);
        })
        .catch(function (error) {
          console.log(error); //请求失败处理
        });
    },
    Apply: function (GUID) {
      this.$axios({
        method: "post",
        url: this.glob.root_path + "/user/Apply_post_by_GUID",
        data: qs.stringify({
          Token: this.$encrypt(this.$root.PublicKey, new Date().toISOString()),
          GUID: GUID,
        }),
      })
        .then((response) => {
          this.GetActiveGUID();
          this.GetData();
          console.log(response);
        })
        .catch(function (error) {
          console.log(error); //请求失败处理
        });
    },
    Delete: function (GUID) {
      this.$axios({
        method: "post",
        url: this.glob.root_path + "/user/Delete_post_by_GUID",
        data: qs.stringify({
          Token: this.$encrypt(this.$root.PublicKey, new Date().toISOString()),
          GUID: GUID,
        }),
      })
        .then((response) => {
          this.GetActiveGUID();
          this.GetData();
          console.log(response);
        })
        .catch(function (error) {
          console.log(error); //请求失败处理
        });
    },
    GetActiveGUID: function () {
      /* 得到拷贝列表 */
      this.$axios({
        method: "post",
        url: this.glob.root_path + "/user/Get_post_by_PostID",
        data: qs.stringify({
          Token: this.$encrypt(this.$root.PublicKey, new Date().toISOString()),
          PostID: this.$route.params.post_id,
        }),
      })
        .then((response) => (this.active_guid = response.data.GUID))
        .catch(function (error) {
          console.log(error); //请求失败处理
        });
    },
    GetData: function () {
      /* 得到拷贝列表 */
      this.$axios({
        method: "post",
        url: this.glob.root_path + "/user/Get_neg_posts_by_PostID",
        data: qs.stringify({
          Token: this.$encrypt(this.$root.PublicKey, new Date().toISOString()),
          PostID: this.$route.params.post_id,
        }),
      })
        .then((response) => (this.copy_list = response.data))
        .catch(function (error) {
          console.log(error); //请求失败处理
        });
    },
  },
  mounted() {
    this.GetActiveGUID();
    this.GetData();
  },
};
</script>