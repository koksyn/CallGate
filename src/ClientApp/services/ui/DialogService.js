export default {
    showConnectionProblemDialogWithTimeout: function (secondsTimeout) {
        swal({
            title: "Connection problems...",
            text: "We can not connect to the server. We will try to connect again in next " + secondsTimeout + " seconds.",
            timer: (parseInt(secondsTimeout) * 1000) - 200,
            showConfirmButton: false,
            type: "error"
        });
    },
    showConnectionProblemDialog: function () {
        swal({
            title: "Connection problems...",
            text: "We can not connect to the server. Please try again later",
            showConfirmButton: true,
            type: "error"
        });
    },
    showServerErrorDialog: function () {
        swal({
            title: "Ooops!",
            text: "Sorry, server error has occurred, some of actions on this page may not work. Please try again later.",
            showConfirmButton: true,
            type: "error"
        });
    }
}