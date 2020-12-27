<template>
  <el-container>


    <el-aside style="background-color: rgb(238, 241, 246)">
      <p>管理员</p>
      <el-menu default-active="1" @select="onMenuItemSelected">

        <el-submenu index="1">
          <template slot="title"><i class="el-icon-message"></i>管理学生</template>
          <el-menu-item-group>
            <el-menu-item index="1-1">列出所有学生的信息</el-menu-item>
            <el-menu-item index="1-2">修改学生信息</el-menu-item>
            <el-menu-item index="1-3">删除学生信息</el-menu-item>
          </el-menu-item-group>
        </el-submenu>

        <el-submenu index="2">
          <template slot="title"><i class="el-icon-message"></i>管理管理员</template>
          <el-menu-item-group>
            <el-menu-item index="2-1">列出所有管理员信息</el-menu-item>
            <el-menu-item index="2-2">删除管理员信息</el-menu-item>
          </el-menu-item-group>
        </el-submenu>

        <el-submenu index="3">
          <template slot="title"><i class="el-icon-message"></i>管理商品</template>
          <el-menu-item-group>
            <el-menu-item index="3-1">列出所有商品信息</el-menu-item>
            <el-menu-item index="3-2">删除商品信息</el-menu-item>
          </el-menu-item-group>
        </el-submenu>

        <el-submenu index="4">
          <template slot="title"><i class="el-icon-message"></i>管理交易记录</template>
          <el-menu-item-group>
            <el-menu-item index="4-1">列出所有交易记录</el-menu-item>
            <el-menu-item index="4-2">删除交易记录</el-menu-item>
          </el-menu-item-group>
        </el-submenu>

        <el-menu-item index="5">
          <template slot="title"><i class="el-icon-message"></i>个人信息</template>
        </el-menu-item>


      </el-menu>
    </el-aside>

    <el-main>
      <div v-if="index==='1-1'">
        <el-table :data="students" height="250" border style="width: 100%">
          <el-table-column prop="id" label="id" width="180"></el-table-column>
          <el-table-column prop="userName" label="昵称" width="180"></el-table-column>
          <el-table-column prop="name" label="真实姓名" width="180"></el-table-column>
          <el-table-column prop="serialNumber" label="学号" width="180"></el-table-column>
        </el-table>
      </div>

      <div v-else-if="index==='1-2'">
        <ChangeStudents/>
      </div>

      <div v-else-if="index==='1-3'">
        <DeleteStudents :judge="{student:0}"/>
      </div>

      <div v-else-if="index==='2-1'">
        <el-table :data="students" height="250" border style="width: 100%">
          <el-table-column prop="id" label="id" width="180"></el-table-column>
          <el-table-column prop="userName" label="昵称" width="180"></el-table-column>
          <el-table-column prop="name" label="真实姓名" width="180"></el-table-column>
          <el-table-column prop="serialNumber" label="工号" width="180"></el-table-column>
        </el-table>
      </div>
      <div v-else-if="index==='2-2'">
        <DeleteStudents :judge="{student:1}"/>
      </div>
      <div v-else-if="index==='3-1'">
        <el-table :data="commodities" height="250" border style="width: 100%">
          <el-table-column prop="id" label="商品id" width="180"></el-table-column>
          <el-table-column prop="title" label="商品名称" width="180"></el-table-column>
          <el-table-column prop="description" label="商品详细" width="180"></el-table-column>
          <el-table-column prop="price" label="商品价格" width="180"></el-table-column>
        </el-table>
      </div>
      <div v-else-if="index==='3-2'">
        <DeleteStudents :judge="{student:2}"/>
      </div>

      <div v-else-if="index==='4-1'">
        <el-table :data="salesRecords" height="250" border style="width: 100%">
          <el-table-column prop="id" label="销售记录id" width="180"></el-table-column>
          <el-table-column prop="commodityId" label="商品id" width="180"></el-table-column>
          <el-table-column prop="comment" label="评价（默认为空）" width="180"></el-table-column>
          <el-table-column prop="auction" label="成交价格" width="180"></el-table-column>
          <el-table-column prop="check" label="是否签收" width="180"></el-table-column>
        </el-table>
      </div>
      <div v-else-if="index==='4-2'">
        <DeleteStudents :judge="{student:3}"/>
      </div>
      <div v-else-if="index==='5'">
        <el-header>
          昵称：{{ userName }}
        </el-header>
        <el-header>
          id: {{ userId }}
        </el-header>
        <el-header>
          姓名： {{ name }}
        </el-header>
        <el-header>
          学（工）号： {{ serial }}
        </el-header>
      </div>
    </el-main>

  </el-container>
</template>

<script>
import ChangeStudents from "@/components/admin/ChangeStudents";
import DeleteStudents from "@/components/admin/DeleteStudents";

export default {
  name: "Admin",
  components: {
    ChangeStudents,
    DeleteStudents
  },
  data() {
    return {
      index: '0',
      students: [],
      commodities: [],
      admins: [],
      salesRecords: [],
      limit: 100,
      userName: localStorage.getItem('userName'),
      userId: localStorage.getItem('userId'),
      token: localStorage.getItem('token'),
      name: localStorage.getItem('name'),
      gender: localStorage.getItem('gender'),
      serial: localStorage.getItem('serialNumber')
    }

  },
  methods: {
    onMenuItemSelected(index) {
      this.index = index
      let url = this.GLOBAL.domain
      if (this.index === '1-1') //列出学生信息
        url += `/statistic/listStudents`
      else if (this.index === '2-1')
        url += `/statistic/listAdmins`
      else if (this.index === '3-1')
        url += `/statistic/listCommodities`
      else if (this.index === '4-1')
        url += `/statistic/listSalesRecords`
      url += `?limit=${this.limit}`
      this.GLOBAL.fly.get(url)
          .then(response => {
            if (this.index === '1-1' && this.index === '2-1')
              this.students = response.data
            else if(this.index==='3-1')
              this.commodities=response.data
            else if(this.index==='4-1')
              this.salesRecords=response.data
            console.log(response)
          })
          .catch(error => {
            console.log(error)
          })

    }

  }

}
</script>

<style scoped>

</style>