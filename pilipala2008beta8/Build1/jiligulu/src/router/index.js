import Vue from 'vue'
import VueRouter from 'vue-router'

Vue.use(VueRouter)

const routes = [{
    path: '/',
    name: 'Home',
    component: () => import('../views/Home.vue')
  },
  {
    path: '/Edit',
    name: 'Edit',
    component: () => import('../views/Edit.vue')
  },
  {
    path: '/PostList',
    name: 'PostList',
    component: () => import('../views/PostList.vue')
  },
  {
    path: '/Iteration',
    name: 'Iteration',
    component: () => import('../views/Iteration.vue')
  },
  {
    path: '/Comment_Home',
    name: 'Comment_Home',
    component: () => import('../views/Comment_Home.vue')
  },
  {
    path: '/Comment_List',
    name: 'Comment_List',
    component: () => import('../views/Comment_List.vue')
  },
  {
    path: '/User',
    name: 'User',
    component: () => import('../views/User.vue')
  },
  {
    path: '/Setting',
    name: 'Setting',
    component: () => import('../views/Setting.vue')
  },
]

const router = new VueRouter({
  routes
})

export default router