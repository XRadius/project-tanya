#!/bin/bash
serviceName="project-tanya"

# ====================
# 
# ====================
dotnet publish "src/Tanya/Tanya.csproj" --output "bin" --runtime linux-x64 --self-contained \
  "-p:Configuration=Release" \
  "-p:DebugType=None" \
  "-p:GenerateRuntimeConfigurationFiles=true" \
  "-p:PublishSingleFile=true"

# ====================
# 
# ====================
if [ $serviceName != "Tanya" ]; then
  mv "bin/Tanya" "bin/${serviceName}"
fi

# ====================
# 
# ====================
"bin/${serviceName}"
