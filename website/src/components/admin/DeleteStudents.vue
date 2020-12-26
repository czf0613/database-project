<template>
  <el-container>
    <el-form ref="form" :model="Input" label-width="100px" class="center">
      <el-form-item label="输入id">
        <el-input v-model="Input.id" placeholder="请输入id"></el-input>
      </el-form-item>
      <el-button type="danger" @click="Warning">删除</el-button>
    </el-form>

    <el-dialog v-if="sure" title="警告！确定删除该信息？" :visible.sync="sure">
      <el-header v-if="judge.student===0">删除学生信息</el-header>
      <el-header v-if="judge.student===1">删除管理员信息</el-header>
      <el-button @click="sure=false">取消</el-button>
      <el-button type="danger" @click="DeleteS">确定删除</el-button>
    </el-dialog>

  </el-container>
</template>

<script>
export default {
  name: "DeleteStudents",
  props: {
    judge: {
      student: 0,//默认是0,1是删除管理员，2是删除商品，3是删除记录
    }
  },
  data() {
    return {
      Input: {
        id: 0
      },
      DeleteInformation: {
        id: 0,
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
      sure: false,
    }
  },
  methods: {
    Warning() {
      this.sure = true
    },
    DeleteS() {
      this.sure = false
      let url = this.GLOBAL.domain
      if (this.judge.student === 0)
        url += `/statistic/deleteStudent?stuId=${this.Input.id}`
      else if (this.judge.student === 1)
        url += `/statistic/deleteAdmin?teacherId=${this.Input.id}`
      else if(this.judge.student===2)
        url += `/statistic/deleteCommodity?commodityId=${this.Input.id}`
      else if(this.judge.student===3)
        url += `/statistic/deleteSalesRecords?commodityId=${this.Input.id}`
          this.GLOBAL.fly.delete(url)
              .then(response => {
             //   this.DeleteInformation = response.data
                console.log(response)
              })
              .catch(error => {
                console.log(error)
                alert("失败")
              })
          },


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