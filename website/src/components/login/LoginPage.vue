<template>
  <div>
    <lottie :options="playOptions" :height="250" :width="250"/>

    <h1>登陆页面</h1>

    <p>{{ cnt }}</p>

    <el-button @click="addOne" type="primary">点击+1</el-button>
  </div>
</template>

<script>
import animationData from '../../assets/anim.json'

//view model
export default {
  name: "LoginPage",
  data() {
    return {
      playOptions: {
        animationData: animationData,
        loop: true,
        autoPlay: true
      },
      cnt: 0
    }
  },
  methods: {
    addOne() {
      this.cnt++
    }
  }
}
</script>

<style scoped>

</style>