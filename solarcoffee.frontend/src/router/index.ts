import Vue from 'vue'
import VueRouter, { RouteConfig } from 'vue-router'
import Home from '@/views/Home.vue'
import Inventory from '@/views/Inventory.vue'
import Customers from '@/views/Customers.vue'
import CreateInvoice from '@/views/CreateInvoice.vue'
import Orders from '@/views/Orders.vue'

Vue.use(VueRouter)

const routes: Array<RouteConfig> = [
  {
    path: '/',
    name: 'home',
    component: Home,
  },
  {
    path: '/inventory',
    name: 'inventory',
    component: Inventory,
  },
  {
    path: '/invoice/new',
    name: 'create-invoice',
    component: CreateInvoice,
  },
  {
    path: '/customers',
    name: 'customers',
    component: Customers,
  },
  {
    path: '/orders',
    name: 'orders',
    component: Orders,
  },
]

const router = new VueRouter({
  mode: 'history',
  base: process.env.BASE_URL,
  routes,
})

export default router
