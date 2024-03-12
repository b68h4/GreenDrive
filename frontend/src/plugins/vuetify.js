import Vue from "vue";
import Vuetify from "vuetify/lib/framework";

Vue.use(Vuetify);

export default new Vuetify({
  theme: {
    dark: true,
    themes: {
      dark: {
        primary: "#00ad5f",
        secondary: "#FFFFFF",
        accent: "#00ad5f",
      },
    },
  },
});
