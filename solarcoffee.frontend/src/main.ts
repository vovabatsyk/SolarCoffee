import Vue from "vue";
import App from "./App.vue";
import router from "./router";
import store from "./store";
import moment from 'moment'

Vue.config.productionTip = false;

Vue.filter('price', function(number:number){
  if(isNaN(number)) {
    '-'
  } 
    return `\$${number.toFixed(2)}`
})

Vue.filter('humanizeDate', function(date:Date){
  moment.locale('uk')
  return moment(date).format('LL')
})

new Vue({
  router,
  store,
  render: h => h(App)
}).$mount("#app");
