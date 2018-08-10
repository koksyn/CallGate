export default {
    isArray(value) {
        return Array.isArray(value);
    },
    isString(value) {
        return (typeof value === 'string') || (value instanceof String);
    },
    isObject(value) {
        return value === Object(value);
    },
    isEmptyObject(value) {
        return Object.keys(value).length === 0;
    },
    isNotEmptyObject(value) {        
        return Object.keys(value).length !== 0;
    }
}