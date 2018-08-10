import SocketManager, { SocketTypes } from "../services/socket/SocketManager";

export default {
    created: function () {
        this.initializeReactor();
    },
    beforeDestroy: function () {
        this.disposeReactor();
    },
    methods: {
        initializeReactor: function () {
            if (SocketManager.isConnected(SocketTypes.Events)) {
                SocketManager.addMessageListener(SocketTypes.Events, this.eventListenerAndEmitter);
                SocketManager.addMessageListener(SocketTypes.ReceiveMessage, this.messageListenerAndEmitter);
            } else {
                this.$events.on('socketsConnected', () => {
                    SocketManager.addMessageListener(SocketTypes.Events, this.eventListenerAndEmitter);
                    SocketManager.addMessageListener(SocketTypes.ReceiveMessage, this.messageListenerAndEmitter);
                });
            }
        },
        disposeReactor: function () {
            SocketManager.removeMessageListener(SocketTypes.Events, this.eventListenerAndEmitter);
            SocketManager.removeMessageListener(SocketTypes.ReceiveMessage, this.messageListenerAndEmitter);
        },
        eventListenerAndEmitter: function (eventSerialized) {
            let event = JSON.parse(eventSerialized.data);

            this.$events.emit("SocketEvent", event);
            this.$events.emit("SocketEventType:" + event.type, event);
        },
        messageListenerAndEmitter: function (messageSerialized) {
            let message = JSON.parse(messageSerialized.data);
            
            this.$events.emit("MessageReceived", message);
        }
    }
}