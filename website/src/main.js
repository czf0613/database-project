import Vue from 'vue'
import App from './App.vue'
import ElementUI from 'element-ui'
import 'element-ui/lib/theme-chalk/index.css'
import Lottie from 'vue-lottie'
import VueRouter from "vue-router"
import global from './global'
import LoginPage from "@/components/login/LoginPage"
import HomePage from "@/components/home/HomePage"

Vue.config.productionTip = false
Vue.prototype.GLOBAL = global

Vue.use(ElementUI)
Vue.use(VueRouter)
Vue.component('Lottie', Lottie)

const router = new VueRouter({
    mode: 'history',
    routes: [{
        path: '/login',
        component: LoginPage
    }, {
        path: '/home',
        name: 'home',
        component: HomePage
    }, {
        path: '*',
        component: HomePage
    }]
})


router.beforeEach((to, from, next) => {
    if (to.path === '/login') {
        if (checkLogin())
            next("/home")
        else
            next()
    } else {
        if (checkLogin())
            next()
        else
            next("/login")
    }

})


function checkLogin() {
    return localStorage.getItem('token') != null
}

new Vue({
    render: h => h(App),
    router
}).$mount('#app')
