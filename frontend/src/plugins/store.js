import Vue from "vue";
import Vuex from "vuex";
import config from "../config";

Vue.use(Vuex);
const strings = config.language_strings[config.language];

const store = {
    state: {
        drawer: false,
        searchData: null,
        about: false,
        menu: [
            {
                title: strings.home,
                icon: "mdi-home",
                link: "/",
                target: "_self",
            },
        ],
    },
    mutations: {
        drawerUpdate(state, val) {
            state.drawer = val;
        },
        search(state, val) {
            state.searchData = val;
        },
        aboutUpdate(state, val) {
            state.about = val;
        },
        rootsUpdate(state, val) {
            state.roots = val;
        },
    },
    actions: {},
    modules: {},
};
if (config.tg_ads.enabled) {
    store.state.menu.push({
        title: config.tg_ads.channel_name,
        icon: "fa-brands fa-telegram",
        link: config.tg_ads.channel_link,
        target: "_blank",
    });
    store.state.menu.push({
        title: config.tg_ads.group_name,
        icon: "fa-brands fa-telegram",
        link: config.tg_ads.group_link,
        target: "_blank",
    });
}
export default new Vuex.Store(store);
