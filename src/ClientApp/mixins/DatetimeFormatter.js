import moment from "moment";

export default {
    methods: {
        formatDate: function (value) {
            return moment(value).format("YYYY-MM-DD");
        },
        formatDateTime: function (value) {
            return moment(value).format("YYYY-MM-DD HH:mm");
        },
        formatDetailedDateTime: function(value) {
            return moment(value).format("MMMM Do YYYY, h:mm:ss a");
        }
    }
}