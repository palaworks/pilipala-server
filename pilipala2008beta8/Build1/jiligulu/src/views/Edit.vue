<template>
  <v-container width="90%">
    <v-row>
      <v-col cols="6">
        <v-text-field
          :disabled="new_data.Type=='note'"
          v-model="new_data.Title"
          label="文章标题"
          hint="Title"
        ></v-text-field>
      </v-col>
      <v-col cols="2">
        <v-select v-model="new_data.Type" :items="type_select" label="类型" hint="Type"></v-select>
      </v-col>
      <v-col cols="2">
        <v-select v-model="new_data.Mode" :items="mode_select" label="模式" hint="Mode"></v-select>
      </v-col>
      <v-col cols="2">
        <v-text-field v-model="new_data.StarCount" type="number" label="星星计数" hint="StarCount"></v-text-field>
      </v-col>
      <v-col cols="10">
        <v-text-field
          :disabled="new_data.Type=='note'"
          v-model="new_data.Summary"
          label="文章概要"
          hint="Summary"
        ></v-text-field>
      </v-col>
      <v-col cols="2">
        <v-text-field v-model="new_data.UVCount" type="number" label="浏览计数" hint="UVCount"></v-text-field>
      </v-col>
      <v-col cols="12">
        <v-textarea v-model="new_data.Content" label="文章内容" hint="Content" counter></v-textarea>
      </v-col>
      <v-col cols="2">
        <v-text-field v-model="new_data.Archiv" label="归档" hint="Archiv"></v-text-field>
      </v-col>
      <v-col cols="6">
        <v-text-field v-model="new_data.Label" label="标签集合" hint="Label"></v-text-field>
      </v-col>
      <v-col class="justify-end d-flex">
        <v-btn color="success" class="mt-2" @click="Push()">
          <v-icon left small>mdi-subdirectory-arrow-right</v-icon>
          {{new_data.ID == null ? '新建' : '编辑到 '+new_data.ID}}
        </v-btn>
      </v-col>
    </v-row>
  </v-container>
</template>

<script>
import qs from "qs";

export default {
  name: "Edit",
  data() {
    return {
      new_data: {
        ID: this.$route.params.post_id,
        Mode: "",
        Type: "",
        User: "",

        UVCount: "",
        StarCount: "",

        Title: "",
        Summary: "",
        Content: "",

        Archiv: "",
        Label: "",
        Cover: "",
      },
      type_select: ["p", "note"],
      mode_select: ["o", "x", "archived", "sche"],
    };
  },
  methods: {
    Push: function () {
      if (this.new_data.ID == null) {
        this.Reg();
        this.$router.push({ name: "PostList" });
      } else {
        this.Update();
        this.$router.push({ name: "PostList" });
      }
    },
    Reg: function () {
      this.$axios({
        method: "post",
        url: "https://localhost:44334/system/serv/SysServ.asmx/Reg",
        data: qs.stringify({
          Mode: this.new_data.Mode,
          Type: this.new_data.Type,
          User: this.new_data.User,

          UVCount: this.new_data.UVCount,
          StarCount: this.new_data.StarCount,

          Title: this.new_data.Title,
          Summary: this.new_data.Summary,
          Content: this.new_data.Content,

          Archiv: this.new_data.Archiv,
          Label: this.new_data.Label,
          Cover: this.new_data.Cover,
        }),
      })
        .then((response) => console.log(response))
        .catch(function (error) {
          console.log(error); //请求失败处理
        });
    },
    Update: function () {
      this.$axios({
        method: "post",
        url: "https://localhost:44334/system/serv/SysServ.asmx/Update",
        data: qs.stringify({
          ID: this.new_data.ID,
          Mode: this.new_data.Mode,
          Type: this.new_data.Type,

          UVCount: this.new_data.UVCount,
          StarCount: this.new_data.StarCount,

          Title: this.new_data.Title,
          Summary: this.new_data.Summary,
          Content: this.new_data.Content,

          Archiv: this.new_data.Archiv,
          Label: this.new_data.Label,
          Cover: this.new_data.Cover,
        }),
      })
        .then((response) => console.log(response))
        .catch(function (error) {
          console.log(error); //请求失败处理
        });
    },
  },
  mounted() {
    if (this.$route.params.post_id != null) {
      this.$axios({
        method: "post",
        url: "https://localhost:44334/system/serv/SysServ.asmx/GetPostData",
        data: qs.stringify({
          ID: this.$route.params.post_id,
        }),
      })
        .then((response) => {
          this.new_data.ID = response.data.ID;
          this.new_data.Mode = response.data.Mode;
          this.new_data.Type = response.data.Type;

          this.new_data.UVCount = response.data.UVCount;
          this.new_data.StarCount = response.data.StarCount;

          this.new_data.Title = response.data.Title;
          this.new_data.Summary = response.data.Summary;
          this.new_data.Content = response.data.Content;

          this.new_data.Archiv = response.data.Archiv;
          this.new_data.Label = response.data.Label;
          this.new_data.Cover = response.data.Cover;
        })
        .catch(function (error) {
          console.log(error); //请求失败处理
        });
    }
  },
};
</script>