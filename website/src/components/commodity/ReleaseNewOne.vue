<template>
  <div>
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

    <el-form ref="form" :model="commodity" label-width="100px" class="center">
      <el-form-item label="商品名称">
        <el-input v-model="commodity.title"></el-input>
      </el-form-item>

      <el-form-item label="商品简介">
        <el-input type="textarea" v-model="commodity.description"></el-input>
      </el-form-item>

      <el-form-item label="商品价格">
        <el-input-number v-model="commodity.price" :precision="2" :controls="false"></el-input-number>
      </el-form-item>
    </el-form>

    <el-button type="primary" @click="release">发布</el-button>
  </div>
</template>

<script>
export default {
  name: "ReleaseNewOne",
  data() {
    return {
      commodity: {
        id: 0,
        title: '',
        description: '',
        price: 0.0,
        releaseTime: new Date(),
        photos: []
      },
      userName: localStorage.getItem('userName')
    }
  },
  methods: {
    afterUpload(response) {
      this.commodity.photos.push(response)
    },
    errorHandler() {
      alert('图片上传失败')
    },
    release() {
      this.GLOBAL.fly.post(`${this.GLOBAL.domain}/commodity/new?userName=${this.userName}`, this.commodity)
          .then(response => {
            console.log(response)
            location.reload()
          })
          .catch(error => {
            console.log(error)
            alert('发布失败')
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