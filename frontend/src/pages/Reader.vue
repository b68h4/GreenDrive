<template>
    <v-container fluid>
        <div v-if="adsense.enabled" align="center">
            <InFeedAdsense
                :data-ad-format="adsense.ad_list['reader0_infeed'].id"
                :data-ad-layout-key="adsense.ad_list['reader0_infeed'].layout_key"
                :data-ad-client="adsense.pub_id"
                :data-ad-slot="adsense.ad_list['reader0_infeed'].id"
                data-full-width-responsive="yes"
            >
            </InFeedAdsense>
        </div>
        <iframe class="reader" v-bind:src="url"></iframe>
    </v-container>
</template>

<script>
import config from "../config";
export default {
    name: config.appName + " - Reader",
    data() {
        return {
            url: null,
            adsense: config.adsense,
        };
    },
    computed: {},
    components: {},
    mounted() {
        let id = this.$route.query.file;
        this.url = `/pdfjs/web/viewer.html?file=${config.apiUrl ? config.apiUrl : ""}/Api/Reader?data=${id}`;
    },
    methods: {
        download: function() {
            document.location.href = `${config.apiUrl ? config.apiUrl : ""}/Api/Reader?data=${this.$route.query.file}`;
        },
    },
};
</script>
<style>
.reader {
    height: 60rem;
    width: 100%;
    max-width: 100%;
    max-height: 100%;
}
</style>
