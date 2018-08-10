var _tokenName = "D3A77891-8A58-4F25-82D2-B41426ED730A";
var _tokenExpirationName = "4E9A6DDE-4990-4D8A-936F-B5B9C199D514";

function setItem(key, value) {
    if (!value) {
        sessionStorage.removeItem(key);
    } else {
        sessionStorage.setItem(key, value);
    }
}

export default {
    setAuthenticationToken(token) {
        setItem(_tokenName, token);
    },
    getAuthenticationToken() {
        return sessionStorage.getItem(_tokenName);
    },
    setAuthenticationTokenExpirationDate(tokenExpirationDate) {
        setItem(_tokenExpirationName, !tokenExpirationDate ? null : tokenExpirationDate.getTime().toString());
    },
    getAuthenticationTokenExpirationDate() {
        let date = sessionStorage.getItem(_tokenExpirationName);

        if (!date) {
            return null;
        }
        
        return new Date(parseInt(date));
    },
    setObject(key, obj) {
        let serialized = JSON.stringify(obj);
        sessionStorage.setItem(key, serialized);
    },
    getObject(key) {
        let serialized = sessionStorage.getItem(key);

        return (!serialized) ? null : JSON.parse(serialized);
    }
}