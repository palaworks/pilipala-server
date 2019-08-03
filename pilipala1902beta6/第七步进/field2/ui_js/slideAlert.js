; (function ($) {
    'use strict';
    var default_opt = { "element": null, "type": "bottom", "content": "", "time": '1000', "destroy": function () { modal.closeAll(); }, "close": function () { modal.close(); }, "closeTips": function () { modal.closeTips(); }, "beforeOpen": function () { }, "afterOpen": function () { }, "beforeClose": function () { }, "afterClose": function () { }, "shadeClose": true }; var opt = {}; var $modal = null, $shadow = null, $content = null, $tips = null, $body = $('body'), docScrollTop = 0; var html =
        "<div class='sd-alert'>" +
        "   <div class='sd-shadow'></div>" +
        "   <div class='sd-content'>" +
        "       <div id='sd-content'></div>" +
        "   </div>" +
        "</div>";
    var tips_flag = false, t = null; var modal = {
        showModal: function (type) {
            if (tips_flag) { this.closeTips(); }
            if (type === 'tips') {
                tips_flag = true; this.tips(type); if (~~opt.time !== 0) { t = setTimeout(function () { this.closeTips(); }.bind(this), opt.time); }
                return true;
            }
            opt.beforeOpen(); $modal.data('data-type', type); $content.html(opt.content).parent().removeClass('sd-' + type + '-hide').addClass('sd-' + type + '-show'); opt.afterOpen();
        }, tips: function (type) { $tips.data('data-type', type).html(opt.content).removeClass('sd-' + type + '-hide').addClass('sd-' + type + '-show'); }, close: function () {
            opt.beforeClose(); this.closeTips(); var type = $modal.data('data-type'); !!type ? this.closeModal(type) : this.closeAll(); setTimeout(function () {
                $content.unbind().html('').parent().removeClass('sd-bottom-hide sd-top-hide sd-left-hide sd-right-hide sd-alert-hide sd-tips-hide');

                $modal.fadeOut(170); $shadow.fadeOut(170);

                $body.removeClass('overflowHide').css('top', 0); $('html').scrollTop(docScrollTop); document.body.scrollTop = docScrollTop; opt.afterClose();
            }, 300);
        }, closeTips: function () { tips_flag = false; clearTimeout(t); $tips.html('').removeClass('sd-tips-hide sd-tips-show'); }, closeAll: function () { this.closeTips(); $modal.prop('outerHTML', ''); }, closeModal: function (type) { $content.parent().removeClass('sd-' + type + '-show').addClass('sd-' + type + '-hide'); }, valid: function (element, option) {
            var canPass = true; element[0].nodeType !== 1 && (canPass = false, console.error("请传入合适的jquery选择器")); if (!option.type || "top,bottom,left,right,alert,tips".indexOf(option.type.trim()) < 0) { canPass = false; console.error("invalid option.type"); }
            return canPass;
        }
    }; var init = function (option) {
        option.type = option.type.trim(); opt = $.extend({}, default_opt, option);docScrollTop = $('html').scrollTop(); if (docScrollTop <= document.body.scrollTop) { docScrollTop = document.body.scrollTop; }
        if (opt.type !== "alert" && opt.type !== "tips") { $body.addClass('overflowHide').css("top", "-" + docScrollTop + "px"); }

        if (!$modal) { $body.append($('<div></div>')).children().last().prop('outerHTML', html); $modal = $('.sd-alert'); $shadow = $('.sd-shadow'); $content = $('#sd-content'); $tips = $('#sd-tips'); } else { $modal.show(); }
        opt.type !== 'tips' && $shadow.show();

    }; var addEvent = function () { $shadow.off('click.sd').on('click.sd', function (event) { opt.shadeClose && modal.close(); }).off('touchmove').on('touchmove', function (event) { event.preventDefault(); }); }; $.fn.slideAlert = function (option) {
        if (!modal.valid(this, option)) { return false; }
        option.element = this; init(option); addEvent(); modal.showModal(opt.type); return opt;
    }
}($));