/* ajax查看文本的函数 */
function showTxt(text_id) {
    $.ajax({
        type: "post",
        url: "index.aspx?guide=0&text=1&text_id=" + text_id,
        data: "",
        dataType: "html",/* html返回类型 */
        success: function (result) {
            $(".TxtCol").html($(result).find(".TxtCol").html());

            refre_count_pv(text_id);/* 刷新count_pv计数 */

            if ($.cookie('isLike' + text_id) == 'true') {/* 如果cookie显示目前文本已点赞 */
                $(".LikeBtn").text("✓" + $(result).find(".LikeBtn").text());
            } else {
                $(".LikeBtn").text("点赞" + $(result).find(".LikeBtn").text());
            }
        }
    });
};

/* ajax返回首页的函数 */
function goHome() {
    $.ajax({
        type: "post",
        url: "index.aspx?guide=0&text=1&row=0",
        data: "",
        dataType: "html",/* html返回类型 */
        success: function (result) {
            $(".TxtCol").html($(result).find(".TxtCol").html());/* 以ajax异步请求到的页面.TxtCol替换原有.TxtCol */
        }
    });
};

/* ajax刷新count_like计数的函数 */
function refre_count_like(text_id) {
    if ($.cookie('isLike' + text_id) == 'true') {/* 如果cookie显示目前文本已经点赞 */
        $.cookie('isLike' + text_id, 'false', { expires: 1 });/* 设置为取消点赞，并设置cookie时效（天） */
        $.ajax({
            type: "post",
            contentType: "application/json",
            url: "./services/indexServ.asmx/less_count_like",
            data: "{text_id:" + text_id + "}",
            dataType: "json",/* json返回类型 */
            success: function (result) {
                $(".LikeBtn").text("点赞" + result.d);
            }
        });
    } else {
        $.cookie('isLike' + text_id, 'true', { expires: 1 });
        $.ajax({
            type: "post",
            contentType: "application/json",
            url: "./services/indexServ.asmx/incre_count_like",
            data: "{text_id:" + text_id + "}",
            dataType: "json",/* json返回类型 */
            success: function (result) {
                $(".LikeBtn").text("✓" + result.d);
            }
        });
    }
};
/* ajax刷新count_pv计数的函数 */
function refre_count_pv(text_id) {
    if ($.cookie('isLook' + text_id) == 'true') {/* 如果cookie显示目前文本已经浏览不做处理 */ }
    else {/* 未被浏览 */
        $.cookie('isLook' + text_id, 'true', { expires: 1 });
        $.ajax({
            type: "post",
            contentType: "application/json",
            url: "./services/indexServ.asmx/incre_count_pv",
            data: "{text_id:" + text_id + "}",
            dataType: "json",/* json返回类型 */
            success: function (result) {
                $(".PvLabel").text("阅读" + result.d);
            }
        });
    }
};

/* ajax推进式文本列表加载的函数 */
function loadTxt() {
    $.ajax({
        type: "post",
        url: "index.aspx?guide=0&text=1&row=" + $(".TxtBox").length,
        data: "",
        dataType: "html",/* html返回类型 */
        success: function (result) {
            $("a").remove(".LoadPostBtn");
            $(".TxtCol").html($(".TxtCol").html() + $(result).find(".TxtCol").html());
        }
    });
}

/* markdown转html的函数 */
function mkdConvert(mkdText) {
    var converter = new showdown.Converter();
    $(".TxtContent").html(converter.makeHtml(mkdText));
};