export default {
    disposeAllPickers: function (className = 'pick-me') {
        $('select.' + className).each(function (id, element) {
            let $element = $(element);
            let parent = $element.parent();

            if (parent.hasClass(className)) {
                $(element).selectpicker('destroy');
            }
        });
    },
    invokeAllPickers: function (className = 'pick-me') {
        $('select.' + className).each(function (id, element) {
            let $element = $(element);

            $element.selectpicker('refresh');
            $element.selectpicker('render');
        });
    }
}