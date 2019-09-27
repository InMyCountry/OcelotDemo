using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StepByStep.Common.Filters
{
    //  因为我们在之前设置构建的 API 路由时包含了版本信息，所以在最终生成的 Swagger 文档中进行测试时，
    //我们都需要在参数列表中添加 API 版本这个参数。这无疑是有些不方便，所以这里我们可以通过继承 IOperationFilter 接口，
    //控制在生成 API 文档时移除 API 版本参数，接口的实现方法如下所示
    public class RemoveVersionFromParameter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {

            var versionParameter = operation.Parameters.FirstOrDefault(p => p.Name == "version");
            if (versionParameter != null)
            {
                //移除版本参数
                operation.Parameters.Remove(versionParameter);
            }
        }
    }
}
