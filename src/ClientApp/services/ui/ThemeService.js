export default {
    enableLoginTheme: function () {
        let loginThemeClass = "body-login";
        let body = document.getElementById("global-body");

        body.className = body.className.replace(/theme\-\w+/g, "theme-indigo");
        let classes = body.className.split(" ");

        if (classes.indexOf(loginThemeClass) === -1) {
            body.className += " " + loginThemeClass;
        }
    },
    disableLoginTheme: function () {
        let body = document.getElementById("global-body");
        let classes = body.className.split(" ");

        if (classes.indexOf("body-login") !== -1) {
            body.className = body.className.replace(/\bbody\-login\b/g, "");
        }
    }
}