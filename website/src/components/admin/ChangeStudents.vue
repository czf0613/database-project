<template>
  <div>
    <div v-if="!showStudent">
      <p>修改学生信息</p>
      <el-form ref="form" :model="studentData" label-width="100px" class="center">
        <el-form-item label="id">
          <el-input v-model="studentData.id" placeholder="id"></el-input>
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

        <div>
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

      <el-button type="primary" @click="comfirmChange">确认修改</el-button>
    </div>
    <div v-else-if="showStudent">
<p>显示修改后的信息
  <el-header>
    id:   {{studentData.id}}
  </el-header>
  <el-header>
    userName:   {{studentUserName}}
  </el-header>
  <el-header>
    name:   {{studentData.name}}
  </el-header>
</p>
      <el-button type="primary" @click="goBack">返回</el-button>
    </div>
  </div>
</template>

<script>
export default {
  name: "ChangeStudents",
  data() {
    return {
      studentUserName:'',
      studentData: {
        id: 0,
        role: 0,
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
      passwordAgain: '',
      showStudent: false
    }
  },
  methods: {
    comfirmChange() {
      if (this.passwordAgain !== this.studentData.password) {
        alert("两次密码不一致哦")
        return
      }
      let url = this.GLOBAL.domain
      url += `/statistic/modifyStudent`
      this.GLOBAL.fly.post(url, this.studentData)
          .then(respose => {
            console.log(respose)
            this.studentData=respose.data//不确定哦
            this.studentUserName=respose.data.userName
            this.showStudent=true
          })
          .catch(error=>{
            console.log(error)
            alert("失败")
          })
    },
    goBack(){
      this.showStudent=!this.showStudent
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