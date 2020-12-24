<template>
  <div>
    <h1>主页面</h1>
    <!--还要写一个学生界面，这里暂时没有写 -->
    <Admin/>
  </div>
</template>

<script>
import Admin from "@/components/admin/Admin"

export default {
  name: "HomePage",
  components: {
    Admin
  }
}
</script>

<style scoped>

</style>