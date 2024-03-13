<template>
    <v-dialog v-model="about" persistent max-width="700">
        <v-card>
            <v-toolbar> {{ strings.about }} </v-toolbar>
            <div class="pa-12 text-center">
                <p>{{ strings.used_techs }}:</p>

                <v-icon color="green" class="click-cursor" size="28px" title="Vue.js" @click="toLink('https://vuejs.org')"> mdi-vuejs</v-icon>
                <v-icon color="green" class="click-cursor" title="Vuetify.js" size="28px" @click="toLink('https://vuetifyjs.com')">mdi-vuetify</v-icon>
                <v-icon color="purple" class="click-cursor" title=".NET / C#" size="28px" @click="toLink('https://dotnet.microsoft.com/')">mdi-dot-net</v-icon>
            </div>
            <div class="pa-4 text-center">
                <p>{{ strings.developer }}:</p>
                <a href="https://github.com/b68h4" target="_blank">@b68h4</a>
            </div>
            <v-card-actions>
                <v-spacer></v-spacer>
                <v-btn color="primary" text @click="about = false"> {{ strings.ok }} </v-btn>
            </v-card-actions>
        </v-card>
    </v-dialog>
</template>

<script>
import config from "../config";
export default {
    name: "About",
    computed: {
        about: {
            get() {
                return this.$store.state.about;
            },
            set(val) {
                this.$store.commit("aboutUpdate", val);
            },
        },
    },
    data() {
        return {
            strings: config.language_strings[config.language],
        };
    },
    methods: {
        toLink: function(link) {
            window.open(link, "newtab");
        },
    },
};
</script>
