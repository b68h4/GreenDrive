<template>
    <v-container id="plyrct">
        <video class="plyr-theme" id="plyr"></video>
        <v-card elevation="2" style="margin-top: 16px" v-if="meta != null">
            <v-card-subtitle>
                <h3>{{ meta.Name }}</h3>
                <h5>{{ strings.uploaded_on }}: {{ meta.ModTime }}</h5>
            </v-card-subtitle>
        </v-card>
        <v-divider style="margin: 15px"></v-divider>
        <div v-if="adsense.enabled" align="center">
            <Adsense :data-ad-client="adsense.pub_id" :data-ad-slot="adsense.ad_list['player1_normal'].id" data-full-width-responsive="yes"> </Adsense>
        </div>
        <div v-if="adsense.enabled" align="center" style="margin-top:3px;">
            <InFeedAdsense
                :data-ad-format="adsense.ad_list['player0_infeed'].id"
                :data-ad-layout-key="adsense.ad_list['player0_infeed'].layout_key"
                :data-ad-client="adsense.pub_id"
                :data-ad-slot="adsense.ad_list['player0_infeed'].id"
                data-full-width-responsive="yes"
            ></InFeedAdsense>
        </div>
    </v-container>
</template>
<script>
import Plyr from "plyr";
import "@/design/player.css";
import config from "../config";

export default {
    name: config.appName + " - Player",
    data() {
        return {
            plyr: null,
            meta: null,
            adsense: config.adsense,
            strings: config.language_strings[config.language],
        };
    },
    computed: {},
    methods: {
        fill(id) {
            fetch(`${config.apiUrl ? config.apiUrl : ""}/Api/Query?id=${id}`)
                .then((response) => response.json())
                .then((data) => {
                    document.title = data.Name;
                    this.meta = data;
                });
        },
    },
    mounted() {
        this.$router.beforeEach(() => {
            this.$store.state.searchData = null;
            this.plyr.stop();
            this.plyr.media.src = "";
        });

        if (this.plyr == null) {
            this.plyr = new Plyr(document.getElementById("plyr"));
        }
        this.fill(this.$route.query.file);
        this.plyr.media.src = `${config.apiUrl ? config.apiUrl : this.baseUrl}/Api/Player?data=${this.$route.query.file}`;
    },
};
</script>
