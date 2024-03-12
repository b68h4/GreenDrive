<template>
    <v-dialog v-model="popup" persistent max-width="700">
        <v-card>
            <v-card-title class="justify-center">{{ strings.our_telegram_channels }}</v-card-title>
            <v-container>
                <v-row>
                    <v-col md="6">
                        <v-spacer></v-spacer>
                        <v-card align="center" elevation="3" width="800" height="200">
                            <div style="padding-top:30px;">
                                <v-icon elevation="2" size="72">fa-brands fa-telegram</v-icon>
                                <v-card-subtitle>
                                    <h2>{{ channel_name }}</h2>
                                </v-card-subtitle>
                                <v-btn :href="channel_link" @click="join('channel')" target="_blank">{{ strings.join }}</v-btn>
                            </div>
                        </v-card>
                    </v-col>
                    <v-col md="6">
                        <v-spacer></v-spacer>
                        <v-card align="center" elevation="3" width="800" height="200">
                            <div style="padding-top:30px;">
                                <v-icon elevation="2" size="72">fa-brands fa-telegram</v-icon>
                                <v-card-subtitle>
                                    <h2>{{ group_name }}</h2>
                                </v-card-subtitle>
                                <v-btn :href="group_link" @click="join('chat')" target="_blank">{{ strings.join }}</v-btn>
                            </div>
                        </v-card>
                    </v-col>
                </v-row>
            </v-container>

            <v-card-actions>
                <v-checkbox v-model="donotshow" :label="strings.do_not_show"> </v-checkbox>
                <v-spacer></v-spacer>
                <v-btn color="primary" text @click="ok"> {{ strings.ok }}! </v-btn>
            </v-card-actions>
        </v-card>
    </v-dialog>
</template>

<script>
import config from "../config";
export default {
    name: "TgPopup",
    data() {
        return {
            popup: true,
            donotshow: false,
            channel_name: config.tg_ads.channel_name,
            channel_link: config.tg_ads.channel_link,
            group_name: config.tg_ads.group_name,
            group_link: config.tg_ads.group_link,
            strings: config.language_strings[config.language],
        };
    },
    methods: {
        ok: function() {
            if (this.donotshow) {
                localStorage.donotshow = true;
            }
            this.popup = false;
        },
        join: function(type) {
            if (config.gtag.enabled) {
                this.$gtag.event("open_telegram_link", {
                    event_category: "open_telegram_link",
                    event_label: type,
                    value: "Join",
                });
            }
        },
    },
    mounted() {
        if (localStorage.donotshow == "true") {
            this.popup = false;
        }
    },
};
</script>
