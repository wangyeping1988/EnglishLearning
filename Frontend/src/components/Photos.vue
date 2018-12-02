<template>
  <!-- <el-carousel indicator-position="outside" :height="bannerHeight + 'px'"> -->
  <el-carousel arrow="always" indicator-position="outside" height='500px'>
    <el-carousel-item v-for="(item,index) in 4" :key=index>
      <img src="../assets/images/banner1.jpg" v-if="index == 0" class="bannerImg" />
      <img src="../assets/images/banner2.jpg" v-if="index == 1" class="bannerImg" />
      <img src="../assets/images/banner3.jpg" v-if="index == 2" class="bannerImg" />
      <img src="../assets/images/banner4.jpg" v-if="index == 3" class="bannerImg" />
    </el-carousel-item>
  </el-carousel>
</template>

<script>
export default {
  name: 'Photos',
  methods: {
    // setSize: function () {
    //   this.bannerHeight = 740 / 2560 * this.screenWidth
    //   if (this.bannerHeight > 740) this.bannerHeight = 740
    //   if (this.bannerHeight < 360) this.bannerHeight = 360
    // }
  },
  mounted: function () {
    // this.setSize()
    // const that = this
    // window.addEventListener(
    //   'resize',
    //   function () {
    //     that.screenWidth = $(window).width()
    //     that.setSize()
    //   },
    //   false
    // )
  }
}
</script>

<style scoped>
.bannerImg {
  width: 100%;
  height: 100%;
  min-height: 450px;
  min-width: 800px;
}
</style>
