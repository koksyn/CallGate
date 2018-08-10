import TypeCheckService from "../utils/TypeCheckService";
import SessionStorageService from "../storage/SessionStorageService";

export default {
    cloneEmptyStack: function () {
        return {
            authorized: [],
            anonymous: []
        };
    },
    pushAuthorized(message, color = 'alert-success') {
        this.push(message, 'authorized', color);
    },
    pushAnonymous(message, color = 'alert-success') {
        this.push(message, 'anonymous', color);
    },
    push(message, rule, color) {
        let stack = this.cloneEmptyStack();
        
        let loaded = SessionStorageService.getObject("NotificationStack");
        if (loaded !== null && TypeCheckService.isNotEmptyObject(loaded)) {
            stack = loaded;
        }
        
        stack[rule].push({ message: message, color: color });
        SessionStorageService.setObject("NotificationStack", stack);
    },
    shift(rule) {
        let stack = this.cloneEmptyStack();
        let notification = null;
        
        let loaded = SessionStorageService.getObject("NotificationStack");

        if (loaded !== null && TypeCheckService.isNotEmptyObject(loaded)) {
            stack = loaded;

            if (stack[rule].length > 0) {
                notification = stack[rule].shift();
            }
        }

        SessionStorageService.setObject("NotificationStack", stack);

        return notification;
    },
    displayAllForAuthorized() {
        let notification = this.shift('authorized');

        while (notification !== null) {
            this.displayNotification(notification.message, notification.color);

            notification = this.shift('authorized');
        } 
    },
    displayAllForAnonymous() {
        let notification = this.shift('anonymous');

        while (notification !== null) {
            this.displayNotification(notification.message, notification.color);

            notification = this.shift('anonymous');
        }
    },
    displayNotification(message, color = 'alert-success') {
        showNotification(color, message, "top", "center", "", "");
    },
    clearStack() {
        sessionStorage.removeItem("NotificationStack");
    }
}