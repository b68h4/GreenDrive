<template>
    <v-app app>
        <v-main>
            <v-container id="container">
                <AppBar></AppBar>
                <Drawer></Drawer>

                <router-view></router-view>
                <About></About>
                <TgPopup v-if="tg_ads"></TgPopup>
            </v-container>
        </v-main>
    </v-app>
</template>

<script>
import Drawer from "./components/Drawer.vue";
import AppBar from "./components/AppBar.vue";
import About from "./components/About.vue";
import TgPopup from "./components/TgPopup.vue";
import config from "./config";
let app = {
    name: config.appName,
    components: { Drawer, AppBar, About, TgPopup },
    data() {
        return {
            items: null,
            plyr: null,
            meta: null,
            player: false,
            ads: false,
            tg_ads: config.tg_ads.enabled,
        };
    },

    mounted() {},
};
export default app;
</script>
