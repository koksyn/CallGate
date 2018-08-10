import AuthenticationService from "../services/AuthenticationService";
import CacheService from "../services/CacheService";
import DialogService from "../services/ui/DialogService";
import Logout from "./Logout";

const HttpStatus = {
    CONNECTION_BROKEN: 0,
    BAD_REQUEST: 400,
    UNAUTHORIZED: 401,
    FORBIDDEN: 403,
    CONFLICT: 409,
    UNPROCESSABLE_ENTITY: 422,
    INTERNAL_SERVER_ERROR: 500
};

export default {
    mixins: [Logout],
    methods: {
        handleFullAsyncHttpError: function (error) {
            if (AuthenticationService.isAuthenticated()) {
                let httpStatus = error.status;

                if (this.isUnauthorizedAttempt(httpStatus)) {
                    this.logout();
                } else if (this.isServerError(httpStatus) || this.isConnectionBroken(httpStatus)) {
                    this.retryConnectionAfterTimeout();
                }
            }
        },
        handleAsyncHttpError: function (error, mode = 'normal') {
            if (AuthenticationService.isAuthenticated()) {
                let httpStatus = error.status;

                if (this.isUnauthorizedAttempt(httpStatus))
                {
                    this.logout();
                }
                else if (this.isServerError(httpStatus))
                {
                    this.retryConnectionAfterTimeout();
                }
                else if (this.isConflict(httpStatus) && mode !== 'conflicted')
                {
                    CacheService.clear();
                    Promise.resolve().then(this.reloadFullAsyncData('conflicted'));
                }
                else if (this.isForbidden(httpStatus))
                {
                    Promise.resolve().then(this.reloadAsyncData());
                }
            }
        },
        handleHttpNotValidationErrors: function (error) {
            console.log(error);
            
            let httpStatus = error.status;

            if (this.isUnauthorizedAttempt(httpStatus) && AuthenticationService.isAuthenticated()) {
                this.logout();
            } else if (this.isServerError(httpStatus)) {
                DialogService.showServerErrorDialog();
            } else if (this.isConnectionBroken(httpStatus)) {
                this.retryConnectionAfterTimeout();
            }
        },
        isServerError: function (httpStatus) {
            return httpStatus >= HttpStatus.INTERNAL_SERVER_ERROR;
        },
        isConnectionBroken: function (httpStatus) {
            return httpStatus === HttpStatus.CONNECTION_BROKEN;
        },
        isConflict: function (httpStatus) {
            return httpStatus === HttpStatus.CONFLICT;
        },
        isForbidden: function (httpStatus) {
            return httpStatus === HttpStatus.FORBIDDEN;
        },
        isValidationError: function (error) {
            let httpStatus = error.status;
            
            return (httpStatus === HttpStatus.BAD_REQUEST) || (httpStatus === HttpStatus.UNPROCESSABLE_ENTITY);
        },
        isUnauthorizedAttempt: function (httpStatus) {
            return httpStatus === HttpStatus.UNAUTHORIZED;
        }
    }
}