#pragma checksum "H:\博客项目\pilipala2008beta8\Build2\PILIPALA\PILIPALA\Views\doc\foot.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "f9f46900104a0f933c911556fb30c1d608527883"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_doc_foot), @"mvc.1.0.view", @"/Views/doc/foot.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "H:\博客项目\pilipala2008beta8\Build2\PILIPALA\PILIPALA\Views\_ViewImports.cshtml"
using PILIPALA;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "H:\博客项目\pilipala2008beta8\Build2\PILIPALA\PILIPALA\Views\_ViewImports.cshtml"
using PILIPALA.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"f9f46900104a0f933c911556fb30c1d608527883", @"/Views/doc/foot.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"0e60be9c90af2170990e47eff766018c8e075941", @"/Views/_ViewImports.cshtml")]
    public class Views_doc_foot : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/ui/js/more/note.js"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral(@"<!-- vue -->
<script>
    var AvaOutline = new Vue({
        el: '#Ava-outline',
        data: {
            style: {
                'border-color': 'rgba(0,0,0,0)'
            },
        },
        methods: {
            click: function () {
                up();
                showPost(12387);
                this.AvaOutline_change_color(12387);
            },
            AvaOutline_change_color: function (ID) {
                if (ID == 12387) {
                    this.style = {
                        'border-color': 'rgba(1, 153, 255, 1)'
                    };

                } else {
                    this.style = {
                        'border-color': 'rgba(0,0,0,0)'
                    }
                }
            }
        }
    })
    var CoOutline = new Vue({
        el: '#Co-outline',
        data: {
            style: {
                'border-color': 'rgba(0,0,0,0)'
            }
        },
        methods: {
            click: function () {
   ");
            WriteLiteral(@"             up();
                showPost(12388);
                this.CoOutline_change_color(12388);
            },
            CoOutline_change_color: function (ID) {
                if (ID == 12388) {
                    this.style = {
                        'border-color': 'rgba(1, 153, 255, 1)'
                    };

                } else {
                    this.style = {
                        'border-color': 'rgba(0,0,0,0)'
                    }
                }
            }
        }
    })
</script>

<!-- js特效 -->
");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "f9f46900104a0f933c911556fb30c1d6085278835036", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral(@"
<script>
    /* 滑到底部查看壁纸 */
    $(window).scroll(throttle(function () {
        var opacity = document.getElementById('column_bottom').getBoundingClientRect().top / 320;
        document.getElementById('fixed').style.opacity = opacity;
        document.getElementById('NavCol').style.opacity = opacity;
        document.getElementById('CardCol').style.opacity = opacity;
        document.getElementById('IllustCo').style.opacity = opacity * -4;
    }, 10))
    /* 返顶按钮淡入淡出 */
    $(window).scroll(throttle(function () {
        if ($(window).scrollTop() >= 1000) {
            $("".upBtn"").fadeIn(350);
        } else {
            $("".upBtn"").fadeOut(350);
        }
    }, 300))
</script>
<script>
    /* 向上滚动NavLine展开 */
    p = 0;
    t = 0;
    $(window).scroll(debounce(function () {
        /* 触发宽度 */
        if ($(window).width() <= 1000) {
            p = $(this).scrollTop();
            if (t <= p) {
                /* 防止触发遮住文本首部 */
                if ($(window).scrollTop() >= 20) ");
            WriteLiteral(@"{
                    $(""#NavLine"").slideUp(200);
                }
            } else {
                /* 触发高度：小于20或大于200 */
                if ($(window).scrollTop() <= 20 | $(window).scrollTop() >= 200) {
                    $(""#NavLine"").slideDown(150);
                }
            }
            t = p;
        } else {
            $(""#NavLine"").slideUp(200);
        }
    }, 50))

    /* 触发宽度外，收起NavLine */
    $(window).resize(function () {
        if ($(window).width() >= 1001) {
            $(""#NavLine"").slideUp(200);
        } else {
            $(""#NavLine"").slideDown(200);
        }
    });
</script>
<script>
    /* 侧边菜单展开 */
    function ListToggle() {
        if ($(""#SiderList"").css(""left"") == ""-800px"") {
            $(""body"").css(""overflow"", ""hidden""); /* 禁用滚动 */

            fadeInX2("".Shadow"", function () { });
            $("".SiderBtn"").slideUp(300);
            slideRight2X(""#SiderList"");

        }

        if ($(""#SiderList"").css(""left"") == ""12px"") {
  ");
            WriteLiteral(@"          $(""body"").css(""overflow"", ""unset""); /* 启用滚动 */

            fadeOutX2("".Shadow"", function () {
                $("".SiderBtn"").slideDown(400);
            });
            slideLeft2X(""#SiderList"");

        }
    }
</script>
<script>
    /* 历史更改时刷新导航蓝条样式 */
    window.onpopstate = function (event) {
        showPost(event.state == null ? -1 : event.state.ID, false);

        pcNavList.pc_card_line_move(event.state == null ? -1 : event.state.ID);
        AvaOutline.AvaOutline_change_color(event.state == null ? -1 : event.state.ID);
        CoOutline.CoOutline_change_color(event.state == null ? -1 : event.state.ID);
    };
    /* 窗口加载完成时刷新导航蓝条样式 */
    window.onload = function () {
        var path = window.location.pathname.substr(1);

        pcNavList.pc_card_line_move(path == '' ? -1 : path);
        AvaOutline.AvaOutline_change_color(path == '' ? -1 : path);
        CoOutline.CoOutline_change_color(path == '' ? -1 : path);
    }
</script>
");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
