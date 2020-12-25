# プロジェクトルートで実行すること
using namespace System.IO;
using namespace System.IO.Compression;

# source directory path (Release directory)
$sourceDir = $Args[0]
if (-not ($sourceDir)) { return 100 }

# target zip file path
$targetZipFile = $Args[1]
if (-not ($targetZipFile)) { return 101 }

# target zip file からディレクトリ名を取得
$parent = [Path]::GetDirectoryName($targetZipFile)
# target zip file を置くディレクトリがない場合は作成する
[Directory]::CreateDirectory($parent)
# 既に zip ファイルがある場合は削除しておく
[File]::Delete($targetZipFile)

# 一時ディレクトリ名
$tempDir = $parent + "\temp"
# Release ディレクトリから一時ディレクトリに丸ごとコピー
Copy-Item $sourceDir -destination $tempDir -recurse

# リリース用に README.md をコピー
Copy-Item "..\README.md" -destination $tempDir -recurse
Copy-Item "..\INSTALL.md" -destination $tempDir -recurse
Copy-Item "..\LICENSE" -destination $tempDir -recurse
Copy-Item "..\COPYRIGHT" -destination $tempDir -recurse

# 一時ディレクトリから不要なファイルを削除(プロジェクトに応じて要変更)
Remove-Item -Recurse -path $tempDir -include *.log
Remove-Item -Recurse -path $tempDir -include *.log.*
Remove-Item -Recurse -path $tempDir -include *.pdb
Remove-Item -Recurse -path $tempDir -include *.xml
Remove-Item -Recurse -path $tempDir -include *.config -Exclude w10wm.exe.Config

# アセンブリの読み込み
Add-Type -AssemblyName System.IO.Compression.FileSystem

# zip ファイル作成
[ZipFile]::CreateFromDirectory($tempDir, $targetZipFile)

# 一時ディレクトリ削除
Remove-Item -Recurse -path $tempDir
