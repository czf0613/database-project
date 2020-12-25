<template>
  <div>
    <lottie :options="playOptions" :height="250" :width="250"/>

    <el-divider/>

    <div v-if="page===0">
      <el-form ref="form" :model="loginFormData" label-width="100px" class="center">
        <el-form-item label="登录角色">
          <el-select v-model="loginFormData.role" placeholder="请选择用户角色">
            <el-option label="学生" :value="0"></el-option>
            <el-option label="管理员" :value="1"></el-option>
          </el-select>
        </el-form-item>

        <el-form-item label="用户名">
          <el-input v-model="loginFormData.userName" placeholder="请输入用户名"></el-input>
        </el-form-item>

        <el-form-item label="密码">
          <el-input v-model="loginFormData.password" show-password placeholder="请输入密码"></el-input>
        </el-form-item>
      </el-form>

      <el-row>
        <el-col :offset="8" :span="3">
          <el-button type="warning" style="width: 150px" @click="forgetPassword">忘记密码</el-button>
        </el-col>

        <el-col :span="3">
          <el-button type="primary" style="width: 150px" @click="goToSignUp">注册</el-button>
        </el-col>

        <el-col :span="3">
          <el-button type="primary" style="width: 150px" @click="login">登录</el-button>
        </el-col>
      </el-row>
    </div>

    <el-container v-else-if="page===1">
      <el-header>
        <el-col :span="3">
          <el-button type="primary" @click="goBack">返回</el-button>
        </el-col>
      </el-header>

      <el-main>
        <el-form ref="form" :model="studentData" label-width="100px" class="center">
          <el-form-item label="角色">
            <el-select v-model="studentData.role" placeholder="请选择用户角色">
              <el-option label="学生" :value="0"></el-option>
              <el-option label="管理员" :value="1"></el-option>
            </el-select>
          </el-form-item>

          <el-form-item label="用户名">
            <el-input v-model="studentData.userName" placeholder="请输入用户名"></el-input>
          </el-form-item>

          <el-form-item label="密码">
            <el-input v-model="studentData.password" show-password placeholder="请输入密码"></el-input>
          </el-form-item>

          <el-form-item label="再次输入密码">
            <el-input v-model="passwordAgain" show-password placeholder="请输入密码"></el-input>
          </el-form-item>

          <el-form-item label="性别">
            <el-select v-model="studentData.gender" placeholder="请选择用户角色">
              <el-option label="男" :value="0"></el-option>
              <el-option label="女" :value="1"></el-option>
              <el-option label="保密" :value="2"></el-option>
              <el-option label="未知" :value="3"></el-option>
            </el-select>
          </el-form-item>

          <el-form-item label="真实姓名">
            <el-input v-model="studentData.name" placeholder="请输入姓名"></el-input>
          </el-form-item>

          <el-form-item label="生日">
            <el-date-picker
                v-model="studentData.birthDay"
                type="date"
                placeholder="选择日期">
            </el-date-picker>
          </el-form-item>

          <el-form-item label="学号或工号">
            <el-input v-model="studentData.serialNumber" placeholder="请输入学号或工号"></el-input>
          </el-form-item>

          <el-form-item label="手机号码">
            <el-input v-model="studentData.phone" placeholder="请输入手机号码"></el-input>
          </el-form-item>

          <div v-if="studentData.role===0">
            <el-form-item label="专业">
              <el-input v-model="studentData.major" placeholder="专业"></el-input>
            </el-form-item>

            <el-form-item label="学院">
              <el-input v-model="studentData.college" placeholder="请输入学院"></el-input>
            </el-form-item>

            <el-form-item label="宿舍号">
              <el-input v-model="studentData.dormitory" placeholder="请输入宿舍号"></el-input>
            </el-form-item>
          </div>
        </el-form>
      </el-main>

      <el-footer>
        <el-button type="primary" @click="signUp">注册</el-button>
      </el-footer>
    </el-container>
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
      loginFormData: {
        role: 0,
        userName: '',
        password: ''
      },
      studentData: {
        role: 0,
        userName: '',
        password: '',
        gender: 2,
        serialNumber: '',
        name: '',
        phone: '',
        birthDay: new Date(),
        college: '',
        major: '',
        dormitory: ''
      },
      page: 0,
      passwordAgain: ''
    }
  },
  methods: {
    forgetPassword() {
      alert('活该')
    },
    login() {
      this.GLOBAL.fly.post(`${this.GLOBAL.domain}/login?userName=${this.loginFormData.userName}&password=${this.loginFormData.password}&role=${this.loginFormData.role}`)
          .then(response => {
            localStorage.setItem('userName', response.data.credential.userName)
            localStorage.setItem('login', 'true')
            this.$router.replace({name: "home", params: {page: this.loginFormData.role}})
          })
          .catch(error => {
            console.log(error)
            alert('用户名或密码错误')
          })
    },
    goToSignUp() {
      this.page = 1
    },
    goBack() {
      this.page = 0
    },
    signUp() {
      if (this.studentData.password !== this.passwordAgain) {
        alert('两次输入的密码不一样')
        return
      }

      let url = this.GLOBAL.domain
      if (this.studentData.role === 0)
        url += `/signUp`
      else if (this.studentData.role === 1)
        url += `/adminSignUp`
      url += `?userName=${this.studentData.userName}&password=${this.studentData.password}`

      this.GLOBAL.fly.post(url, this.studentData)
          .then(response => {
            console.log(response)
            this.loginFormData.role = this.studentData.role
            this.loginFormData.userName = this.studentData.userName
            this.loginFormData.password = this.studentData.password
            this.login()
          })
          .catch(error => {
            console.log(error)
            alert('注册失败')
          })
    }
  }
}
</script>

<style scoped>
.center {
  display: flex;
  justify-content: center;
  flex-direction: column;
  align-items: center;
}
</style>