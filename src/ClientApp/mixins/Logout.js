import AuthenticationService from "../services/AuthenticationService";
import SocketManager from "../services/socket/SocketManager";

export default {
    methods: {
        logout: function () {
            SocketManager.closeAllConnections();
            AuthenticationService.clearAuthenticationToken();
            this.$router.push("/login");
        }
    }
}