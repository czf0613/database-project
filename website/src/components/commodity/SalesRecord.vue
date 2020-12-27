<template>
  <div class="center">
    <p>交易ID：{{ salesRecord.id }}</p>
    <p>商品ID：{{ salesRecord.commodityId }}</p>
    <p>交易时间：{{ salesRecord.transactionTime }}</p>
    <p>收货地址：{{ salesRecord.addressDetail }}</p>
    <p>交易评价：{{ salesRecord.comment }}</p>
    <el-button type="warning" @click="salesRecord.check=true">确认收货</el-button>
    <el-button type="danger" @click="openAddComment">添加评价</el-button>

    <el-dialog title="确认收货" :visible.sync="salesRecord.check">

      <el-form ref="form" label-width="100px" class="right">
        <el-form-item label="交易评价">
          <el-input v-model="commentt"></el-input>
        </el-form-item>
      </el-form>

      <div>
        <el-button type="danger" @click="addComment">添加评价</el-button>
      </div>
      <div slot="footer" class="dialog-footer">
        <el-button @click="salesRecord.check = false">取 消</el-button>
        <el-button type="danger" @click="confirm">确认收货</el-button>
      </div>
    </el-dialog>

    <el-dialog v-if="commentDiaVisible===true" title="添加评价">
      <el-form ref="form" label-width="100px" class="right">

        <el-form-item label="交易评价">
          <el-input v-model="commentt"></el-input>
        </el-form-item>
      </el-form>

      <div slot="footer" class="dialog-footer">
        <el-button @click="commentDiaVisible = false">取 消</el-button>
        <el-button type="danger" @click="addComment">确认修改</el-button>
      </div>

    </el-dialog>
  </div>
</template>

<script>
export default {
  name: "SalesRecord",
  props: {
    salesRecord: {
      id: 0,
      commodityId: 0,
      addressDetail: '',
      comment: '',
      transactionTime: '',
      auction: 0.0,
      check: false
    }
  },
  data() {
    return {
      commentDiaVisible: false,
      commentt: '',
      userName: localStorage.getItem('userName')
    }
  },
  methods: {
    confirm() {
      this.check = true
      let url = this.GLOBAL.domain
      url += `/sales/confirm?salesRecord=${this.salesRecord.id}`
      this.GLOBAL.fly.post(url)
          .then(response => {
            console.log(response)
            //返回SalesRecords对象该怎么使用？？
            alert("收货成功")
          })
          .catch(error => {
                console.log(error)
                alert("收货失败")
              }
          )
    },

    addComment() {
      if (this.salesRecord.check === false)
        return

      this.salesRecord.comment = this.commentt
      let url = this.GLOBAL.domain
      url += `/sales/comment?salesRecord=${this.salesRecord.id}&comment=${this.commentt}`

      this.GLOBAL.fly.post(url)
          .then(response => {
            console.log(response)
            //返回SalesRecords对象该怎么使用？？
            this.commentDiaVisible = false
            alert("评价成功")
          })
          .catch(error => {
                console.log(error)
                alert("评价失败")
              }
          )
    },

    openAddComment() {
      this.commentDiaVisible = true
    }
  }
}
</script>

<style scoped>
.center {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  color: #2c3e50;
  margin-top: 60px;
  text-align: center;
  justify-content: center;
  flex-direction: column;
}

.align {
  display: flex;
  justify-content: center;
  flex-direction: row;
  align-items: center
}
</style>