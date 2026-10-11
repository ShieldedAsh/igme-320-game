{
  description = "Unity with Helix flake";

  inputs = {
    nixpkgs.url = "github:NixOS/nixpkgs/nixos-unstable";
    flake-utils.url = "github:numtide/flake-utils";
  };

  outputs = { self, nixpkgs, flake-utils }:
    flake-utils.lib.eachDefaultSystem (system:
      let
        pkgs = import nixpkgs { inherit system; };
        dotnet = pkgs.dotnet-sdk_8;
      in
      {
        devShells.default = pkgs.mkShell {
          packages = [
            dotnet                  
            pkgs.omnisharp-roslyn   
            pkgs.mono               
            pkgs.msbuild            
            pkgs.netcoredbg         
          ];

          DOTNET_ROOT = "${dotnet}/share/dotnet";
          DOTNET_CLI_TELEMETRY_OPTOUT = "1";
          DOTNET_NOLOGO = "1";

          shellHook = ''
            export FrameworkPathOverride="${pkgs.mono}/lib/mono/4.7.1-api"
          '';
        };
      });
}
