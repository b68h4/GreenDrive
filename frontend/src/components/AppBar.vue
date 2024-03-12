<template>
    <v-app-bar app overflow-hidden elevate-on-scroll>
        <v-app-bar-nav-icon @click="drawer = !drawer"></v-app-bar-nav-icon>
        <v-icon @click="redirect" style="--fa-flash-opacity: 0.24; --fa-flash-scale: 1.4; margin: 2px">fa-solid fa-stars fa-flash</v-icon>
        <v-spacer></v-spacer>
        <v-text-field v-model="searchData" hide-details prepend-icon="mdi-magnify" single-line class="search-bar" v-show="search"></v-text-field>
        <v-spacer></v-spacer>
        <v-checkbox
            style="margin-top: 24px"
            v-model="search"
            color="primary"
            @click="searchData = null"
            class="mdi-18px"
            off-icon="mdi-magnify"
            on-icon="mdi-magnify"
        ></v-checkbox>
        <v-checkbox
            style="margin-top: 24px"
            v-model="$vuetify.theme.dark"
            color="primary"
            class="mdi-18px"
            off-icon="mdi-theme-light-dark"
            on-icon="mdi-theme-light-dark"
        ></v-checkbox>
    </v-app-bar>
</template>

<script>
import "@/design/appbar.css";
export default {
    name: "AppBar",
    data() {
        return {
            search: false,
        };
    },
    created() {},
    methods: {
        redirect: function() {
            window.location = `/`;
        },
    },
    computed: {
        drawer: {
            get() {
                return this.$store.state.drawer;
            },
            set(val) {
                this.$store.commit("drawerUpdate", val);
            },
        },
        searchData: {
            get() {
                return this.$store.state.searchData;
            },
            set(val) {
                this.$store.commit("search", val);
            },
        },
    },
};
</script>
