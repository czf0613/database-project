<template>
  <div class="center">
    <el-image
        fit="contain"
        lazy :src="url" style="max-width: 300px; max-height: 300px" alt="图片">
      <img src="../../assets/loading.jpg" slot="placeholder" alt="加载中">
      <img src="../../assets/error.png" slot="error" alt="加载失败">
    </el-image>

    <el-divider/>

    <el-row class="align">
      <el-col :span="10"><p>文件名：{{ fileName }}</p></el-col>

      <el-divider direction="vertical"/>
      <el-button type="danger" @click="displayBuy=true">购买</el-button>
      <el-divider direction="vertical"/>

    </el-row>

    <el-dialog v-if="displayBuy===true" title="购买商品" :visible.sync="displayBuy">

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
    <br>

    <p>图片URL：{{ url }}</p>
  </div>
</template>

<script>
import Student from "@/components/student/Student";
export default {
  name: "commodity",
  props: {
    url: String,
    fileName: String,
    deleteCallBack: Function
  },
  data(){
    return{
      displayBuy:false,
      commodity:{
        id:0,
        title:'',
        description:'',
//没写list url那个
        releaseTime:DateTimeOffset.Now,
        price:0.0,
        seller:Student//不确定这样是不是正确的
      },
      addressDetail: {
        name: '',
        address: '',
        phone: ''
      }
    }
  },
  methods:{
    buy() {
      this.displayBuy = !this.displayBuy
      let url = this.GLOBAL.domain
      url += `/sales/buy?userName=${this.userName}&commodityId=${this.commodityId}`
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
  margin-top: 100px;
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