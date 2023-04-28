@echo off

set DB_HOST=rolx-database.mariadb.database.azure.com
set DB_USER=theadmin
set CA_FILE=Combined.crt.pem

if not exist %CA_FILE% (
    echo Error: Certificate %CA_FILE% not found
    echo Please follow the instructions in the readme.md to download and create the certificate.
    goto end
)

echo Working on %DB_HOST% with user %DB_USER%
set "psCommand=powershell -Command "$pword = read-host 'Enter password' -AsSecureString ; ^
    $BSTR=[System.Runtime.InteropServices.Marshal]::SecureStringToBSTR($pword); ^
    [System.Runtime.InteropServices.Marshal]::PtrToStringAuto($BSTR)""
for /f "usebackq delims=" %%p in (`%psCommand%`) do set MYSQL_PWD=%%p

echo - dumping production
mysqldump ^
    -u %DB_USER% ^
    --host=%DB_HOST% ^
    --ssl-ca=%CA_FILE% ^
    --single-transaction ^
    --extended-insert ^
    --result-file=rolx_production.sql ^
    rolx_production

echo - clearing staging
mysql ^
    -u %DB_USER% ^
    --host=%DB_HOST% ^
    --ssl-ca=%CA_FILE% ^
    -e "drop database rolx_staging; create database rolx_staging character set utf8mb4 collate utf8mb4_unicode_ci;"

echo - restoring staging
mysql ^
    -u %DB_USER% ^
    --host=%DB_HOST% ^
    --ssl-ca=%CA_FILE% ^
    rolx_staging < rolx_production.sql

echo - locking users
mysql ^
    -u %DB_USER% ^
    --host=%DB_HOST% ^
    --ssl-ca=%CA_FILE% ^
    -D rolx_staging ^
    -e "UPDATE users SET users.IsConfirmed = 0 WHERE users.Role < 1000"

:end
