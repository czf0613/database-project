<template>
  <div class="center">
    <div class="align">
      <el-image v-for="url in commodity.photos" lazy :src="url" :key="url">
        <div slot="placeholder" class="image-slot">
          加载中<span class="dot">...</span>
        </div>

        <div>
          div slot="error" class="image-slot">
          <i class="el-icon-picture-outline"></i>
        </div>
      </el-image>
    </div>

    <p>商品名称：{{ commodity.title }}</p>
    <p>商品描述：{{ commodity.description }}</p>
    <p>价格：{{ commodity.price }}</p>
    <el-button type="warning" @click="displayBuy=true">购买</el-button>

    <el-dialog title="购买商品" :visible.sync="displayBuy">

      <el-form ref="form" :model="addressDetail" label-width="100px" class="right">
        <el-form-item label="收件人姓名">
          <el-input v-model="addressDetail.name"></el-input>
        </el-form-item>
        <el-form-item label="收件人地址">
          <el-input v-model="addressDetail.address"></el-input>
        </el-form-item>
        <el-form-item label="收件人电话">
          <el-input v-model="addressDetail.phone"></el-input>
        </el-form-item>
      </el-form>

      <div slot="footer" class="dialog-footer">
        <el-button @click="displayBuy = false">取 消</el-button>
        <el-button type="danger" @click="buy">确认购买</el-button>
      </div>
    </el-dialog>
  </div>
</template>

<script>
export default {
  name: "Commodity",
  props: {
    commodity: {
      id: 0,
      title: '',
      description: '',
      photos: [],
      releaseTime: '',
      price: 0.0
    }
  },
  data() {
    return {
      displayBuy: false,
      addressDetail: {
        name: '',
        address: '',
        phone: ''
      },
      userName: localStorage.getItem('userName')
    }
  },
  methods: {
    buy() {
      this.displayBuy = !this.displayBuy
      let url = this.GLOBAL.domain
      url += `/sales/buy?userName=${this.userName}&commodityId=${this.commodity.id}`
      this.GLOBAL.fly.post(url, this.addressDetail)
          .then(response => {
            console.log(response)
            //返回SalesRecords对象该怎么使用？？
            alert("购买成功")
          })
          .catch(error => {
                console.log(error)
                alert("购买失败")
              }
          )
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