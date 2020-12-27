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

    <el-container style="flex-direction: row">
      <el-aside>
        <el-menu default-active="1" @select="onMenuItemSelected">
          <el-menu-item index="1">
            <template slot="title"><i class="el-icon-goods"></i>商城</template>
          </el-menu-item>

          <el-submenu index="2">
            <template slot="title"><i class="el-icon-menu"></i>我的商品</template>
            <el-menu-item-group>
              <el-menu-item index="2-1">我发布的</el-menu-item>
              <el-menu-item index="2-2">我卖出的</el-menu-item>
              <el-menu-item index="2-3">我买到的</el-menu-item>
            </el-menu-item-group>
          </el-submenu>
        </el-menu>
      </el-aside>

      <el-main>
      <span v-if="index==='1'">
        <div v-if="!addingCommodity">
          <Commodity v-for="item in commodities" :commodity="item" :judge="{add:false}" :key="item.id"/>
        </div>

        <ReleaseNewOne v-if="addingCommodity"/>
      </span>

        <span v-if="index==='2-1'">
        <Commodity v-for="item in allMyCommodities" :commodity="item" :judge="{add:true}" :key="item.id"/>
      </span>

        <span v-else-if="index==='2-2'">
        <SalesRecord v-for="item in sold" :salesRecord="item" :key="item.id"/>
      </span>

        <span v-else-if="index==='2-3'">
        <SalesRecord v-for="item in bought" :salesRecord="item" :key="item.id"/>
      </span>
      </el-main>
    </el-container>
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
            this.sold = response.data.sold
            this.bought = response.data.bought
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
  },
  watch: {
    index: function () {
      if (this.index.indexOf("2") !== -1)
        this.searchMyAdd()
    }
  }
}
</script>

<style scoped>
.el-header, .el-footer {
  background-color: #B3C0D1;
  color: #333;
  text-align: center;
  line-height: 60px;
}

.el-aside {
  background-color: #D3DCE6;
  color: #333;
  text-align: center;
  line-height: 200px;
}

.el-main {
  background-color: #E9EEF3;
  color: #333;
  text-align: center;
}
</style>