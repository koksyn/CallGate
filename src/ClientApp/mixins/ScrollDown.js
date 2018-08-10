export default {
    data: function () {
        return {
            scrollDownNeeded: false
        }
    },
    updated: function () {
        if (this.scrollDownNeeded) {
            this.scrollDown();
        }
    },
    methods: {
        scrollDown: function () {
            document.documentElement.scrollTop = document.documentElement.scrollHeight;
            this.scrollDownNeeded = false;
        },
        scrollDownOnNextUpdate: function () {
            this.scrollDownNeeded = true;
        }
    }
}