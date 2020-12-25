<template>
  <el-container>
    <el-header>
      <el-col :span="3">
        <el-button type="danger" @click="logOut">退出登录</el-button>
      </el-col>

      <el-col :span="3" :offset="1">
        <el-button type="danger" @click="changePassword">修改密码</el-button>
      </el-col>
    </el-header>

    <el-main>
      <Student v-if="page===0"/>

      <Admin v-else-if="page===1"/>
    </el-main>

    <el-dialog title="密码修改" :visible.sync="changePassOrNot">
      <el-form ref="form" :model="changeInformation" label-width="100px" class="right">

        <el-form-item label="旧密码">
          <el-input v-model="changeInformation.oldPassword" placeholder="请输入旧密码"></el-input>
        </el-form-item>
        <el-form-item label="新密码">
          <el-input v-model="changeInformation.newPassword" placeholder="请输入新密码"></el-input>
        </el-form-item>
        <el-form-item label="再一次">
          <el-input v-model="passWordagain" placeholder="请再输入一次新密码"></el-input>
        </el-form-item>
      </el-form>

      <div slot="footer" class="dialog-footer">
        <el-button @click="dialogFormVisible = false">取 消</el-button>
        <el-button type="danger" @click="confirm">确认修改</el-button>
      </div>
    </el-dialog>
  </el-container>
</template>

<script>
import Admin from "@/components/admin/Admin";
import Student from "@/components/student/Student";

export default {
  name: "HomePage",
  components: {
    Admin,
    Student
  },
  data() {
    return {
      page: 0,
      changePassOrNot: false,
      passWordagain: '',
      changeInformation: {
        userName: localStorage.getItem('userName'),
        oldPassword: '',
        newPassword: ''
      }
    }
  },
  methods: {
    logOut() {
      localStorage.clear()
      this.$router.replace('login')
    },
    changePassword() {
      this.changePassOrNot = !this.changePassOrNot
    },
    confirm(){
      if(this.changeInformation.newPassword!=this.passWordagain){
        alert('两次密码输入不一致')
        return
      }
    }
  },
  mounted() {
    this.page = this.$route.params.page
  }
}
</script>

<style scoped>
.right {
  display: flex;
  justify-content: right;
  flex-direction: column;
}
</style>