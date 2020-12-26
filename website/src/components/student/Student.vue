<template>
  <el-container>
    <el-header>
      <el-col :span="8">
        <el-input v-model="searchKey" placeholder="请输入关键词进行搜索"/>
      </el-col>

      <el-col :span="1" :offset="1">
        <el-button icon="el-icon-search" circle @click="search" @keydown="search"></el-button>
      </el-col>

      <el-col :span="3" :offset="8">
        <el-button type="primary" @click="showAddCommodityPanel">发布商品</el-button>
      </el-col>
    </el-header>

    <el-aside style="background-color: rgb(238,241,246)">
      <el-menu default-active="1" @select="onMenuItemSelected">
        <el-menu-item index="1">
          <template slot="title"><i class="el-icon-message"></i>商城</template>
        </el-menu-item>

        <el-submenu index="2">
          <template slot="title"><i class="el-icon-message"></i>我的商品</template>
          <el-menu-item-group>
            <el-menu-item index="2-1" @click="searchMyAdd">我发布的</el-menu-item>
            <el-menu-item index="2-2" @click="searchMySold">我卖出的</el-menu-item>
            <el-menu-item index="2-3" @click="searchMyBought">我买到的</el-menu-item>
          </el-menu-item-group>
        </el-submenu>

        <el-submenu index="3" >
          <template slot="title"><i class="el-icon-message"></i>个人信息</template>
          <el-menu-item-group>
            <el-menu-item index="3-1">修改个人信息</el-menu-item>
          </el-menu-item-group>
        </el-submenu>
      </el-menu>
    </el-aside>

    <el-main>
      <div v-if="index==='1'">
        <div v-if="!addingCommodity">
          <Commodity v-for="item in commodities" :commodity="item" :judge="{add:false}" :key="item.id"/>
        </div>

        <ReleaseNewOne v-if="addingCommodity"/>
      </div>

      <div v-if="index==='2-1'">
        <Commodity v-for="item in allMyCommodities" :commodity="item" :judge="{add:true}" :key="item.id"/>
      </div>

      <div v-else-if="index==='2-2'">
        <SalesRecord v-for="item in sold" :sold="item" :key="item.id"/>
      </div>

      <div v-else-if="index==='2-3'">
        <SalesRecord v-for="item in bought" :bought="item" :key="item.id"/>
      </div>

      <div v-else-if="index==='3-1'">

      </div>
    </el-main>
  </el-container>
</template>

<script>
import Commodity from "@/components/commodity/Commodity";
import ReleaseNewOne from "@/components/commodity/ReleaseNewOne";
import SalesRecord from "@/components/commodity/SalesRecord";

export default {
  name: "Student",
  components: {
    Commodity,
    ReleaseNewOne,
    SalesRecord
  },
  data() {
    return {
      commodities: [],
      allMyCommodities: [],
      sold: [],
      bought: [],
      searchKey: '',
      addingCommodity: false,
      index: '1',
      userName: localStorage.getItem('userName')
    }
  },
  methods: {
    search() {
      if (this.searchKey.length === 0)
        return

      this.GLOBAL.fly.get(`${this.GLOBAL.domain}/commodity/search?query=${this.searchKey}`)
          .then(response => {
            this.commodities = response.data
            console.log(response)
          })
          .catch(error => {
            console.log(error)
          })
    },
    searchMyAdd() {
      this.GLOBAL.fly.get(`${this.GLOBAL.domain}/statistic/myCommodities?username=${this.userName}`)
          .then(response => {
            this.allMyCommodities = response.data.allMyCommodities
            console.log(response)
          })
          .catch(error => {
            console.log(error)
          })
    },
    searchMySold() {
      this.GLOBAL.fly.get(`${this.GLOBAL.domain}/statistic/myCommodities?username=${this.userName}`)
          .then(response => {
            this.sold = response.data.sold
            console.log(response)
          })
          .catch(error => {
            console.log(error)
          })
    },
    searchMyBought() {
      this.GLOBAL.fly.get(`${this.GLOBAL.domain}/statistic/myCommodities?username=${this.userName}`)
          .then(response => {
            this.bought = response.data.bought
            console.log(response)
          })
          .catch(error => {
            console.log(error)
          })
    },
    showAddCommodityPanel() {
      this.addingCommodity = !this.addingCommodity
    },
    onMenuItemSelected(index) {
      this.index = index

    }
  }
}
</script>

<style scoped>

</style>