<template>
  <div class="center">
    <div class="align">
      <el-image v-for="url in commodity.photos" lazy :src="url" :key="url" style="width: 200px; height: 200px">
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

    <el-button v-if="judge.add === true" type="warning" @click="updateVisible">修改</el-button>

    <el-dialog title="修改商品" :visible.sync="displayUpdate">

      <el-form ref="form" :model="commodity" label-width="100px" class="right">
        <el-form-item label="标题">
          <el-input v-model="commodity.title"></el-input>
        </el-form-item>
        <el-form-item label="商品简介">
          <el-input type="textarea" v-model="commodity.description"></el-input>
        </el-form-item>

        <el-form-item label="商品价格">
          <el-input-number v-model="commodity.price" :precision="2" :controls="false"></el-input-number>
        </el-form-item>
      </el-form>

      <el-upload
          drag
          action="https://pic-bed.xyz/api/upload?userId=1"
          :headers="{token: 'c1c5719e761241c3ab3e1627286b9647'}"
          multiple
          accept=".jpg,.jpeg,.gif,.png,.ico,.bmp,.heif"
          :on-success="afterUpload"
          :on-error="errorHandler">
        <i class="el-icon-upload"></i>
        <div class="el-upload__text">将文件拖到此处，或<em>点击上传</em></div>
        <div class="el-upload__tip" slot="tip">只能上传图片文件</div>
      </el-upload>

      <div slot="footer" class="dialog-footer">
        <el-button @click="displayUpdate = false">取 消</el-button>
        <el-button type="danger" @click="update">确认修改</el-button>
      </div>
    </el-dialog>

    <el-button v-if="judge.add === true" type="warning" @click="deletee">删除</el-button>

  </div>
</template>

<script>
export default {
  name: "Commodity",
  props: {
    judge:{
      add:false
    },
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
      displayUpdate: false,
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
            //返回SalesRecords对象
            alert("购买成功")
          })
          .catch(error => {
                console.log(error)
                alert("购买失败")
              }
          )
    },
    updateVisible(){
      this.displayUpdate = true
    },
    update(){
      let url = this.GLOBAL.domain
      url += `/commodity/update`
      this.GLOBAL.fly.post(url, this.commodity)
          .then(respose => {
            console.log(respose)
            this.displayUpdate = false;
            alert("修改成功")
          })
          .catch(error=>{
            console.log(error)
            alert("修改失败")
          })
    },
    deletee(){
      let url = this.GLOBAL.domain
      url += `/commodity/delete`
      this.GLOBAL.fly.delete(url, this.commodity)
          .then(respose => {
            console.log(respose)
          })
          .catch(error=>{
            console.log(error)
            alert("删除失败")
          })
    },
    afterUpload(response) {
      this.commodity.photos.push(response)
    },
    errorHandler() {
      alert('图片上传失败')
    },
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