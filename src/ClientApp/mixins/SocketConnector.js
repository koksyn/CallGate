import SocketManager, {SocketTypes} from "../services/socket/SocketManager";
import DialogService from "../services/ui/DialogService";

// https://developer.mozilla.org/en-US/docs/Web/API/CloseEvent
const SocketCloseCode = {
    SOCKET_ABNORMAL_CLOSURE: 1006
};

export default {
    data: function () {
        return {
            socketsConnected: false    
        };
    },
    methods: {
        openAndEmitEventForSockets: function (waitingForConnectionSeconds = 15) {
            this.socketsConnected = false;
            SocketManager.openAllConnections();

            let maxTimeoutMiliseconds = waitingForConnectionSeconds * 1000;
            let intervalMiliseconds = 100;

            let repeat = setInterval(() => {
                maxTimeoutMiliseconds -= intervalMiliseconds;

                if (maxTimeoutMiliseconds <= 0) {
                    this.retryConnectionAfterTimeout();
                    clearInterval(repeat);
                } else if (SocketManager.isAllTypesConnected()) {
                    this.handleSocketErrors();
                    
                    this.socketsConnected = true;
                    console.log("[Sockets] Connected");
                    this.$events.emit('socketsConnected');
                    
                    clearInterval(repeat);
                }
            }, intervalMiliseconds);
        },
        handleSocketErrors: function () {
            SocketManager.addCloseListener(SocketTypes.SendMessage, this.reconnectSockets);
            SocketManager.addCloseListener(SocketTypes.ReceiveMessage, this.reconnectSockets);
            SocketManager.addCloseListener(SocketTypes.Events, this.reconnectSockets);
        },
        reconnectSockets: function (closeEvent) {
            if (closeEvent.code === SocketCloseCode.SOCKET_ABNORMAL_CLOSURE && this.socketsConnected) {
                this.openAndEmitEventForSockets(5);
                DialogService.showConnectionProblemDialogWithTimeout(5);
                console.log(closeEvent);
            }
        }
    }
}