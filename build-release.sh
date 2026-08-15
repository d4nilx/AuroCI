#!/bin/bash 

# 1. Очищаємо папку від старих збірок, щоб не запушити сміття!
rm -rf ./packages
mkdir -p ./packages 
echo "📦 Folder ./packages is ready."
echo "------------------------------"

targets=(
    "osx-arm64:macos"
    "win-x64:windows"
    "linux-x64:linux"
)
  
for target in "${targets[@]}"; do
    arch="${target%:*}"
    name="${target#*:}"
   
    echo "🔨 Building $name ($arch)..."
    
    dotnet publish AuroCI.CLI \
                     -c Release \
                     -r "$arch" \
                     --self-contained true \
                     -p:PublishSingleFile=true \
                     -p:DebugType=None \
                     -o ./packages
                     
    ext=""
    if [[ "$arch" == *"win"* ]]; then
        ext=".exe"
    fi
    
    mv "./packages/AuroCI.CLI$ext" "./packages/auroci-${name}$ext"
                     
    echo "✅ Building for $name ($arch) completed."
    echo "------------------------------"
done

echo "🎉 All binaries built!"
echo "------------------------------"

read -p "Enter release version (e.g., v1.0.0): " version

echo "🚀 Pushing to GitHub Releases..."
gh release create "$version" ./packages/* --title "AuroCI $version" --notes "New CI/CD generator release"

echo "✅ Release $version successfully published on GitHub!"
echo "------------------------------"

echo "📦 Packing NuGet package..."
dotnet pack AuroCI.CLI -c Release -o ./packages

read -sp "🔑 Enter your NuGet API Key: " nuget_key
echo ""

echo "🚀 Pushing to NuGet.org..."
dotnet nuget push ./packages/*.nupkg -k "$nuget_key" -s https://api.nuget.org/v3/index.json

echo "✅ Package successfully published on NuGet! You are amazing! 😎"