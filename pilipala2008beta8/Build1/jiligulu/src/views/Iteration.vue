<template>
  <v-app>
    <div class="d-flex justify-center mt-2">
      <v-btn class="mx-2" rounded small color="primary" @click="Rollback()">
        <v-icon left>mdi-restart-alert</v-icon>回滚到上一次更改(Rollback)
      </v-btn>
      <v-btn class="mx-2" rounded small color="error" @click="Release()">
        <v-icon left>mdi-delete-alert</v-icon>释放冗余(Release)
      </v-btn>
    </div>
    <v-card class="mt-2 mx-auto" rounded width="90%">
      <v-expansion-panels accordion>
        <v-expansion-panel v-for="item in copy_list" :key="item.GUID">
          <v-expansion-panel-header>
            <div class="text-subtitle-1">{{item.Title}}</div>
            <v-card flat class="text-right text--secondary">
              <v-chip
                v-if="item.GUID==active_guid"
                class="ma-2"
                color="green"
                text-color="white"
                close-icon="mdi-delete"
              >
                <v-avatar left>
                  <v-icon small>mdi-source-branch-check</v-icon>
                </v-avatar>当前
              </v-chip>
              {{item.LCT}}
            </v-card>
          </v-expansion-panel-header>
          <v-expansion-panel-content>
            <v-list-item-title class="text-h6">
              {{item.GUID}}
              <v-btn
                :disabled="item.GUID==active_guid"
                class="ml-2 mb-1"
                color="primary"
                small
                rounded
                @click="Apply(item.GUID)"
              >应用</v-btn>

              <v-btn
                :disabled="item.GUID==active_guid"
                class="ml-2 mb-1"
                color="error"
                small
                rounded
                @click="Delete(item.GUID)"
              >删除</v-btn>
            </v-list-item-title>
            <v-chip class="ma-1">
              <v-icon left>mdi-cube-outline</v-icon>
              {{item.MD5}}
            </v-chip>
            <v-chip class="mx-1">
              <v-icon left>mdi-slash-forward-box</v-icon>
              {{item.Mode}}
            </v-chip>
            <v-chip class="mx-1">
              <v-icon left>mdi-folder-outline</v-icon>
              {{item.Archiv}}
            </v-chip>
            <v-chip v-for="el in item.Label.split('$')" :key="el" class="mx-1">{{el}}</v-chip>
            <v-list-item-subtitle class="text-subtitle-1 text--secondary my-2">{{ item.Summary }}</v-list-item-subtitle>
            <v-list-item-subtitle
              style="max-width: 78vw;"
              class="ml-3 text--disabled"
            >{{ item.Content }}</v-list-item-subtitle>
          </v-expansion-panel-content>
        </v-expansion-panel>
      </v-expansion-panels>
    </v-card>
  </v-app>
</template>
<script>
import qs from "qs";

export default {
  name: "Iteration",
  data() {
    return {
      copy_list: null,
      active_guid: null,
    };
  },
  methods: {
    Rollback: function () {
      this.$axios({
        method: "post",
        url: "https://localhost:44334/system/serv/SysServ.asmx/Rollback",
        data: qs.stringify({
          ID: this.$route.params.post_id,
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
    Release: function () {
      this.$axios({
        method: "post",
        url: "https://localhost:44334/system/serv/SysServ.asmx/Release",
        data: qs.stringify({
          ID: this.$route.params.post_id,
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
        url: "https://localhost:44334/system/serv/SysServ.asmx/Apply",
        data: qs.stringify({
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
        url: "https://localhost:44334/system/serv/SysServ.asmx/Delete",
        data: qs.stringify({
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
        url: "https://localhost:44334/system/serv/SysServ.asmx/GetPostData",
        data: qs.stringify({
          ID: this.$route.params.post_id,
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
        url: "https://localhost:44334/system/serv/SysServ.asmx/GetCopys",
        data: qs.stringify({
          ID: this.$route.params.post_id,
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