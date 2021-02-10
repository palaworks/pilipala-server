#pragma checksum "H:\pilipala\pilipala2008beta9\PILIPALA\PILIPALA\Views\Navi\_NaviCol.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "e995fc37c37cd8ebce70b4579ffa6ae963a21745"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Navi__NaviCol), @"mvc.1.0.view", @"/Views/Navi/_NaviCol.cshtml")]
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
#line 1 "H:\pilipala\pilipala2008beta9\PILIPALA\PILIPALA\Views\_ViewImports.cshtml"
using PILIPALA;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "H:\pilipala\pilipala2008beta9\PILIPALA\PILIPALA\Views\_ViewImports.cshtml"
using PILIPALA.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 1 "H:\pilipala\pilipala2008beta9\PILIPALA\PILIPALA\Views\Navi\_NaviCol.cshtml"
using PILIPALA.Theme;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "H:\pilipala\pilipala2008beta9\PILIPALA\PILIPALA\Views\Navi\_NaviCol.cshtml"
using WaterLibrary.pilipala;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "H:\pilipala\pilipala2008beta9\PILIPALA\PILIPALA\Views\Navi\_NaviCol.cshtml"
using WaterLibrary.pilipala.Entity;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "H:\pilipala\pilipala2008beta9\PILIPALA\PILIPALA\Views\Navi\_NaviCol.cshtml"
using WaterLibrary.pilipala.Component;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "H:\pilipala\pilipala2008beta9\PILIPALA\PILIPALA\Views\Navi\_NaviCol.cshtml"
using WaterLibrary.Utils;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"e995fc37c37cd8ebce70b4579ffa6ae963a21745", @"/Views/Navi/_NaviCol.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"0e60be9c90af2170990e47eff766018c8e075941", @"/Views/_ViewImports.cshtml")]
    public class Views_Navi__NaviCol : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
            WriteLiteral("\r\n");
#nullable restore
#line 10 "H:\pilipala\pilipala2008beta9\PILIPALA\PILIPALA\Views\Navi\_NaviCol.cshtml"
  
    Reader Reader;
    Writer Writer;
    Counter Counter;
    CommentLake CommentLake;
    ComponentFactory ComponentFactory = new ComponentFactory();

    CORE.CoreReady += ComponentFactory.Ready;

    

#line default
#line hidden
#nullable disable
#nullable restore
#line 19 "H:\pilipala\pilipala2008beta9\PILIPALA\PILIPALA\Views\Navi\_NaviCol.cshtml"
            
    CORE.Run();

    Reader = ComponentFactory.GenReader(Reader.ReadMode.CleanRead);
    Writer = ComponentFactory.GenWriter();
    Counter = ComponentFactory.GenCounter();
    CommentLake = ComponentFactory.GenCommentLake();

    

#line default
#line hidden
#nullable disable
#nullable restore
#line 27 "H:\pilipala\pilipala2008beta9\PILIPALA\PILIPALA\Views\Navi\_NaviCol.cshtml"
                              
    var archive = ThemeHandler.Config["Navi"]["DefaultArchive"].ToList();
    var REGEXP = ConvertH.ListToString(archive, '|');
    PostSet PostSet = Reader.GetPost(PostProp.ArchiveName, REGEXP, PostProp.PostID, PostProp.Title);

#line default
#line hidden
#nullable disable
            WriteLiteral(@"

<div class=""box bSha bRds-t"">
    <div class=""AvaBox"">
        <div class=""Ava-outline"" v-bind:style=""style"" v-on:click=""click"" id=""Ava-outline"">
            <img class=""Ava bSha cur"" src=""http://q1.qlogo.cn/g?b=qq&nk=1951327599&s=640"">
        </div>
    </div>

    <div class=""Name"">THAUMY的小站</div>
    <div class=""Note""><span id=""note""></span></div>
</div>

<div class=""List"" id=""pcNavList"">
    <div class=""barBox"">
        <transition name=""fade"">
            <div class=""bar"" v-if=""barSeen"" v-bind:style=""style""></div>
        </transition>
    </div>
    <div class=""box"">
        <pc-card v-for=""a in List"" v-bind:item=""a"" v-bind:key=""a.index""></pc-card>
    </div>
</div>

<div class=""Co-outline"" v-bind:style=""style"" v-on:click=""click"" id=""Co-outline"">
    <div class=""Co cur"">
        THAUMY的博客©2016-2020保留所有权利<br>
        基于pilipala构建<br>
        Field Theme Designed By Thaumy<br>
    </div>
</div>

<!-- vue导航组件 -->
<script>
    Vue.component(""pc-card"", {
        props: [");
            WriteLiteral(@"""item""],
        template: '<div class=""Card cur bRds"" v-on:click=""pc_card_click(item.ID)"" :key=""item.index""><div class=""contain"">{{item.Title}}</div></div>',
        methods: {
            pc_card_click: function (ID) {
                up();
                showPost(ID);
                pcNavList.pc_card_line_move(ID);

                AvaOutline.style = {
                    'border-color': 'rgba(0,0,0,0)'
                }
                CoOutline.style = {
                    'border-color': 'rgba(0,0,0,0)'
                }
            }
        }
    })
    var pcNavList = new Vue({
        el: '#pcNavList',
        data: {
            barSeen: true,
            style: {
                top: '0vh'
            },
            List:
            [
                { index: 0, ID: -1, Title: '首页' },
");
#nullable restore
#line 94 "H:\pilipala\pilipala2008beta9\PILIPALA\PILIPALA\Views\Navi\_NaviCol.cshtml"
                   int i = 1;

#line default
#line hidden
#nullable disable
#nullable restore
#line 95 "H:\pilipala\pilipala2008beta9\PILIPALA\PILIPALA\Views\Navi\_NaviCol.cshtml"
                 foreach (Post Post in PostSet)
                {
                    

#line default
#line hidden
#nullable disable
            WriteLiteral("{ index:");
#nullable restore
#line 97 "H:\pilipala\pilipala2008beta9\PILIPALA\PILIPALA\Views\Navi\_NaviCol.cshtml"
                              Write(i++);

#line default
#line hidden
#nullable disable
            WriteLiteral(" , ID:");
#nullable restore
#line 97 "H:\pilipala\pilipala2008beta9\PILIPALA\PILIPALA\Views\Navi\_NaviCol.cshtml"
                                         Write(Post.PostID);

#line default
#line hidden
#nullable disable
            WriteLiteral(", Title:\'");
#nullable restore
#line 97 "H:\pilipala\pilipala2008beta9\PILIPALA\PILIPALA\Views\Navi\_NaviCol.cshtml"
                                                              Write(Html.Raw(Post.Title));

#line default
#line hidden
#nullable disable
            WriteLiteral("\' },");
#nullable restore
#line 97 "H:\pilipala\pilipala2008beta9\PILIPALA\PILIPALA\Views\Navi\_NaviCol.cshtml"
                                                                                                   
                }

#line default
#line hidden
#nullable disable
            WriteLiteral(@"            ]
        },
        methods: {
            pc_card_line_move: function (ID) {
                var lock = 0;
                this.List.forEach((element) => {
                    if (element.ID == ID) {
                        this.barSeen = true;
                        this.style = {
                            top: element.index * 6 + 'vh',
                        }
                        lock = 1;
                    }
                });
                if (lock == 0) {
                    this.barSeen = false;
                }
            }
        }
    })
    var SiderList = new Vue({
        el: '#SiderList',
        data: {
            List: pcNavList.List
        }
    })
</script>

");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public ThemeHandler ThemeHandler { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public ICORE CORE { get; private set; }
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
