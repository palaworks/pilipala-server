#pragma checksum "H:\博客项目\pilipala2008beta8\Build2\PILIPALA\PILIPALA\Views\Panel\List.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "f2dad439c9fbbf320c2e0cd2b9889ea9be397aa9"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Panel_List), @"mvc.1.0.view", @"/Views/Panel/List.cshtml")]
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
#nullable restore
#line 7 "H:\博客项目\pilipala2008beta8\Build2\PILIPALA\PILIPALA\Views\Panel\List.cshtml"
using PILIPALA.system;

#line default
#line hidden
#nullable disable
#nullable restore
#line 8 "H:\博客项目\pilipala2008beta8\Build2\PILIPALA\PILIPALA\Views\Panel\List.cshtml"
using WaterLibrary.pilipala.Entity;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"f2dad439c9fbbf320c2e0cd2b9889ea9be397aa9", @"/Views/Panel/List.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"0e60be9c90af2170990e47eff766018c8e075941", @"/Views/_ViewImports.cshtml")]
    public class Views_Panel_List : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 2 "H:\博客项目\pilipala2008beta8\Build2\PILIPALA\PILIPALA\Views\Panel\List.cshtml"
  
    ViewBag.Title = "List";
    Layout = ViewBag.Layout;

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
            WriteLiteral("\r\n<!-- 置顶文章输出 -->\r\n");
#nullable restore
#line 11 "H:\博客项目\pilipala2008beta8\Build2\PILIPALA\PILIPALA\Views\Panel\List.cshtml"
 foreach (Post Post in ViewBag.置顶文章)
{
    if (Post.Type == "")
    {

#line default
#line hidden
#nullable disable
            WriteLiteral("        <div class=\"Card M bSha bRds\">\r\n\r\n            <div");
            BeginWriteAttribute("onclick", " onclick=\"", 284, "\"", 317, 3);
            WriteAttributeValue("", 294, "up();showPost(", 294, 14, true);
#nullable restore
#line 17 "H:\博客项目\pilipala2008beta8\Build2\PILIPALA\PILIPALA\Views\Panel\List.cshtml"
WriteAttributeValue("", 308, Post.ID, 308, 8, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 316, ")", 316, 1, true);
            EndWriteAttribute();
            WriteLiteral(" class=\"contain cur\">\r\n                <!-- 前置样式 -->\r\n                ");
#nullable restore
#line 19 "H:\博客项目\pilipala2008beta8\Build2\PILIPALA\PILIPALA\Views\Panel\List.cshtml"
           Write(Html.Raw(Post.Cover));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                <div class=\"Title\">");
#nullable restore
#line 20 "H:\博客项目\pilipala2008beta8\Build2\PILIPALA\PILIPALA\Views\Panel\List.cshtml"
                              Write(Post.Title);

#line default
#line hidden
#nullable disable
            WriteLiteral("</div>\r\n");
#nullable restore
#line 21 "H:\博客项目\pilipala2008beta8\Build2\PILIPALA\PILIPALA\Views\Panel\List.cshtml"
                 if (Post.Summary == "")
                {

#line default
#line hidden
#nullable disable
            WriteLiteral("                    <div class=\"Summary\">");
#nullable restore
#line 23 "H:\博客项目\pilipala2008beta8\Build2\PILIPALA\PILIPALA\Views\Panel\List.cshtml"
                                    Write(Post.TextContent(80));

#line default
#line hidden
#nullable disable
            WriteLiteral("</div>\r\n");
#nullable restore
#line 24 "H:\博客项目\pilipala2008beta8\Build2\PILIPALA\PILIPALA\Views\Panel\List.cshtml"
                }
                else
                {

#line default
#line hidden
#nullable disable
            WriteLiteral("                    <div class=\"Summary\">");
#nullable restore
#line 27 "H:\博客项目\pilipala2008beta8\Build2\PILIPALA\PILIPALA\Views\Panel\List.cshtml"
                                    Write(Post.Summary);

#line default
#line hidden
#nullable disable
            WriteLiteral("</div>\r\n");
#nullable restore
#line 28 "H:\博客项目\pilipala2008beta8\Build2\PILIPALA\PILIPALA\Views\Panel\List.cshtml"
                }

#line default
#line hidden
#nullable disable
            WriteLiteral("            </div>\r\n\r\n            <div class=\"AtBox bRds-b\">\r\n");
#nullable restore
#line 32 "H:\博客项目\pilipala2008beta8\Build2\PILIPALA\PILIPALA\Views\Panel\List.cshtml"
                 if (Formatter.CN_TimeSummary(Post.CT) == null)
                {

#line default
#line hidden
#nullable disable
            WriteLiteral("                    <div class=\"Date\">");
#nullable restore
#line 34 "H:\博客项目\pilipala2008beta8\Build2\PILIPALA\PILIPALA\Views\Panel\List.cshtml"
                                  Write(Convert.ToString(Post.CT.Year) + "-" + Post.CT.Month + "-" + Post.CT.Day);

#line default
#line hidden
#nullable disable
            WriteLiteral("</div>\r\n");
#nullable restore
#line 35 "H:\博客项目\pilipala2008beta8\Build2\PILIPALA\PILIPALA\Views\Panel\List.cshtml"
                }
                else
                {

#line default
#line hidden
#nullable disable
            WriteLiteral("                    <div class=\"Date\">");
#nullable restore
#line 38 "H:\博客项目\pilipala2008beta8\Build2\PILIPALA\PILIPALA\Views\Panel\List.cshtml"
                                 Write(Formatter.CN_TimeSummary(Post.CT));

#line default
#line hidden
#nullable disable
            WriteLiteral("</div>\r\n");
#nullable restore
#line 39 "H:\博客项目\pilipala2008beta8\Build2\PILIPALA\PILIPALA\Views\Panel\List.cshtml"
                }

#line default
#line hidden
#nullable disable
            WriteLiteral("                <div class=\"UVCount\">");
#nullable restore
#line 40 "H:\博客项目\pilipala2008beta8\Build2\PILIPALA\PILIPALA\Views\Panel\List.cshtml"
                                Write(Post.UVCount);

#line default
#line hidden
#nullable disable
            WriteLiteral("</div>\r\n                <div class=\"CommentCount\">");
#nullable restore
#line 41 "H:\博客项目\pilipala2008beta8\Build2\PILIPALA\PILIPALA\Views\Panel\List.cshtml"
                                     Write(Post.PropertyContainer["CommentCount"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</div>\r\n                <div class=\"StarCount\">");
#nullable restore
#line 42 "H:\博客项目\pilipala2008beta8\Build2\PILIPALA\PILIPALA\Views\Panel\List.cshtml"
                                  Write(Post.StarCount);

#line default
#line hidden
#nullable disable
            WriteLiteral("</div>\r\n            </div>\r\n\r\n        </div>\r\n");
#nullable restore
#line 46 "H:\博客项目\pilipala2008beta8\Build2\PILIPALA\PILIPALA\Views\Panel\List.cshtml"
    }
    else if (Post.Type == "note")
    {

#line default
#line hidden
#nullable disable
            WriteLiteral("        <div class=\"Card M bSha bRds\">\r\n            <div class=\"contain cur bRds\"");
            BeginWriteAttribute("onclick", " onclick=\"", 1540, "\"", 1588, 4);
            WriteAttributeValue("", 1550, "up();", 1550, 5, true);
            WriteAttributeValue("\r\n\r\n\r\n                 ", 1555, "(", 1578, 24, true);
#nullable restore
#line 53 "H:\博客项目\pilipala2008beta8\Build2\PILIPALA\PILIPALA\Views\Panel\List.cshtml"
WriteAttributeValue("", 1579, Post.ID, 1579, 8, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 1587, ")", 1587, 1, true);
            EndWriteAttribute();
            WriteLiteral(">\r\n                <!-- 前置样式 -->\r\n                ");
#nullable restore
#line 55 "H:\博客项目\pilipala2008beta8\Build2\PILIPALA\PILIPALA\Views\Panel\List.cshtml"
           Write(Html.Raw(Post.Cover));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n\r\n                <div class=\"NoteContent bRds markdown-body\">\r\n                    ");
#nullable restore
#line 58 "H:\博客项目\pilipala2008beta8\Build2\PILIPALA\PILIPALA\Views\Panel\List.cshtml"
               Write(Html.Raw(Post.HtmlContent()));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                </div>\r\n            </div>\r\n        </div>\r\n");
#nullable restore
#line 62 "H:\博客项目\pilipala2008beta8\Build2\PILIPALA\PILIPALA\Views\Panel\List.cshtml"
    }
}

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n\r\n\r\n<!-- 其他文章输出 -->\r\n");
#nullable restore
#line 68 "H:\博客项目\pilipala2008beta8\Build2\PILIPALA\PILIPALA\Views\Panel\List.cshtml"
 foreach (Post Post in ViewBag.其他文章)
{
    if (Post.Type == "")
    {

#line default
#line hidden
#nullable disable
            WriteLiteral("        <div class=\"Card M bSha bRds\">\r\n\r\n            <div");
            BeginWriteAttribute("onclick", " onclick=\"", 2002, "\"", 2035, 3);
            WriteAttributeValue("", 2012, "up();showPost(", 2012, 14, true);
#nullable restore
#line 74 "H:\博客项目\pilipala2008beta8\Build2\PILIPALA\PILIPALA\Views\Panel\List.cshtml"
WriteAttributeValue("", 2026, Post.ID, 2026, 8, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 2034, ")", 2034, 1, true);
            EndWriteAttribute();
            WriteLiteral(" class=\"contain cur\">\r\n                <!-- 前置样式 -->\r\n                ");
#nullable restore
#line 76 "H:\博客项目\pilipala2008beta8\Build2\PILIPALA\PILIPALA\Views\Panel\List.cshtml"
           Write(Html.Raw(Post.Cover));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                <div class=\"Title\">");
#nullable restore
#line 77 "H:\博客项目\pilipala2008beta8\Build2\PILIPALA\PILIPALA\Views\Panel\List.cshtml"
                              Write(Post.Title);

#line default
#line hidden
#nullable disable
            WriteLiteral("</div>\r\n");
#nullable restore
#line 78 "H:\博客项目\pilipala2008beta8\Build2\PILIPALA\PILIPALA\Views\Panel\List.cshtml"
                 if (Post.Summary == "")
                {

#line default
#line hidden
#nullable disable
            WriteLiteral("                    <div class=\"Summary\">");
#nullable restore
#line 80 "H:\博客项目\pilipala2008beta8\Build2\PILIPALA\PILIPALA\Views\Panel\List.cshtml"
                                    Write(Post.TextContent(80));

#line default
#line hidden
#nullable disable
            WriteLiteral("</div>\r\n");
#nullable restore
#line 81 "H:\博客项目\pilipala2008beta8\Build2\PILIPALA\PILIPALA\Views\Panel\List.cshtml"
                }
                else
                {

#line default
#line hidden
#nullable disable
            WriteLiteral("                    <div class=\"Summary\">");
#nullable restore
#line 84 "H:\博客项目\pilipala2008beta8\Build2\PILIPALA\PILIPALA\Views\Panel\List.cshtml"
                                    Write(Post.Summary);

#line default
#line hidden
#nullable disable
            WriteLiteral("</div>\r\n");
#nullable restore
#line 85 "H:\博客项目\pilipala2008beta8\Build2\PILIPALA\PILIPALA\Views\Panel\List.cshtml"
                }

#line default
#line hidden
#nullable disable
            WriteLiteral("            </div>\r\n\r\n            <div class=\"AtBox bRds-b\">\r\n");
#nullable restore
#line 89 "H:\博客项目\pilipala2008beta8\Build2\PILIPALA\PILIPALA\Views\Panel\List.cshtml"
                 if (Formatter.CN_TimeSummary(Post.CT) == null)
                {

#line default
#line hidden
#nullable disable
            WriteLiteral("                    <div class=\"Date\">");
#nullable restore
#line 91 "H:\博客项目\pilipala2008beta8\Build2\PILIPALA\PILIPALA\Views\Panel\List.cshtml"
                                  Write(Convert.ToString(Post.CT.Year) + "-" + Post.CT.Month + "-" + Post.CT.Day);

#line default
#line hidden
#nullable disable
            WriteLiteral("</div>\r\n");
#nullable restore
#line 92 "H:\博客项目\pilipala2008beta8\Build2\PILIPALA\PILIPALA\Views\Panel\List.cshtml"
                }
                else
                {

#line default
#line hidden
#nullable disable
            WriteLiteral("                    <div class=\"Date\">");
#nullable restore
#line 95 "H:\博客项目\pilipala2008beta8\Build2\PILIPALA\PILIPALA\Views\Panel\List.cshtml"
                                 Write(Formatter.CN_TimeSummary(Post.CT));

#line default
#line hidden
#nullable disable
            WriteLiteral("</div>\r\n");
#nullable restore
#line 96 "H:\博客项目\pilipala2008beta8\Build2\PILIPALA\PILIPALA\Views\Panel\List.cshtml"
                }

#line default
#line hidden
#nullable disable
            WriteLiteral("                <div class=\"UVCount\">");
#nullable restore
#line 97 "H:\博客项目\pilipala2008beta8\Build2\PILIPALA\PILIPALA\Views\Panel\List.cshtml"
                                Write(Post.UVCount);

#line default
#line hidden
#nullable disable
            WriteLiteral("</div>\r\n                <div class=\"CommentCount\">");
#nullable restore
#line 98 "H:\博客项目\pilipala2008beta8\Build2\PILIPALA\PILIPALA\Views\Panel\List.cshtml"
                                     Write(Post.PropertyContainer["CommentCount"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</div>\r\n                <div class=\"StarCount\">");
#nullable restore
#line 99 "H:\博客项目\pilipala2008beta8\Build2\PILIPALA\PILIPALA\Views\Panel\List.cshtml"
                                  Write(Post.StarCount);

#line default
#line hidden
#nullable disable
            WriteLiteral("</div>\r\n            </div>\r\n        </div>\r\n");
#nullable restore
#line 102 "H:\博客项目\pilipala2008beta8\Build2\PILIPALA\PILIPALA\Views\Panel\List.cshtml"
    }
    else if (Post.Type == "note")
    {

#line default
#line hidden
#nullable disable
            WriteLiteral("        <div class=\"Card M bSha bRds\">\r\n            <div class=\"contain cur bRds\"");
            BeginWriteAttribute("onclick", " onclick=\"", 3256, "\"", 3289, 3);
            WriteAttributeValue("", 3266, "up();showPost(", 3266, 14, true);
#nullable restore
#line 106 "H:\博客项目\pilipala2008beta8\Build2\PILIPALA\PILIPALA\Views\Panel\List.cshtml"
WriteAttributeValue("", 3280, Post.ID, 3280, 8, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 3288, ")", 3288, 1, true);
            EndWriteAttribute();
            WriteLiteral(">\r\n                <!-- 前置样式 -->\r\n                ");
#nullable restore
#line 108 "H:\博客项目\pilipala2008beta8\Build2\PILIPALA\PILIPALA\Views\Panel\List.cshtml"
           Write(Html.Raw(Post.Cover));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                <div class=\"NoteContent bRds markdown-body\">\r\n                    ");
#nullable restore
#line 110 "H:\博客项目\pilipala2008beta8\Build2\PILIPALA\PILIPALA\Views\Panel\List.cshtml"
               Write(Html.Raw(Post.HtmlContent()));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                </div>\r\n            </div>\r\n        </div>\r\n");
#nullable restore
#line 114 "H:\博客项目\pilipala2008beta8\Build2\PILIPALA\PILIPALA\Views\Panel\List.cshtml"
    }
}

#line default
#line hidden
#nullable disable
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
