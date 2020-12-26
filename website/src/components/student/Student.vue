<template>
  <el-container>
    <p>学生的页面</p>
    <el-header>
      <el-col :span="3">

      </el-col>

    </el-header>

  </el-container>
</template>

<script>
import Commodity from "@/components/commodity/Commodity";

export default {
  name: "Student",
  components:{
    Commodity
  },
  data() {
    return {

      userName: localStorage.getItem('userName'),
      commodityId: 0,



    }
  },
  methods: {



  }
}
</script>

<style scoped>

</style>