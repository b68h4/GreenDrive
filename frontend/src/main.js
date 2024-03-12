import Vue from "vue";
import App from "./App.vue";
import vuetify from "./plugins/vuetify";
import store from "./plugins/store";
import Ads from "vue-google-adsense";
import VueGtag from "vue-gtag";
import router from "./plugins/router";
import config from "./config";
Vue.config.productionTip = false;
Vue.use(require("vue-script2"));
Vue.use(Ads.Adsense);
Vue.use(Ads.InArticleAdsense);
Vue.use(Ads.InFeedAdsense);

if (config.adsense.enabled) {
    Vue.use(Ads.AutoAdsense, {
        adClient: config.adsense.pub_id,
        isNewAdsCode: false,
    });
}

if (config.gtag.enabled) {
    Vue.use(
        VueGtag,
        {
            config: { id: config.gtag.gtag_id },
            appName: config.appName,
            pageTrackerScreenviewEnabled: true,
        },
        router
    );
}

new Vue({
    vuetify,
    store,
    router,
    render: (h) => h(App),
}).$mount("#app");
