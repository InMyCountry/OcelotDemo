using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using StepByStep.Common.Filters;
using StepByStep.Common.Helper;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace StepByStep.Common.Extension
{
    public static class ServiceCollectionExtesions
    {
        /// <summary>
        /// Swagger带版本信息
        /// </summary>
        /// <param name="services"></param>
        public static void AddUserSwaggerWithVersion(this IServiceCollection services)
        {
            // 配置 Swagger 文档信息
            services.AddSwaggerGen(s =>
            {
                // 根据 API 版本信息生成 API 文档
                //
                var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

                foreach (var description in provider.ApiVersionDescriptions)
                {
                    s.SwaggerDoc(description.GroupName, new Info
                    {
                        Contact = new Contact
                        {
                            Name = "联系人",
                            Email = "邮箱",
                            Url = "地址"
                        },
                        Description = "Api.NetCore 接口文档",
                        Title = "Api.NetCore",
                        Version = description.ApiVersion.ToString()
                    });
                }

                // 在 Swagger 文档显示的 API 地址中将版本信息参数替换为实际的版本号
                s.DocInclusionPredicate((version, apiDescription) =>
                {
                    if (!version.Equals(apiDescription.GroupName))
                        return false;

                    var values = apiDescription.RelativePath
                        .Split('/')
                        .Select(v => v.Replace("v{version}", apiDescription.GroupName)); apiDescription.RelativePath = string.Join("/", values);
                    return true;
                });

                // 参数使用驼峰命名方式
                s.DescribeAllParametersInCamelCase();

                // 取消 API 文档需要输入版本信息
                s.OperationFilter<RemoveVersionFromParameter>();

            

                #region 启用swagger验证功能
                //添加一个必须的全局安全信息，和AddSecurityDefinition方法指定的方案名称一致即可，CoreAPI。
                var security = new Dictionary<string, IEnumerable<string>> { { "Jwt授权", new string[] { } }, };
                s.AddSecurityRequirement(security);
                s.AddSecurityDefinition("fff", new ApiKeyScheme
                {
                    Description = "JWT授权(数据将在请求头中进行传输) 在下方输入Bearer {token} 即可，注意两者之间有空格",
                    Name = "Authorization",//jwt默认的参数名称
                    In = "header",//jwt默认存放Authorization信息的位置(请求头中)
                    Type = "apiKey"
                });
                #endregion
                #region 读取xml信息 

                // 获取接口文档描述信息
                //var basePath = Path.GetDirectoryName(AppContext.BaseDirectory);
                //Console.WriteLine(basePath);
                //var apiPath = Path.Combine(basePath, "Api.NetCore.xml");
                //s.IncludeXmlComments(apiPath, true);

                //获取基目录
                var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                //获取所有的xml文件
                string[] files = Directory.GetFiles(baseDirectory, "*.xml");
                foreach (var item in files)
                {
                    //启用xml注释.该方法第二个参数启用控制器的注释，默认为false.
                    s.IncludeXmlComments(item, true);
                }
                #endregion
            });
        }

        /// <summary>
        /// Swagger不带版本信息
        /// </summary>
        /// <param name="services"></param>
        public static void AddUserSwaggerWithoutVersion(this IServiceCollection services)
        {
                        services.AddSwaggerGen(c =>
                        {
                            // 添加文档信息
                            c.SwaggerDoc("v1", new Info
                            {
                                Title = "CoreWebApi",
                                Version = "v1",
                                Description = "ASP.NET CORE WebApi",
                                Contact = new Contact
                                {
                                    Name = "TheAcme",
                                    Email = "admin@163.com"
                                }
                            });
                            #region 读取xml信息         
                            //获取基目录
                            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                            //获取所有的xml文件
                            string[] files = Directory.GetFiles(baseDirectory, "*.xml");
                            foreach (var item in files)
                            {
                                //启用xml注释.该方法第二个参数启用控制器的注释，默认为false.
                                c.IncludeXmlComments(item, true);
                            }
                            #endregion

                            #region 启用swagger验证功能
                            //添加一个必须的全局安全信息，和AddSecurityDefinition方法指定的方案名称一致即可，CoreAPI。
                            var security = new Dictionary<string, IEnumerable<string>> { { "fff", new string[] { } }, };
                            c.AddSecurityRequirement(security);
                            c.AddSecurityDefinition("fff", new ApiKeyScheme
                            {
                                Description = "JWT授权(数据将在请求头中进行传输) 在下方输入Bearer {token} 即可，注意两者之间有空格",
                                Name = "Authorization",//jwt默认的参数名称
                                In = "header",//jwt默认存放Authorization信息的位置(请求头中)
                                Type = "apiKey"
                            });
                            #endregion
                        });
        }

        /// <summary>
        /// JWT安全验证服务添加
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddJwtConfigByUser(this IServiceCollection services)
        {
            #region 添加验证服务
            // 添加验证服务
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    // 是否开启签名认证
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(JwtTokenHelp.secretKey)),
                    // 发行人验证，这里要和token类中Claim类型的发行人保持一致
                    ValidateIssuer = true,
                    ValidIssuer = "API",//发行人
                                        // 接收人验证
                    ValidateAudience = true,
                    ValidAudience = "User",//订阅人
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                };
            });
            #endregion
            return services;
        }

        /// <summary>
        /// 添加 API 版本控制扩展方法 
        /// 需要安装两个类库
        /// Install-Package Microsoft.AspNetCore.Mvc.Versioning
        /// Install-Package Microsoft.AspNetCore.Mvc.Versioning.ApiExplorer
        /// </summary>
        /// <param name="services">生命周期中注入的服务集合 <see cref="IServiceCollection"/></param>
        public static void AddApiVersionConfigByUser(this IServiceCollection services)
        {
            // 添加 API 版本支持
            services.AddApiVersioning(o =>
            {
                // 是否在响应的 header 信息中返回 API 版本信息
                o.ReportApiVersions = true;

                // 默认的 API 版本
                o.DefaultApiVersion = new ApiVersion(1, 0);

                // 未指定 API 版本时，设置 API 版本为默认的版本
                o.AssumeDefaultVersionWhenUnspecified = true;
            });

            // 配置 API 版本信息
            services.AddVersionedApiExplorer(option =>
            {
                // api 版本分组名称
                option.GroupNameFormat = "'v'VVVV";
                // 未指定 API 版本时，设置 API 版本为默认的版本
                option.AssumeDefaultVersionWhenUnspecified = true;
            });
        }
    }
}
