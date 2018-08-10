export default {
    openModal: function (id, color = 'indigo') {
        $('#' + id + ' .modal-content').removeAttr('class').addClass('modal-content modal-col-' + color);
        $('#' + id).modal('show');
    },
    closeModal: function (id) {
        $('#' + id).modal.hide();
    }
}