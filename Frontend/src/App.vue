<template>
<el-container>
  <el-header>
    <el-menu
      :default-active="activeIndex2"
      class="el-menu-demo"
      mode="horizontal"
      @select="handleSelect"
      background-color="#545c64"
      text-color="#fff"
      active-text-color="#ffd04b">
      <el-menu-item index="1">首页</el-menu-item>
      <el-submenu index="2">
        <template slot="title">宝宝</template>
        <el-menu-item index="/Photos">照片</el-menu-item>
        <el-menu-item index="/Videos">视频</el-menu-item>
        <el-menu-item index="/Diaries" disabled>日记</el-menu-item>
      </el-submenu>
      <el-menu-item index="/EnglishLearning">英语学习</el-menu-item>
    </el-menu>
  </el-header>
  <el-container>
    <el-main>
      <div id="app">
        <!-- <img src="./assets/logo.png"> -->
        <router-view/>
      </div>
    </el-main>
  </el-container>
  <el-footer>
    footer
  </el-footer>
</el-container>
</template>

<script>
export default {
  name: 'App',
  data () {
    return {
      activeIndex: '1',
      activeIndex2: '1'
    }
  },
  methods: {
    handleSelect (key, keyPath) {
      console.log(key, keyPath)
    },
    handleOpen (key, keyPath) {
      console.log(key, keyPath)
    },
    handleClose (key, keyPath) {
      console.log(key, keyPath)
    }
  }
}
</script>

<style>
#app {
  font-family: "Avenir", Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  margin-top: 5px;
}

.el-header {
  padding: 0px;
  background-color: #b3c0d1;
  color: #333;
  line-height: 60px;
}

.el-aside {
  width: 150px !important;
  color: #333;
}
</style>
