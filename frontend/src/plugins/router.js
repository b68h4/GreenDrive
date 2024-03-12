import Vue from "vue";
import VueRouter from "vue-router";
import Reader from "../pages/Reader.vue";
import Player from "../pages/Player.vue";
import Main from "../pages/Main.vue";
import NotFound from "../components/NotFound.vue";

import config from "../config";
Vue.use(VueRouter);

const routes = [
    { path: "/", name: config.appName, component: Main },
    { path: "/Player", name: config.appName + " - Player", component: Player },
    { path: "/Reader", name: config.appName + " - PDF Reader", component: Reader },
    { path: "*", name: config.appName + " - Page not found", component: NotFound },
];

const router = new VueRouter({
    mode: "history",
    routes,
});

export default router;
