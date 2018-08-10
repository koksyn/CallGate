export default {
    data: function () {
        return {
            preloaderLoading: false
        }
    },
    methods: {
        showDetailsPreloader: function () {
            if (!this.preloaderLoading) {
                this.preloaderLoading = true;
            }
        },
        hideDetailsPreloader: function () {
            if (this.preloaderLoading) {
                this.preloaderLoading = false;
            }
        }
    },
}