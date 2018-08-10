const ActiveGroupIdBag = 'activeGroupId';
const ActiveColorBag = 'activeColor';

export default {
    hasActiveGroupId() {
        return this.getActiveGroupId() !== undefined;
    },
    getActiveGroupId() {
        return sessionStorage.getItem(ActiveGroupIdBag); 
    },
    setActiveGroupId(id) {
        sessionStorage.setItem(ActiveGroupIdBag, id);
    },
    clear() {
        sessionStorage.removeItem(ActiveGroupIdBag);
    },
    hasActiveColor() {
        return this.getActiveColor() !== undefined;
    },
    getActiveColor() {
        return sessionStorage.getItem(ActiveColorBag);
    },
    setActiveColor(color) {
        sessionStorage.setItem(ActiveColorBag, color);
    }
}