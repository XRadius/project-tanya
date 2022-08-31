#!/bin/bash
rm -rf "bin"

# ====================
# 
# ====================
dotnet publish "src/Tanya/Tanya.csproj" --no-self-contained --output "bin" --runtime linux-x64 \
  "-p:Configuration=Release" \
  "-p:DebugType=None" \
  "-p:GenerateRuntimeConfigurationFiles=true" \
  "-p:PublishSingleFile=true"

# ====================
# 
# ====================
cat > "bin/service-install.sh" << INSTALLER
#!/bin/bash
echo "================================================================================"
echo "This installation script will register a system service. When finished, the name"
echo "of this service is readable by any user. To make sure that it cannot be used for"
echo "detection purposes, you have to enter a random service name. Ensure that it does"
echo "not exist already, and only use characters in [0-9A-Z-]."
echo "================================================================================"
read -p "ServiceName: " serviceName

# ====================
# 
# ====================
rootPath=\$(realpath .)
execPath=\$(realpath "Tanya")
servPath="/etc/systemd/system/\${serviceName}.service"

# ====================
# 
# ====================
cat > \$servPath << EOF
[Unit]
Description=\${serviceName}

[Service]
Type=simple
WorkingDirectory=\${rootPath}
ExecStart=\${execPath}

[Install]
WantedBy=multi-user.target
EOF

# ====================
# 
# ====================
chmod 770 "\$servPath"

# ====================
# 
# ====================
systemctl daemon-reload
systemctl start \${serviceName}
systemctl enable \${serviceName}
INSTALLER

cat > "bin/service-uninstall.sh" << UNINSTALLER
#!/bin/bash
read -p "ServiceName: " serviceName

# ====================
#
# ====================
systemctl disable \${serviceName}
systemctl stop \${serviceName}

# ====================
#
# ====================
rm -rf "/etc/systemd/system/\${serviceName}.service"
UNINSTALLER

# ====================
#
# ====================
chmod +x "bin/service-install.sh"
chmod +x "bin/service-uninstall.sh"
