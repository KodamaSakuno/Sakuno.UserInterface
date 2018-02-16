param(
    [string]$SolutionDir=$(throw "Parameter missing: -SolutionDir"),
    [string]$ProjectPath=$(throw "Parameter missing: -ProjectPath"),
    [string]$Configuration=$(throw "Parameter missing: -Configuration"),
    [string]$TargetDir=$(throw "Parameter missing: -TargetDir"),
    [string]$TargetPath=$(throw "Parameter missing: -TargetPath")
)

if ($env:DXSDK_DIR -eq $null) {
    $host.SetShouldExit(1)
    Exit-PSHostProcess
}

$compilerPath = Join-Path $env:DXSDK_DIR "Utilities\bin\x86\fxc.exe"

if (-not (Test-Path $compilerPath)) {
    $host.SetShouldExit(1)
    Exit-PSHostProcess
}

$targetLastWriteTime = $null

if (Test-Path $TargetPath) {
    $targetLastWriteTime = (Get-Item $TargetPath).LastWriteTime
}

$effectsDir = Join-Path $SolutionDir "src\Sakuno.UserInterface\Media\Effects"

foreach ($file in Get-ChildItem $effectsDir -Filter "*.fx") {
    if ($targetLastWriteTime -eq $null -or $file.LastWriteTime -gt $targetLastWriteTime) {
        $outputPath = Join-Path $file.Directory.FullName ($file.Basename + ".ps")

        & $compilerPath "/T", "ps_3_0", "/E", "main", "/Fo", $outputPath, $file.FullName | Out-Null

        if ($LASTEXITCODE -ne 0) {
            $host.SetShouldExit($LASTEXITCODE)
            Exit-PSHostProcess
        }
    }
}