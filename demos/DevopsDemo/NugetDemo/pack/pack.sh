#!/bin/bash
pwd
current_dir=`pwd` 
#nuget api密钥
nuget_key="aa7890bf-8dfb-33e3-bed9-c1571e5b9b96" 
#托管仓库地址
nuget_source="https://nexus.devops.test.com/repository/nuget-hosted/"
#包的版本
package_version="0.0.15"
#包名
nupkg_pakcage_name="Devops.Common.EvalSDK.${package_version}.nupkg"
#项目库路径
csproj_path="../Devops.Common.EvalSDK/Devops.Common.EvalSDK.csproj"
#包配置
nuspec_path="Devops.Common.EvalSDK.nuspec"
#nuspec path , relative csproj path
nuspec_path_relative_csproj="../pack/Devops.Common.EvalSDK.nuspec"

#git pull
#删除旧版本
rm -f nupkg_pakcage_name
cd ${current_dir} 
#替换版本号
sed -i 's|<version>.*</version>|<version>'${package_version}'</version>|g' ${nuspec_path}

echo pack ${nupkg_pakcage_name}
#打包nupkg文件到当前pack目录 包名.x.x.x.nupkg
dotnet pack ${csproj_path}  -p:NuspecFile=${nuspec_path_relative_csproj} -c Release --output ../pack  -v m


#判断是否打包成功
echo
if [ ! -f "${nupkg_pakcage_name}" ]; then
    echo "pack ${nupkg_pakcage_name} is error"
    exit -1
fi

#推送包
echo push ${nupkg_pakcage_name}
dotnet nuget push ${nupkg_pakcage_name} -k ${nuget_key} -s ${nuget_source}