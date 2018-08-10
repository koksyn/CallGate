import CacheService from "../services/CacheService";

export default {
    data: function () {
        return {
            actualThemeColor: 'indigo'
        }
    },
    methods: {
        saveThemeColor: function (event) {
            if (event.pageX !== 0 && event.pageY !== 0) {
                let $element = $(event.target);
                let tagName = $element.prop("tagName");
                
                let $link = (tagName === 'LI') ? $element : $element.parent();
                let color = $link.data('theme');

                this.actualThemeColor = color;
                CacheService.setActiveColor(color);
            }
        },
        reloadThemeColor: function () {
            this.fetchActualThemeColor();
            
            if (CacheService.hasActiveColor()) {
                let color = CacheService.getActiveColor();
                
                if (color !== this.actualThemeColor) {
                    this.changeThemeColorTo(color);
                }
            }
        },
        changeThemeColorTo: function (colorTo) {
            let colorFrom = this.actualThemeColor;

            $('.right-sidebar .theme-choose-skin li').each(function (index, element) {
                let $body = $('body');
                let $link = $(element);
                
                $link.removeClass('active');

                if (colorTo === $link.data('theme')) {
                    $link.addClass('active');
                    
                    $body.removeClass('theme-' + colorFrom);
                    $body.addClass('theme-' + colorTo);
                }
            });    
        },
        fetchActualThemeColor: function () {
            let classes = document.getElementById('global-body').className;
            let color = this.actualThemeColor;

            if (classes.length > 0) {
                classes.split(' ').forEach(function (name) {
                    if(name.startsWith("theme-")) {
                        color = name.substring(6);
                    }                    
                });
            }
            
            this.actualThemeColor = color;
        }
    }
}