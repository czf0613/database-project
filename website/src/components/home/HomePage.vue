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

    <el-dialog v-if="dialogFormVisible===true" title="密码修改" :visible.sync="changePassOrNot">
      <el-form ref="form" :model="changeInformation" label-width="100px" class="right">

        <el-form-item label="旧密码">
          <el-input v-model="changeInformation.oldPassword" show-password placeholder="请输入旧密码"></el-input>
        </el-form-item>
        <el-form-item label="新密码">
          <el-input v-model="changeInformation.newPassword" show-password placeholder="请输入新密码"></el-input>
        </el-form-item>
        <el-form-item label="再一次">
          <el-input v-model="passWordagain" show-password placeholder="请再输入一次新密码"></el-input>
        </el-form-item>
      </el-form>

      <div slot="footer" class="dialog-footer">
        <el-button @click="dialogFormVisible = false">取 消</el-button>
        <el-button type="danger" @click="confirmPassword">确认修改</el-button>
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
      dialogFormVisible:false,
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
      this.dialogFormVisible=true
    },
    confirmPassword() {
      if (this.changeInformation.newPassword != this.passWordagain) {
        alert('两次密码输入不一致')
        return
      }
      let url = this.GLOBAL.domain
      url += `/changePassword?userName=${this.changeInformation.userName}&oldPassword=${this.changeInformation.oldPassword}&newPassword=${this.changeInformation.newPassword}`
      this.GLOBAL.fly.post(url)
          .then(response => {
            console.log(response)
            this.dialogVisible = false
            alert('修改成功')

          })
          .catch(error => {
            console.log(error)
            alert("修改失败")
          })
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