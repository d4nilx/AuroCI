#!/bin/bash 

mkdir -p ./packages 
echo "Build successfully completed and packages directory created."
echo "------------------------------"

targets=(
    "osx-arm64:macos"
    "win-x64:windows"
    "linux-x64:linux"
)
  
for target in "${targets[@]}"; do
   arch=$(echo "$target" | cut -d ':' -f 1)
   name=$(echo "$target" | cut -d ':' -f 2)
   
    echo "Building for $name ($arch)..."
    
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
                     
    echo "✅ Build for $name ($arch) completed."
    echo "------------------------------"
done

echo "All builds completed successfully. 🎉"