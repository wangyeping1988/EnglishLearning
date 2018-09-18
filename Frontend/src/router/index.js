import Vue from 'vue'
import Router from 'vue-router'
import HelloWorld from '@/components/HelloWorld'
import Slides from '@/components/Slides'

Vue.use(Router)

export default new Router({
  routes: [
    {
      path: '/',
      name: 'Slides',
      component: Slides
    },
    {
      path: '/HelloWorld',
      name: 'HelloWorld',
      component: HelloWorld
    }
  ]
})
