<template>
    <v-container>
        <v-card class="text-center card-size">
            <v-card-title class="justify-center">{{ strings.no_longer_exists }}</v-card-title>
            <v-btn color="red" href="/" class="justify-center" target="_self">{{ strings.go_home }}</v-btn>
        </v-card>
    </v-container>
</template>
<script>
import config from "../config";
export default {
    name: "Not found",
    data() {
        return {
            strings: config.language_strings[config.language],
        };
    },
};
</script>
<style>
.card-size {
    height: 150px;
}
</style>
