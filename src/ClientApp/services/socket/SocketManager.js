import AuthenticationService from "../AuthenticationService";

var sockets = [];

export const SocketTypes = {
    ReceiveMessage: "/messages/receive",
    SendMessage: "/messages/send",
    Events: "/events"
};

export default {
    openAllConnections: function () {
        let _this = this;
        let jwtToken = AuthenticationService.getAuthenticationToken();
        
        Object.keys(SocketTypes).forEach(function(key) {
            let socketType = SocketTypes[key];

            if (!_this.isConnected(socketType)) {
                _this.openConnection(socketType, jwtToken);
            }
        });
    },
    closeAllConnections: function () {
        let _this = this;

        Object.keys(SocketTypes).forEach(function(key) {
            let socketType = SocketTypes[key];

            if (_this.isConnected(socketType)) {
                _this.closeConnection(socketType);
            }
        });
    },
    openConnection: function (socketType, jwtToken) {
        let scheme = (document.location.protocol === "https:") ? "wss" : "ws";
        let hostname = document.location.hostname;
        let port = document.location.port;
        
        sockets[socketType] = new WebSocket(scheme + "://" + hostname + ":" + port + socketType + "?jwtToken=" + jwtToken);
    },
    closeConnection: function (socketType) {
        sockets[socketType].close();
    },
    sendMessage: function (socketType, data) {
        if (this.isConnected(socketType)) {
            sockets[socketType].send(data);
        }
    },
    addMessageListener: function (socketType, func) {
        if (this.exist(socketType)) {
            sockets[socketType].addEventListener("message", func);
        }
    },
    removeMessageListener: function (socketType, func) {
        if (this.exist(socketType)) {
            sockets[socketType].removeEventListener("message", func);
        }
    },
    addCloseListener: function (socketType, func) {
        if (this.exist(socketType)) {
            sockets[socketType].addEventListener("close", func);
        }
    },
    removeCloseListener: function (socketType, func) {
        if (this.exist(socketType)) {
            sockets[socketType].removeEventListener("close", func);
        }
    },
    addErrorListener: function (socketType, func) {
        if (this.exist(socketType)) {
            sockets[socketType].addEventListener("error", func);
        }
    },
    removeErrorListener: function (socketType, func) {
        if (this.exist(socketType)) {
            sockets[socketType].removeEventListener("error", func);
        }
    },
    isConnected: function (socketType) {
        return this.exist(socketType) && sockets[socketType].readyState === sockets[socketType].OPEN;
    },
    exist: function (socketType) {
        return sockets[socketType] != null;
    },
    isAllTypesConnected: function () {
        let _this = this;
        let types = Object.keys(SocketTypes);
        
        return types.every((key) => _this.isConnected(SocketTypes[key]));
    }
}