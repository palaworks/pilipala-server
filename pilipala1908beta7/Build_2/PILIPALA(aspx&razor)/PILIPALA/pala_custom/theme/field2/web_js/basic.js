/* ajax查看文本 */
function showText(text_id) {
    $.ajax({
        type: "post",
        url: "/pala_custom/theme/field2/cut/CONTENT.aspx?text_id=" + text_id,
        data: "",
        dataType: "html",/* html返回类型 */
        success: function (result) {
            $(".CardCol").html($(result).find(".CardCol").html());

            refre_countPv(text_id);/* 刷新count_pv计数 */

            if ($.cookie('isStar' + text_id) == 'true') {/* 如果cookie显示目前文本已点赞 */
                starOpacity100();/* 透明度1 */
                $(".Star").text($(result).find(".Star").text());
            } else {
                starOpacity050();/* 透明度0.5 */
                $(".Star").text($(result).find(".Star").text());
            }
        }
    });
};

/* ajax返回首页 */
function showHome() {
    $.ajax({
        type: "post",
        url: "/pala_custom/theme/field2/cut/LIST.aspx",
        data: "",
        dataType: "html",/* html返回类型 */
        success: function (result) {
            $(".CardCol").html($(result).find(".CardCol").html());/* 以ajax异步请求到的页面.CardCol替换原有.CardCol */
        }
    });
};

/* ajax刷新count_star计数 */
function refre_countStar(text_id) {
    if ($.cookie('isStar' + text_id) == 'true') {/* 如果cookie显示目前文本已经点赞 */
        $.cookie('isStar' + text_id, 'false', { expires: 1 });/* 设置为取消点赞，并设置cookie时效（天） */
        $.ajax({
            type: "post",
            contentType: "application/json",
            url: "/pala_custom/theme/field2/web_service/FieldService.asmx/subs_countStar",
            data: "{text_id:" + text_id + "}",
            dataType: "json",/* json返回类型 */
            success: function (result) {
                starOpacity050();/* 透明度0.5 */
                $(".Star").text(result.d);
            }
        });
    } else {
        $.cookie('isStar' + text_id, 'true', { expires: 1 });
        $.ajax({
            type: "post",
            contentType: "application/json",
            url: "/pala_custom/theme/field2/web_service/FieldService.asmx/plus_countStar",
            data: "{text_id:" + text_id + "}",
            dataType: "json",/* json返回类型 */
            success: function (result) {
                starOpacity100();/* 透明度1 */
                $(".Star").text(result.d);
            }
        });
    }
};

/* ajax刷新count_pv计数 */
function refre_countPv(text_id) {
    if ($.cookie('isSaw' + text_id) == 'true') {/* 如果cookie显示目前文本已经浏览不做处理 */ }
    else {/* 未被浏览 */
        $.cookie('isSaw' + text_id, 'true', { expires: 1 });
        $.ajax({
            type: "post",
            contentType: "application/json",
            url: "/pala_custom/theme/field2/web_service/FieldService.asmx/plus_countPv",
            data: "{text_id:" + text_id + "}",
            dataType: "json",/* json返回类型 */
            success: function (result) {
                $(".Pv").text(result.d);
            }
        });
    }
};

/* ajax推进式文本列表加载 */
function loadText() {
    $.ajax({
        type: "post",
        url: "index.aspx?guide=0&text=1&row=" + $(".content2").length,
        data: "",
        dataType: "html",/* html返回类型 */
        success: function (result) {
            $("a").remove(".LoadPostBtn");
            $(".CardCol").html($(".CardCol").html() + $(result).find(".CardCol").html());
        }
    });
}

/* markdown转html */
function mkdConvert(mkdText) {
    var converter = new showdown.Converter();
    $(".CardCol>.Card>.contain>.Content").html(converter.makeHtml(mkdText));
};

function starOpacity050() {
    $(".AtBox").append("<style id=\"Star_tempstyle\">.CoBox>.AtBox>.Star:before{opacity: 0.5}</style>");
}
function starOpacity100() {
    $("#Star_tempstyle").remove();
}