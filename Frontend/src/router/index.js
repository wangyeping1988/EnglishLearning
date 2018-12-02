import Vue from 'vue'
import Router from 'vue-router'
import Login from '@/components/Login'
import Photos from '@/components/Photos'
import Videos from '@/components/Videos'
import EnglishLearning from '@/components/EnglishLearning'

Vue.use(Router)

export default new Router({
  routes: [
    {
      path: '/',
      name: 'Login',
      component: Login
    },
    {
      path: '/Photos',
      name: 'Photos',
      component: Photos
    },
    {
      path: '/Videos',
      name: 'Videos',
      component: Videos
    },
    {
      path: '/EnglishLearning',
      name: 'EnglishLearning',
      component: EnglishLearning
    }
  ]
})
