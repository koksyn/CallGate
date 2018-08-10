import SessionStorageService from "./storage/SessionStorageService";
import Vue from 'vue';

export default {
    storageService: SessionStorageService,
    setBearerToken() {
        if (!!this.storageService.getAuthenticationToken()) {
            Vue.http.headers.common["Authorization"] = "Bearer " + this.storageService.getAuthenticationToken();
        }
    },
    isAuthenticated() {
        let expirationDate = this.storageService.getAuthenticationTokenExpirationDate();

        return (!expirationDate) ? false : expirationDate > new Date();
    },
    setAuthenticationToken(token, expirationDate) {
        this.storageService.setAuthenticationToken(token);
        this.storageService.setAuthenticationTokenExpirationDate(expirationDate);
    },
    getAuthenticationToken() {
        return this.storageService.getAuthenticationToken();
    },
    clearAuthenticationToken() {
        this.storageService.setAuthenticationToken(null);
        this.storageService.setAuthenticationTokenExpirationDate(null);
    }
}
