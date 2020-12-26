<template>
  <el-container>
    <el-header>
      <el-col :span="3">
        <el-button type="danger" @click="logOut">退出登录</el-button>
      </el-col>

      <el-col :span="3" :offset="1">
        <el-button type="danger" @click="changePassword">修改密码</el-button>
      </el-col>

      <el-col :span="3" :offset="1">
        <el-button type="danger" @click="changeInformation">修改信息</el-button>
      </el-col>
    </el-header>

    <el-main>
      <Student v-if="page===0"/>

      <Admin v-else-if="page===1"/>
    </el-main>

    <el-dialog v-if ="dialogFormVisible === true" title="密码修改" :visible.sync="changePassOrNot">
      <el-form ref="form" :model="changePassInformation" label-width="100px" class="right">

        <el-form-item label="旧密码">
          <el-input v-model="changePassInformation.oldPassword" show-password placeholder="请输入旧密码"></el-input>
        </el-form-item>
        <el-form-item label="新密码">
          <el-input v-model="changePassInformation.newPassword" show-password placeholder="请输入新密码"></el-input>
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

    <el-dialog v-if ="dialogFormVisible === true" title="信息修改" :visible.sync="changeInforOrNot">
      <el-form ref="form" :model="changeInfor" label-width="100px" class="right">

        <el-form-item label="用户名">
          <el-input v-model="changeInfor.userName" placeholder="请输入用户名"></el-input>
        </el-form-item>

        <el-form-item label="密码（不可修改）">
          <el-input v-model="changeInfor.password" show-password placeholder="请输入密码"></el-input>
        </el-form-item>

        <el-form-item label="性别">
          <el-select v-model="changeInfor.gender" placeholder="请选择用户角色">
            <el-option label="男" :value="0"></el-option>
            <el-option label="女" :value="1"></el-option>
            <el-option label="保密" :value="2"></el-option>
            <el-option label="未知" :value="3"></el-option>
          </el-select>
        </el-form-item>

        <el-form-item label="学号或工号">
          <el-input v-model="changeInfor.serialNumber" placeholder="请输入学号或工号"></el-input>
        </el-form-item>

        <el-form-item label="手机号码">
          <el-input v-model="changeInfor.phone" placeholder="请输入手机号码"></el-input>
        </el-form-item>

        <div v-if="this.page===0">
          <el-form-item label="专业">
            <el-input v-model="changeInfor.major" placeholder="专业"></el-input>
          </el-form-item>

          <el-form-item label="学院">
            <el-input v-model="changeInfor.college" placeholder="请输入学院"></el-input>
          </el-form-item>

          <el-form-item label="宿舍号">
            <el-input v-model="changeInfor.dormitory" placeholder="请输入宿舍号"></el-input>
          </el-form-item>
        </div>

      </el-form>

      <div slot="footer" class="dialog-footer">
        <el-button @click="dialogFormVisible = false">取 消</el-button>
        <el-button type="danger" @click="confirmInformation">确认修改</el-button>
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
      dialogFormVisible: true,
      changeInforOrNot: false,
      passWordagain: '',
      changePassInformation: {
        userName: localStorage.getItem('userName'),
        oldPassword: '',
        newPassword: ''
      },
      loginFormData: {
        role: 0,
        userName: '',
        password: ''
      },
      changeInfor: {
        role: 0,
        userName: localStorage.getItem('userName'),
        password: '',
        gender: 2,
        serialNumber: '',
        name: '',
        phone: '',
        birthDay: new Date(),
        college: '',
        major: '',
        dormitory: ''
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
      this.dialogFormVisible = true
    },
    confirmPassword() {
      if (this.changePassInformation.newPassword !== this.passWordagain) {
        alert('两次密码输入不一致')
        return
      }
      let url = this.GLOBAL.domain
      url += `/changePassword?userName=${this.changePassInformation.userName}&oldPassword=${this.changePassInformation.oldPassword}&newPassword=${this.changeInformation.newPassword}`
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
    },
    confirmInformation() {
      let url = this.GLOBAL.domain
      if (this.page === 0) {
        url += `/self`
        this.changeInfor.role = 0
      } else if (this.page === 1) {
        url += `/adminSelf`
        this.changeInfor.role = 1
      }
      //url += `?userName=${this.changeInfor.userName}&password=${this.changeInfor.password}`

      this.GLOBAL.fly.post(url, this.changeInfor)
          .then(response => {
            console.log(response)
            this.loginFormData.role = this.changeInfor.role
            this.loginFormData.userName = this.changeInfor.userName
            this.loginFormData.password = this.changeInfor.password
            this.login()
            alert('修改成功')

          })
          .catch(error => {
            console.log(error)
            alert("修改失败")
          })
    },
    changeInformation() {
      this.changeInforOrNot = !this.changeInforOrNot
      this.dialogFormVisible = true
    }
  },
    created() {
      this.page = this.$route.params.page
      if (this.page === undefined)
        this.page = parseInt(localStorage.getItem('page'))
      else
        localStorage.setItem('page', `${this.page}`)
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